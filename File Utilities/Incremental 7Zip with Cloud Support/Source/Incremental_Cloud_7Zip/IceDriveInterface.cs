using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Globalization;

using System.Security.Cryptography;

using System.Web.Script.Serialization;

namespace BackupRestoreTool
{

    public class IcedriveItem
    {
        public long id { get; set; }
        public string uid { get; set; }
        public string filename { get; set; }
        public long parentId { get; set; }
        public long moddate { get; set; }
        public int isFolder { get; set; }
        public long filesize { get; set; }
        public string fileType { get; set; }
        public string extension { get; set; }
    }

    public class IcedriveResponse
    {
        public bool error { get; set; }
        public long id { get; set; }
        public string access { get; set; }
        public int results { get; set; }
        public string message { get; set; }
        public List<IcedriveItem> data { get; set; }
    }

    public class IcedriveCreateFolderResponse
    {
        public bool error { get; set; }
        public long id { get; set; }
        public long folderId { get; set; }
        public string name { get; set; }
        public string filename { get; set; }
        public long mtime { get; set; }
        public string message { get; set; }
    }

    public class DownloadMultiResponse
    {
        public bool error { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public List<DownloadUrlEntry> urls { get; set; }
    }

    public class DownloadUrlEntry
    {
        public long id { get; set; }
        public string filename { get; set; }
        public long filesize { get; set; }
        public long folderId { get; set; }
        public string path { get; set; }
        public string url { get; set; }
    }

    public class UploadEndpointsResponse
    {
        public bool error { get; set; }

        public string message { get; set; }

        public string[] upload_endpoints { get; set; }
    }


    public class IcedriveClient : IDisposable
    {
        private readonly string bearerToken;
        private const string ApiRoot = "https://apis.icedrive.net/v3/webapp/";
        private const int TimeoutMs = 1800000;
        private const string BrowserUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/156.0.0.0 Safari/537.36";

        public IcedriveClient()
        {
            bearerToken = App.bearertoken;
            if (string.IsNullOrWhiteSpace(bearerToken))
                throw new ArgumentException("Bearer token is required.");
        }

        private static bool IsIcedriveStorage(Uri uri)
        {
            return uri != null && uri.Scheme == Uri.UriSchemeHttps &&
                (uri.Host.Equals("icedrive.io", StringComparison.OrdinalIgnoreCase) ||
                 uri.Host.EndsWith(".icedrive.io", StringComparison.OrdinalIgnoreCase));
        }

        private HttpWebRequest CreateRequest(string url, string method, bool authorized)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            request.ProtocolVersion = HttpVersion.Version11;
            request.Timeout = TimeoutMs;
            request.ReadWriteTimeout = TimeoutMs;
            request.AllowAutoRedirect = false;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.UserAgent = BrowserUserAgent;
            request.Accept = "*/*";
            request.Referer = "https://icedrive.net/";
            request.KeepAlive = true;
            request.Headers[HttpRequestHeader.AcceptLanguage] = "en-US,en;q=0.9,kn;q=0.8";
            request.Headers["Origin"] = "https://icedrive.net";
            if (authorized)
                request.Headers[HttpRequestHeader.Authorization] = "Bearer " + bearerToken;
            return request;
        }

        private static string ReadResponse(HttpWebResponse response)
        {
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                return reader.ReadToEnd();
        }

        private static HttpWebResponse GetResponse(HttpWebRequest request)
        {
            try { return (HttpWebResponse)request.GetResponse(); }
            catch (WebException ex)
            {
                HttpWebResponse errorResponse = ex.Response as HttpWebResponse;
                if (errorResponse == null) throw;
                using (errorResponse)
                {
                    string details = ReadResponse(errorResponse);
                    throw new IOException("HTTP " + (int)errorResponse.StatusCode +
                        " " + errorResponse.StatusDescription + ": " + details, ex);
                }
            }
        }

