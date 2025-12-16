using BackupRestoreTool;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Incremental_Zip
{
    class PipeServer
    {
        static ArchiveInfo ret = null;

        /// <summary>
        /// Encrypts a file from its path and a plain password.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="password"></param>
        static void FileEncrypt(object src, string outputFile, string password)
        {
            //http://stackoverflow.com/questions/27645527/aes-encryption-on-large-files

            //generate random salt
            byte[] salt = GenerateRandomSalt();

            //create output file name
            System.IO.FileStream fsCrypt = FileStreamEx.FileStream(outputFile, System.IO.FileMode.Create);

            //convert password string to byte arrray
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);

            //Set Rijndael symmetric encryption algorithm
            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;
            AES.Padding = PaddingMode.PKCS7;

            //http://stackoverflow.com/questions/2659214/why-do-i-need-to-use-the-rfc2898derivebytes-class-in-net-instead-of-directly
            //"What it does is repeatedly hash the user password along with the salt." High iteration counts.
            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);

            //Cipher modes: http://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption
            AES.Mode = CipherMode.CFB;

            // write salt to the begining of the output file, so in this case can be random every time
            fsCrypt.Write(salt, 0, salt.Length);

            CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateEncryptor(), CryptoStreamMode.Write);

            System.IO.MemoryStream fsIn = new System.IO.MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fsIn, src);
            }
            catch (System.Runtime.Serialization.SerializationException e)
            {
                App.logit("Failed to serialize. Reason: " + e.Message);
            }
            fsIn.Seek(0, System.IO.SeekOrigin.Begin);
            //create a buffer (1mb) so only this amount will allocate in the memory and not the whole file
            byte[] buffer = new byte[1048576];
            int read;

            try
            {
                while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
                {
                    cs.Write(buffer, 0, read);
                }

                // Close up
                fsIn.Close();
            }
            catch (Exception ex)
            {
                App.logit("Failed to serialize. Reason: " + ex.Message);
            }
            finally
            {
                cs.Close();
                fsCrypt.Close();
            }
        }

        /// <summary>
        /// Creates a random salt that will be used to encrypt your file. This method is required on FileEncrypt.
        /// </summary>
        /// <returns></returns>
        static byte[] GenerateRandomSalt()
        {
            byte[] data = new byte[32];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < 10; i++)
                {
                    // Fille the buffer with the generated data
                    rng.GetBytes(data);
                }
            }

            return data;
        }

        /// <summary>
        /// Decrypts an encrypted file with the FileEncrypt method through its path and the plain password.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFile"></param>
        /// <param name="password"></param>
        static object FileDecrypt(string inputFile, string password)
        {
            object ret = null;
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] salt = new byte[32];

            System.IO.FileStream fsCrypt = FileStreamEx.FileStream(inputFile, System.IO.FileMode.Open);
            fsCrypt.Read(salt, 0, salt.Length);

            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;
            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Padding = PaddingMode.PKCS7;
            AES.Mode = CipherMode.CFB;

            CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read);

            System.IO.MemoryStream fsOut = new System.IO.MemoryStream();

            int read;
            byte[] buffer = new byte[1048576];

            try
            {
                while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fsOut.Write(buffer, 0, read);
                }
            }
            catch (CryptographicException ex_CryptographicException)
            {
                App.logit("CryptographicException error: " + ex_CryptographicException.Message);
            }
            catch (Exception ex)
            {
                App.logit("Error: " + ex.Message);
            }

            try
            {
                cs.Close();
            }
            catch (Exception ex)
            {
                App.logit("Error by closing CryptoStream: " + ex.Message);
            }

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                fsOut.Seek(0, System.IO.SeekOrigin.Begin);
                // Deserialize the hashtable from the file and 
                // assign the reference to the local variable.
                ret = formatter.Deserialize(fsOut);
            }
            catch (Exception e)
            {
                App.logit("Failed to deserialize. Reason: " + e.Message);
            }

            finally
            {
                fsOut.Close();
                fsCrypt.Close();
            }

            return ret;
        }

        static void PipeReader()
        {
            System.IO.MemoryStream fsOut = new System.IO.MemoryStream();
            using (NamedPipeServerStream pipeServer = new NamedPipeServerStream("brInfoK", PipeDirection.In))
            {
                pipeServer.WaitForConnection(); // Wait for a client to connect
                // Use a StreamReader for easy text reading
                using (BinaryReader reader = new BinaryReader(pipeServer))
                {
                    int read;
                    byte[] buffer = new byte[1048576];

                    try
                    {
                        while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fsOut.Write(buffer, 0, read);
                        }
                    }
                    catch (Exception ex)
                    {
                        App.logit("Failed to read. Reason: " + ex.Message);
                    }
                }
            }
            fsOut.Position = 0;

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                ret = (ArchiveInfo) formatter.Deserialize(fsOut);
            }
            catch (Exception e)
            {
                App.logit("Failed to deserialize. Reason: " + e.Message);
            }
        }

        static void PipeWriter(object src)
        {
            System.IO.MemoryStream fsIn = new System.IO.MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fsIn, src);
            }
            catch (System.Runtime.Serialization.SerializationException e)
            {
                App.logit("Failed to serialize. Reason: " + e.Message);
                return;
            }

            using (NamedPipeServerStream pipeServer = new NamedPipeServerStream("brinfoK", PipeDirection.Out))
            {
                pipeServer.WaitForConnection(); // Wait for a client to connect

                using (BinaryWriter writer = new BinaryWriter(pipeServer))
                {
                    writer.Write(fsIn.ToArray());
                }
            }
        }

        static bool iszipfile(string path)
        {

            return false;
        }

        public static void driver(object src, string[] args, out ArchiveInfo tgt)
        {
            tgt = null;
            if (args.Length == 0)
                return;

            //7z a -pabcd  -si -mhe=on  brinfoK.dat < \\.\pipe\brinfoK
            if (args[0] == "a")
            {
                var t = new Thread(PipeWriter);
                t.Start(src);
                System.Threading.Thread.Sleep(5000);

                var fname = Path.GetFileName(args[3]);
                string cmdline = $"/c \"{args[1]}\" a -p{args[2]} -si -mhe=on  {fname} < \\\\.\\pipe\\brinfoK";
                Process.Start("cmd", cmdline).WaitForExit();
                t.Join();
                File.Copy(fname, args[3], true);
                File.Delete(fname);
            }

            //7z e -pabcd -so brinfoK.dat > \\.\pipe\brinfoK
            else if (args[0] == "e")
            {
                var t = new Thread(PipeReader);
                t.Start();
                System.Threading.Thread.Sleep(5000);
                var fname = Path.GetFileName(args[3]);
                File.Copy(args[3], fname, true);
                string cmdline = $"/c \"{args[1]}\" e -p{args[2]} -so  {fname} > \\\\.\\pipe\\brinfoK";
                Process.Start("cmd", cmdline).WaitForExit();
                t.Join();
                File.Delete(fname);
                tgt = ret;
            }
        }
    }
}
