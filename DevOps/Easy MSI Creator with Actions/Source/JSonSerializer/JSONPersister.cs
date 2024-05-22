using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;

namespace JSonSerializer
{
    #region JSONFormatter
    /// <summary>StringWalker</summary>
    class StringWalker
    {
        string sS;

        /// <summary>Index</summary>
        public int Index { get; set; }

        /// <summary>StringWalker</summary>
        public StringWalker(string s)
        {
            sS = s;
            Index = -1;
        }

        /// <summary>MoveNext</summary>
        public bool MoveNext()
        {
            bool bRet = false;
            if (Index != sS.Length - 1)
            {
                bRet = true; ;
                Index++;
            }
            return bRet;
        }

        /// <summary>CharAtIndex</summary>
        public char CharAtIndex()
        {
            return sS[Index];
        }
    };

    /// <summary>IndentWriter</summary>
    class IndentWriter
    {
        StringBuilder sbSB = new StringBuilder();
        int iIndent;

        /// <summary>Indent</summary>
        public void Indent()
        {
            iIndent++;
        }

        /// <summary>UnIndent</summary>
        public void UnIndent()
        {
            if (iIndent > 0)
            {
                iIndent--;
            }
        }

        /// <summary>WriteLine</summary>
        public void WriteLine(string line)
        {
            sbSB.AppendLine(CreateIndent() + line);
        }

        /// <summary>CreateIndent</summary>
        private string CreateIndent()
        {
            StringBuilder indentString = new StringBuilder();
            for (int i = 0; i < iIndent; i++)
            {
                indentString.Append("    ");
            }
            return indentString.ToString();
        }

        /// <summary>ToString</summary>
        public override string ToString()
        {
            return sbSB.ToString();
        }
    };

    /// <summary>JSONFormatter</summary>
    class JSONFormatter
    {
        StringWalker walker;
        IndentWriter writer = new IndentWriter();
        StringBuilder currentLine = new StringBuilder();
        bool quoted;

        /// <summary>JSONFormatter</summary>
        public JSONFormatter(string json)
        {
            walker = new StringWalker(json);
            ResetLine();
        }

        /// <summary>ResetLine</summary>
        public void ResetLine()
        {
            currentLine.Length = 0;
        }

        /// <summary>Format</summary>
        public string Format()
        {
            while (MoveNextChar())
            {
                if (!this.quoted && this.IsOpenBracket())
                {
                    this.WriteCurrentLine();
                    this.AddCharToLine();
                    this.WriteCurrentLine();
                    writer.Indent();
                }
                else if (!this.quoted && this.IsCloseBracket())
                {
                    this.WriteCurrentLine();
                    writer.UnIndent();
                    this.AddCharToLine();
                }
                else if (!this.quoted && this.IsColon())
                {
                    this.AddCharToLine();
                    this.WriteCurrentLine();
                }
                else
                {
                    AddCharToLine();
                }
            }
            this.WriteCurrentLine();
            return writer.ToString();
        }

        private bool MoveNextChar()
        {
            bool success = walker.MoveNext();
            if (this.IsApostrophe())
            {
                this.quoted = !quoted;
            }
            return success;
        }

        /// <summary></summary>
        public bool IsApostrophe()
        {
            return this.walker.CharAtIndex() == '"';
        }

        /// <summary></summary>
        public bool IsOpenBracket()
        {
            return this.walker.CharAtIndex() == '{'
                || this.walker.CharAtIndex() == '[';
        }

        /// <summary></summary>
        public bool IsCloseBracket()
        {
            return this.walker.CharAtIndex() == '}'
                || this.walker.CharAtIndex() == ']';
        }

        /// <summary></summary>
        public bool IsColon()
        {
            return this.walker.CharAtIndex() == ',';
        }

        private void AddCharToLine()
        {
            this.currentLine.Append(walker.CharAtIndex());
        }

        private void WriteCurrentLine()
        {
            string line = this.currentLine.ToString().Trim();
            if (line.Length > 0)
            {
                writer.WriteLine(line);
            }
            this.ResetLine();
        }
    };
    #endregion

    #region .NET JSON Persister 
    /// <summary>JSONPersister</summary>
    public static class JSONPersister<T>
    {

        /// <summary>Write</summary>
        public static void Write(string filepath, T objinsance)
        {
            File.WriteAllText(filepath, GetJSON(objinsance));
        }

        /// <summary>GetJSON</summary>
        public static string GetJSON(T objinsance)
        {
            return new JSONFormatter(new JavaScriptSerializer().Serialize(objinsance)).Format();
        }

        public static string GetRawJSON(T objinsance)
        {
            return new JavaScriptSerializer().Serialize(objinsance);
        }


        /// <summary>SetJSON</summary>
        public static T SetJSON(string buffer)
        {
            T objinsance = default(T);
            objinsance = new JavaScriptSerializer().Deserialize<T>(buffer);
            return objinsance;
        }

        /// <summary>Read</summary>
        public static T Read(string filepath)
        {
            T objinsance = default(T);
            if (File.Exists(filepath))
            {
                objinsance = new JavaScriptSerializer().Deserialize<T>(File.ReadAllText(filepath));
            }
            return objinsance;
        }
    }
    #endregion


}