        private static string RequestText(HttpWebRequest request)
        {
            using (HttpWebResponse response = GetResponse(request))
            {
                if (response.StatusCode != HttpStatusCode.OK &&
                    response.StatusCode != HttpStatusCode.Created &&
                    response.StatusCode != HttpStatusCode.NoContent)
                    throw new IOException("Unexpected HTTP status " + (int)response.StatusCode);
                return ReadResponse(response);
            }
        }

        private static string EscapeHeader(string value)
        {
            return value.Replace("\\", "_").Replace("\"", "_").Replace("\r", "_").Replace("\n", "_");
        }

        private static void WriteUtf8(Stream stream, string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            stream.Write(bytes, 0, bytes.Length);
        }

        // Stream multipart bodies without buffering the entire file in memory.
        private string PostMultipart(string url, Dictionary<string, string> fields,
            string filePath = null, bool storageEndpoint = false)
        {
            HttpWebRequest request = CreateRequest(url, "POST", true);
            if (storageEndpoint) request.Headers["X-Icedrive-Api-Version"] = "2";
            string boundary = "----IcedriveBoundary" + Guid.NewGuid().ToString("N");
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.AllowWriteStreamBuffering = false;
            // Chunked transfer is supported by HTTP/1.1; some storage endpoints may
            // require Content-Length, so calculate it instead.
            byte[] prefix;
            using (MemoryStream buffer = new MemoryStream())
            {
                foreach (KeyValuePair<string, string> field in fields)
                {
                    WriteUtf8(buffer, "--" + boundary + "\r\nContent-Disposition: form-data; name=\"" +
                        EscapeHeader(field.Key) + "\"\r\n\r\n" + field.Value + "\r\n");
                }
                if (filePath != null)
                {
                    string filename = EscapeHeader(Path.GetFileName(filePath));
                    string ext = Path.GetExtension(filename).ToLowerInvariant();
                    string mime = ext == ".txt" ? "text/plain" :
                        (ext == ".htm" || ext == ".html") ? "text/html" :
                        ext == ".json" ? "application/json" :
                        ext == ".pdf" ? "application/pdf" : "application/octet-stream";
                    WriteUtf8(buffer, "--" + boundary + "\r\nContent-Disposition: form-data; name=\"files[]\"; filename=\"" +
                        filename + "\"\r\nContent-Type: " + mime + "\r\n\r\n");
                }
                prefix = buffer.ToArray();
            }
            byte[] suffix = Encoding.UTF8.GetBytes((filePath != null ? "\r\n" : "") + "--" + boundary + "--\r\n");
            long fileLength = filePath == null ? 0 : new FileInfo(filePath).Length;
            request.ContentLength = prefix.LongLength + fileLength + suffix.LongLength;
            using (Stream destination = request.GetRequestStream())
            {
                destination.Write(prefix, 0, prefix.Length);
                if (filePath != null)
                    using (FileStream source = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                        source.CopyTo(destination);
                destination.Write(suffix, 0, suffix.Length);
            }
            return RequestText(request);
        }

        private static Dictionary<string, object> ParseSuccess(string json, string operation)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
            Dictionary<string, object> data = serializer.Deserialize<Dictionary<string, object>>(json);
            if (data == null) throw new IOException(operation + ": empty JSON response");
            object error;
            if (data.TryGetValue("error", out error) && Convert.ToBoolean(error))
                throw new IOException(operation + ": " +
                    (data.ContainsKey("message") ? Convert.ToString(data["message"]) : json));
            return data;
        }

        public long CreateFolder(long parentFolderId, string folderName)
        {
            if (parentFolderId < 0) throw new ArgumentException("Invalid folder ID.");
            if (string.IsNullOrWhiteSpace(folderName)) throw new ArgumentException("Folder name cannot be empty.");
            string json = PostMultipart(ApiRoot + "folder-create", new Dictionary<string, string>
            {
                {"request", "folder-create"}, {"type", "folder-create"},
                {"parentId", "folder-" + parentFolderId}, {"filename", folderName}
            });
            ParseSuccess(json, "CreateFolder");
            IcedriveCreateFolderResponse result = new JavaScriptSerializer().Deserialize<IcedriveCreateFolderResponse>(json);
            if (result == null || result.folderId <= 0) throw new IOException("CreateFolder returned an invalid folder ID.");
            return result.folderId;
        }

        public bool DeleteItem(long itemId, bool bFile)
        {
            if (itemId <= 0) throw new ArgumentException("Invalid item ID.");
            string json = PostMultipart(ApiRoot + "trash-add", new Dictionary<string, string>
            {
                {"request", "trash-add"}, {"items", (bFile ? "file-" : "folder-") + itemId}
            });
            ParseSuccess(json, "DeleteItem");
            return true;
        }

        public bool RenameItem(long itemID, string newItemName, bool bFile)
        {
            if (itemID <= 0) throw new ArgumentException("Invalid item ID.");
            if (string.IsNullOrWhiteSpace(newItemName)) throw new ArgumentException("Item name cannot be empty.");
            string kind = bFile ? "file" : "folder";
            string json = PostMultipart(ApiRoot + kind + "-rename", new Dictionary<string, string>
            {
                {"request", kind + "-rename"}, {"filename", newItemName}, {"id", kind + "-" + itemID}
            });
            ParseSuccess(json, "RenameItem");
            return true;
        }

        private IcedriveResponse GetCollection(long folderId)
        {
            HttpWebRequest request = CreateRequest(ApiRoot + "collection&type=cloud&folderId=" + folderId, "GET", true);
            request.Accept = "application/json";
            string json = RequestText(request);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
            IcedriveResponse result = serializer.Deserialize<IcedriveResponse>(json);
            if (result == null || result.error)
                throw new IOException("GetCollection: " + (result == null ? "empty response" : result.message));
            if (result.data == null) result.data = new List<IcedriveItem>();
            return result;
        }

        public List<IcedriveItem> ListItems(long folderId)
        {
            return GetCollection(folderId).data.ToList();
        }

        public string UploadFile(string localFilePath, long parentFolderId, string uploadUrl)
        {
            if (!File.Exists(localFilePath)) throw new FileNotFoundException("File not found.", localFilePath);
            Uri uri;
            if (!Uri.TryCreate(uploadUrl, UriKind.Absolute, out uri) || !IsIcedriveStorage(uri) ||
                uri.AbsolutePath != "/deposit")
                throw new ArgumentException("Invalid Icedrive upload URL.");
            DateTime modified = File.GetLastWriteTimeUtc(localFilePath);
            double seconds = (modified - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            string json = PostMultipart(uploadUrl, new Dictionary<string, string>
            {
                {"folderId", parentFolderId.ToString(CultureInfo.InvariantCulture)},
                {"moddate", seconds.ToString("0.###", CultureInfo.InvariantCulture)}
            }, localFilePath, true);
            ParseSuccess(json, "UploadFile");
            return json;
        }

        public bool DownloadFile(long fileId, string localFilePath)
        {
            if (fileId <= 0) throw new ArgumentException("Invalid file ID.");
            if (string.IsNullOrWhiteSpace(localFilePath)) throw new ArgumentException("Invalid destination path.");
            string json = GetDownloadUrl(fileId);
            DownloadMultiResponse result = new JavaScriptSerializer().Deserialize<DownloadMultiResponse>(json);
            if (result == null || result.error || result.urls == null || result.urls.Count == 0)
                throw new IOException("Invalid Icedrive download response.");
            DownloadUrlEntry info = result.urls[0];
            if (info.id != 0 && info.id != fileId)
                throw new IOException("Download response file ID mismatch.");
            Uri uri;
            if (!Uri.TryCreate(info.url, UriKind.Absolute, out uri) || !IsIcedriveStorage(uri))
                throw new IOException("Invalid Icedrive download URL.");
            string directory = Path.GetDirectoryName(Path.GetFullPath(localFilePath));
            Directory.CreateDirectory(directory);
            string temporaryPath = localFilePath + ".part";
            try
            {
                HttpWebRequest request = CreateRequest(info.url, "GET", false);
                // Avoid automatic decompression for binary storage files; compare raw byte counts.
                request.AutomaticDecompression = DecompressionMethods.None;
                using (HttpWebResponse response = GetResponse(request))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new IOException("Unexpected download HTTP status: " + (int)response.StatusCode);
                    if (response.ContentType.StartsWith("text/html", StringComparison.OrdinalIgnoreCase) ||
                        response.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
                        throw new IOException("Download returned " + response.ContentType + " instead of file data.");
                    using (Stream source = response.GetResponseStream())
                    using (FileStream destination = new FileStream(temporaryPath, FileMode.Create, FileAccess.Write, FileShare.None))
                        source.CopyTo(destination);
                    long actual = new FileInfo(temporaryPath).Length;
                    if (response.ContentLength >= 0 && actual != response.ContentLength)
                        throw new IOException("HTTP download size mismatch: " + actual + " vs " + response.ContentLength);
                    if (actual != info.filesize)
                        throw new IOException("Icedrive download size mismatch: " + actual + " vs " + info.filesize);
                }
                if (File.Exists(localFilePath)) File.Delete(localFilePath);
                File.Move(temporaryPath, localFilePath);
                return true;
            }
            catch
            {
                // Preserve the partial file to aid diagnosis; delete manually after inspection.
                throw;
            }
        }

        // Retry transient HTTP errors. Never retry uploads automatically (duplicate risk).
        private string GetTextWithRetry(string url)
        {
            const int maxAttempts = 3;
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                HttpWebRequest request = CreateRequest(url, "GET", true);
                request.Accept = "application/json";
                try { return RequestText(request); }
                catch (IOException ex)
                {
                    string msg = ex.Message;
                    bool transient = msg.StartsWith("HTTP 522 ") || msg.StartsWith("HTTP 502 ") ||
                        msg.StartsWith("HTTP 503 ") || msg.StartsWith("HTTP 504 ");
                    if (!transient || attempt == maxAttempts) throw;
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(120));
                }
            }
            throw new IOException("Retry attempts exhausted.");
        }

        public string GetDownloadUrl(long fileId)
        {
            if (fileId <= 0) throw new ArgumentException("Invalid file ID.");
            string url = ApiRoot + "download-multi&items=file-" + fileId + "&crypto=false";
            string json = GetTextWithRetry(url);
            ParseSuccess(json, "GetDownloadUrl");
            return json;
        }

        public bool UploadFile(string localFilePath, long parentFolderId)
        {
            if (!File.Exists(localFilePath)) throw new FileNotFoundException("File not found.", localFilePath);
            string[] endpoints = GetUploadEndpoints();
            if (endpoints == null || endpoints.Length == 0) throw new IOException("No upload endpoints available.");
            UploadFile(localFilePath, parentFolderId, endpoints[0]);
            return true;
        }

        public string[] GetUploadEndpoints()
        {
            string json = GetTextWithRetry(ApiRoot + "geo-fileserver-list");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
            UploadEndpointsResponse result = serializer.Deserialize<UploadEndpointsResponse>(json);
            if (result == null || result.error || result.upload_endpoints == null || result.upload_endpoints.Length == 0)
                throw new IOException("GetUploadEndpoints failed: " + json);
            return result.upload_endpoints;
        }

        public List<IcedriveItem> GetItem(long parentFolderId, long itemId, bool brecursive = false)
        {
            IcedriveResponse result = GetCollection(parentFolderId);
            if (itemId == -1) return result.data.ToList();
            List<IcedriveItem> matches = result.data.Where(item => item.id == itemId).ToList();
            if (matches.Count == 0 && brecursive)
            {
                foreach (IcedriveItem item in result.data)
                {
                    if (item.isFolder != 1) continue;
                    matches = GetItem(item.id, itemId, true);
                    if (matches.Count > 0) break;
                }
            }
            return matches;
        }

        public void Dispose() { }
    }
}
