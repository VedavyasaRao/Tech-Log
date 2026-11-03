using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading.Tasks;
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
        private readonly HttpClient client;
        private string bearerToken;

        private const string BaseUrl =
            "https://apis.icedrive.net/v3/webapp/collection&type=cloud&folderId=";

        public IcedriveClient()
        {
            bearerToken = App.bearertoken;

            if (string.IsNullOrWhiteSpace(bearerToken))
                throw new ArgumentException("Bearer token is required.");

            HttpClientHandler handler = new HttpClientHandler();

            handler.AutomaticDecompression =
                DecompressionMethods.GZip |
                DecompressionMethods.Deflate;

            handler.AllowAutoRedirect = false;

            client = new HttpClient(handler);
            client.Timeout = TimeSpan.FromSeconds(60);
        }

        public long CreateFolder(
            long parentFolderId,
            string folderName)
        {
            if (parentFolderId < 0)
                throw new ArgumentException("Invalid folder ID.");

            if (string.IsNullOrWhiteSpace(folderName))
                throw new ArgumentException("Folder name cannot be empty.");

            string url =
                "https://apis.icedrive.net/v3/webapp/folder-create";

            using (HttpRequestMessage request =
                new HttpRequestMessage(HttpMethod.Post, url))
            {
                request.Version = new Version(1, 1);

                // Authorization
                request.Headers.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer", bearerToken);

                // Browser headers
                request.Headers.TryAddWithoutValidation(
                    "User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                    "AppleWebKit/537.36 (KHTML, like Gecko) " +
                    "Chrome/156.0.0.0 Safari/537.36");

                request.Headers.TryAddWithoutValidation(
                    "sec-ch-ua-platform", "\"Windows\"");

                request.Headers.TryAddWithoutValidation(
                    "sec-ch-ua",
                    "\"Not:A-Brand\";v=\"8\", " +
                    "\"Chromium\";v=\"156\", " +
                    "\"Google Chrome\";v=\"156\"");

                request.Headers.TryAddWithoutValidation(
                    "sec-ch-ua-mobile", "?0");

                request.Headers.TryAddWithoutValidation(
                    "DNT", "1");

                request.Headers.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("*/*"));

                request.Headers.TryAddWithoutValidation(
                    "Origin", "https://icedrive.net");

                request.Headers.Referrer =
                    new Uri("https://icedrive.net/");

                request.Headers.TryAddWithoutValidation(
                    "Sec-Fetch-Site", "same-site");

                request.Headers.TryAddWithoutValidation(
                    "Sec-Fetch-Mode", "cors");

                request.Headers.TryAddWithoutValidation(
                    "Sec-Fetch-Dest", "empty");

                request.Headers.TryAddWithoutValidation(
                    "Accept-Language",
                    "en-US,en;q=0.9,kn;q=0.8");

                // Multipart form data
                using (MultipartFormDataContent form =
                    new MultipartFormDataContent())
                {
                    form.Add(
                        new StringContent("folder-create"),
                        "request");

                    form.Add(
                        new StringContent("folder-create"),
                        "type");

                    form.Add(
                        new StringContent("folder-" + parentFolderId),
                        "parentId");

                    form.Add(
                        new StringContent(folderName),
                        "filename");

                    request.Content = form;


                    using (HttpResponseMessage response =
                        client.SendAsync(request).GetAwaiter().GetResult())
                    {
                        string json =
                            response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception(
                                "CreateFolder HTTP Error: " +
                                (int)response.StatusCode +
                                Environment.NewLine +
                                json);
                        }

                        JavaScriptSerializer serializer =
                            new JavaScriptSerializer();

                        IcedriveCreateFolderResponse result =
                            serializer.Deserialize<IcedriveCreateFolderResponse>(json);

                        if (result == null)
                            throw new Exception("Empty CreateFolder response.");

                        if (result.error)
                            throw new Exception(
                                "Icedrive API Error: " + result.message);

                        if (result.folderId <= 0)
                            throw new Exception(
                                "CreateFolder succeeded but returned an invalid folder ID.");

                        return result.folderId;
                    }
                }
            }
        }

        public bool DeleteItem(long itemId, bool bFile)
        {
            if (itemId <= 0)
                throw new ArgumentException("Invalid Item ID.");

            string url =
                "https://apis.icedrive.net/v3/webapp/trash-add";

            using (HttpRequestMessage request =
                new HttpRequestMessage(HttpMethod.Post, url))
            {
                request.Version = new Version(1, 1);

                // Authorization
                request.Headers.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer", bearerToken);

                // Browser-compatible headers
                request.Headers.Connection.Add("keep-alive");

                request.Headers.TryAddWithoutValidation(
                    "User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                    "AppleWebKit/537.36 (KHTML, like Gecko) " +
                    "Chrome/156.0.0.0 Safari/537.36");

                request.Headers.TryAddWithoutValidation(
                    "sec-ch-ua-platform", "\"Windows\"");

                request.Headers.TryAddWithoutValidation(
                    "sec-ch-ua",
                    "\"Not:A-Brand\";v=\"8\", " +
                    "\"Chromium\";v=\"156\", " +
                    "\"Google Chrome\";v=\"156\"");

                request.Headers.TryAddWithoutValidation(
                    "sec-ch-ua-mobile", "?0");

                request.Headers.TryAddWithoutValidation(
                    "DNT", "1");

                request.Headers.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("*/*"));

                request.Headers.TryAddWithoutValidation(
                    "Origin", "https://icedrive.net");

                request.Headers.Referrer =
                    new Uri("https://icedrive.net/");

                request.Headers.TryAddWithoutValidation(
                    "Sec-Fetch-Site", "same-site");

                request.Headers.TryAddWithoutValidation(
                    "Sec-Fetch-Mode", "cors");

                request.Headers.TryAddWithoutValidation(
                    "Sec-Fetch-Dest", "empty");

                request.Headers.TryAddWithoutValidation(
                    "Accept-Language",
                    "en-US,en;q=0.9,kn;q=0.8");

                // Multipart form data
                using (MultipartFormDataContent form =
                    new MultipartFormDataContent())
                {
                    form.Add(
                        new StringContent("trash-add"),
                        "request");

                    form.Add(
                        new StringContent(((bFile)?"file-":"folder-") + itemId),
                        "items");

                    request.Content = form;

                    using (HttpResponseMessage response =
                         client.SendAsync(request).GetAwaiter().GetResult())
                    {
                        string json =
                             response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception(
                                "DeleteItem HTTP Error: " +
                                (int)response.StatusCode +
                                Environment.NewLine +
                                json);
                        }

                        JavaScriptSerializer serializer =
                            new JavaScriptSerializer();

                        var result =
                            serializer.Deserialize<Dictionary<string, object>>(json);

                        if (result == null)
                            throw new Exception(
                                "Empty DeleteItem response.");

                        if (result.ContainsKey("error") &&
                            Convert.ToBoolean(result["error"]))
                        {
                            string message =
                                result.ContainsKey("message")
                                ? Convert.ToString(result["message"])
                                : "Unknown Icedrive error";

                            throw new Exception(
                                "DeleteItem API Error: " + message);
                        }

                        return true;
                    }
                }
            }
        }

        public bool RenameItem(
            long itemID,
            string newItemName, bool bFile)
        {
            if (itemID <= 0)
                throw new ArgumentException("Invalid Item ID.");

            if (string.IsNullOrWhiteSpace(newItemName))
                throw new ArgumentException("Item name cannot be empty.");

            string url =
                $"https://apis.icedrive.net/v3/webapp/{(bFile?"file":"folder")}-rename";

            using (HttpRequestMessage request =
                new HttpRequestMessage(HttpMethod.Post, url))
            {
                request.Version = new Version(1, 1);

                // Authorization
                request.Headers.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer", bearerToken);

                // Browser headers
                request.Headers.TryAddWithoutValidation(
                    "User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                    "AppleWebKit/537.36 (KHTML, like Gecko) " +
                    "Chrome/156.0.0.0 Safari/537.36");

                request.Headers.TryAddWithoutValidation(
                    "sec-ch-ua-platform", "\"Windows\"");

                request.Headers.TryAddWithoutValidation(
                    "sec-ch-ua",
                    "\"Not:A-Brand\";v=\"8\", " +
                    "\"Chromium\";v=\"156\", " +
                    "\"Google Chrome\";v=\"156\"");

                request.Headers.TryAddWithoutValidation(
                    "sec-ch-ua-mobile", "?0");

                request.Headers.TryAddWithoutValidation(
                    "DNT", "1");

                request.Headers.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("*/*"));

                request.Headers.TryAddWithoutValidation(
                    "Origin", "https://icedrive.net");

                request.Headers.Referrer =
                    new Uri("https://icedrive.net/");

                request.Headers.TryAddWithoutValidation(
                    "Sec-Fetch-Site", "same-site");

                request.Headers.TryAddWithoutValidation(
                    "Sec-Fetch-Mode", "cors");

                request.Headers.TryAddWithoutValidation(
                    "Sec-Fetch-Dest", "empty");

                request.Headers.TryAddWithoutValidation(
                    "Accept-Language",
                    "en-US,en;q=0.9,kn;q=0.8");

                // Multipart form data
                using (MultipartFormDataContent form =
                    new MultipartFormDataContent())
                {
                    form.Add(
                        new StringContent($"{(bFile ? "file" : "folder")}-rename"),
                        "request");

                    form.Add(
                        new StringContent(newItemName),
                        "filename");

                    form.Add(
                        new StringContent($"{(bFile ? "file" : "folder")}-{itemID}"),
                        "id");

                    request.Content = form;

                    using (HttpResponseMessage response =
                         client.SendAsync(request).GetAwaiter().GetResult())
                    {
                        string json =
                             response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception(
                                "RenameItem HTTP Error: " +
                                (int)response.StatusCode +
                                Environment.NewLine +
                                json);
                        }

                        JavaScriptSerializer serializer =
                            new JavaScriptSerializer();

                        var result =
                            serializer.Deserialize<Dictionary<string, object>>(json);

                        if (result == null)
                            throw new Exception("Empty RenameItem response.");

                        if (result.ContainsKey("error") &&
                            Convert.ToBoolean(result["error"]))
                        {
                            string message = result.ContainsKey("message")
                                ? Convert.ToString(result["message"])
                                : "Unknown Icedrive error";

                            throw new Exception(
                                "RenameItem API Error: " + message);
                        }

                        return true;
                    }
                }
            }
        }

        private IcedriveResponse GetCollection(long folderId)
        {
            string url = BaseUrl + folderId;

            using (HttpRequestMessage request =
                new HttpRequestMessage(HttpMethod.Get, url))
            {
                request.Version = new Version(1, 1);

                // Authorization
                request.Headers.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer", bearerToken);

                // Connection
                request.Headers.Connection.Add("keep-alive");

                // Browser headers
                request.Headers.TryAddWithoutValidation(
                    "sec-ch-ua-platform", "\"Windows\"");

                request.Headers.TryAddWithoutValidation(
                    "sec-ch-ua",
                    "\"Not:A-Brand\";v=\"8\", " +
                    "\"Chromium\";v=\"156\", " +
                    "\"Google Chrome\";v=\"156\"");

                request.Headers.TryAddWithoutValidation(
                    "sec-ch-ua-mobile", "?0");

                request.Headers.TryAddWithoutValidation(
                    "User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                    "AppleWebKit/537.36 (KHTML, like Gecko) " +
                    "Chrome/156.0.0.0 Safari/537.36");

                // Accept
                request.Headers.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(
                        "application/json"));

                // Cache
                request.Headers.TryAddWithoutValidation(
                    "Cache", "no-cache");

                // DNT
                request.Headers.TryAddWithoutValidation(
                    "DNT", "1");

                // Content-Type header only.
                // IMPORTANT: Do not assign request.Content for GET.
                request.Headers.TryAddWithoutValidation(
                    "Content-Type", "application/json");

                // Origin
                request.Headers.TryAddWithoutValidation(
                    "Origin", "https://icedrive.net");

                // Fetch headers
                request.Headers.TryAddWithoutValidation(
                    "Sec-Fetch-Site", "same-site");

                request.Headers.TryAddWithoutValidation(
                    "Sec-Fetch-Mode", "cors");

                request.Headers.TryAddWithoutValidation(
                    "Sec-Fetch-Dest", "empty");

                // Referer
                request.Headers.Referrer =
                    new Uri("https://icedrive.net/");

                // Accept-Encoding
                request.Headers.TryAddWithoutValidation(
                    "Accept-Encoding", "gzip, deflate");

                // Accept-Language
                request.Headers.TryAddWithoutValidation(
                    "Accept-Language",
                    "en-US,en;q=0.9,kn;q=0.8");

                // IMPORTANT:
                // No request.Content for GET.

                using (HttpResponseMessage response =
                     client.SendAsync(request).GetAwaiter().GetResult())
                {
                    string json =
                         response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception(
                            "Icedrive HTTP Error: " +
                            (int)response.StatusCode +
                            " " + response.ReasonPhrase +
                            Environment.NewLine +
                            json);
                    }

                    JavaScriptSerializer serializer =
                        new JavaScriptSerializer();

                    serializer.MaxJsonLength = int.MaxValue;

                    IcedriveResponse result =
                        serializer.Deserialize<IcedriveResponse>(json);

                    if (result == null)
                        throw new Exception("Empty Icedrive response.");

                    if (result.error)
                    {
                        throw new Exception(
                            "Icedrive API Error: " + result.message);
                    }

                    if (result.data == null)
                        result.data = new List<IcedriveItem>();

                    return result;
                }
            }
        }

        public List<IcedriveItem> ListItems(
            long folderId)
        {
            IcedriveResponse result =
                GetCollection(folderId);

            return result.data           
                .ToList();
        }


        public string UploadFile(
            string localFilePath,
            long parentFolderId,
            string uploadUrl)
        {
            if (!File.Exists(localFilePath))
                throw new FileNotFoundException(
                    "File not found.", localFilePath);

            if (string.IsNullOrWhiteSpace(uploadUrl))
                throw new ArgumentException("Upload URL is required.");

            Uri uri;
            if (!Uri.TryCreate(uploadUrl, UriKind.Absolute, out uri) ||
                uri.Scheme != Uri.UriSchemeHttps)
            {
                throw new ArgumentException("Invalid HTTPS upload URL.");
            }

            // Upload only to the Icedrive storage domain.
            if (!uri.Host.Equals("icedrive.io",
                    StringComparison.OrdinalIgnoreCase) &&
                !uri.Host.EndsWith(".icedrive.io",
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    "Upload URL is not an Icedrive storage endpoint.");
            }

            string filename = Path.GetFileName(localFilePath);

            // Unix timestamp in seconds, including milliseconds.
            DateTime modified =
                File.GetLastWriteTimeUtc(localFilePath);

            double timestamp =
                (modified -
                 new DateTime(1970, 1, 1, 0, 0, 0,
                     DateTimeKind.Utc)).TotalSeconds;

            string moddate = timestamp.ToString(
                "0.###",
                System.Globalization.CultureInfo.InvariantCulture);

            using (HttpRequestMessage request =
                new HttpRequestMessage(HttpMethod.Post, uri))
            {
                request.Version = new Version(1, 1);

                request.Headers.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer", bearerToken);

                request.Headers.TryAddWithoutValidation(
                    "X-Icedrive-Api-Version", "2");

                request.Headers.TryAddWithoutValidation(
                    "Origin", "https://icedrive.net");

                request.Headers.Referrer =
                    new Uri("https://icedrive.net/");

                request.Headers.TryAddWithoutValidation(
                    "User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                    "AppleWebKit/537.36 (KHTML, like Gecko) " +
                    "Chrome/156.0.0.0 Safari/537.36");

                request.Headers.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("*/*"));

                using (MultipartFormDataContent form =
                    new MultipartFormDataContent())
                {
                    form.Add(
                        new StringContent(
                            parentFolderId.ToString()),
                        "folderId");

                    form.Add(
                        new StringContent(moddate),
                        "moddate");

                    using (FileStream stream =
                        new FileStream(
                            localFilePath,
                            FileMode.Open,
                            FileAccess.Read,
                            FileShare.Read))
                    {
                        StreamContent fileContent =
                            new StreamContent(stream);

                        string extension =
                            Path.GetExtension(filename).ToLowerInvariant();

                        string contentType = "application/octet-stream";

                        if (extension == ".txt")
                            contentType = "text/plain";
                        else if (extension == ".html" ||
                                 extension == ".htm")
                            contentType = "text/html";
                        else if (extension == ".json")
                            contentType = "application/json";
                        else if (extension == ".pdf")
                            contentType = "application/pdf";

                        fileContent.Headers.ContentType =
                            new MediaTypeHeaderValue(contentType);

                        form.Add(
                            fileContent,
                            "files[]",
                            filename);

                        request.Content = form;

                        using (HttpResponseMessage response =
                             client.SendAsync(request).GetAwaiter().GetResult())
                        {
                            string result =
                                 response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                            if (!response.IsSuccessStatusCode)
                            {
                                throw new Exception(
                                    "Upload failed. HTTP " +
                                    (int)response.StatusCode +
                                    Environment.NewLine +
                                    result);
                            }

                            return result;
                        }
                    }
                }
            }
        }


        public bool DownloadFile(
            long fileId,
            string localFilePath)
        {
            if (fileId <= 0)
                throw new ArgumentException("Invalid file ID.");

            if (string.IsNullOrWhiteSpace(localFilePath))
                throw new ArgumentException("Invalid destination path.");

            // STEP 1: Get download information
            string json = GetDownloadUrl(fileId);

            JavaScriptSerializer serializer =
                new JavaScriptSerializer();

            DownloadMultiResponse result =
                serializer.Deserialize<DownloadMultiResponse>(json);

            if (result == null)
                throw new Exception("Invalid download response.");

            if (result.error)
                throw new Exception(
                    "Icedrive API error: " + result.message);

            if (result.urls == null || result.urls.Count == 0)
                throw new Exception("No download URL returned.");

            DownloadUrlEntry fileInfo = result.urls[0];

            string downloadUrl = fileInfo.url;

            Uri uri;

            if (!Uri.TryCreate(
                    downloadUrl,
                    UriKind.Absolute,
                    out uri) ||
                uri.Scheme != Uri.UriSchemeHttps)
            {
                throw new Exception("Invalid download URL.");
            }

            if (!uri.Host.Equals("icedrive.io",
                    StringComparison.OrdinalIgnoreCase) &&
                !uri.Host.EndsWith(".icedrive.io",
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception(
                    "Unexpected download server: " + uri.Host);
            }

            // STEP 2: Prepare destination
            string directory =
                Path.GetDirectoryName(Path.GetFullPath(localFilePath));

            Directory.CreateDirectory(directory);

            string temporaryPath = localFilePath + ".part";

            // STEP 3: Download actual file
            try
            {
                using (HttpRequestMessage request =
                    new HttpRequestMessage(HttpMethod.Get, uri))
                {
                    request.Version = new Version(1, 1);

                    request.Headers.TryAddWithoutValidation(
                        "User-Agent",
                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                        "AppleWebKit/537.36 (KHTML, like Gecko) " +
                        "Chrome/156.0.0.0 Safari/537.36");

                    request.Headers.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("*/*"));

                    request.Headers.Referrer =
                        new Uri("https://icedrive.net/");

                    // Do not send Authorization, Origin, or
                    // X-Icedrive-Api-Version for this test.

                    using (HttpResponseMessage response =
                         client.SendAsync(
                            request,
                            HttpCompletionOption.ResponseHeadersRead).GetAwaiter().GetResult())
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            string error =
                                 response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                            /*
                            Console.WriteLine(
                                "Download HTTP Status: " +
                                (int)response.StatusCode);

                            Console.WriteLine(
                                "Reason: " + response.ReasonPhrase);

                            Console.WriteLine(
                                "Content-Type: " +
                                response.Content.Headers.ContentType);

                            Console.WriteLine(
                                "Response Body: " + error);
                            */

                            throw new Exception(
                                "File download failed. HTTP " +
                                (int)response.StatusCode);
                        }

                        using (Stream source =
                             response.Content.ReadAsStreamAsync().GetAwaiter().GetResult())
                        using (FileStream destination =
                            new FileStream(
                                temporaryPath,
                                FileMode.Create,
                                FileAccess.Write,
                                FileShare.None))
                        {
                             source.CopyToAsync(destination);
                        }
                    }
                }

                // STEP 4: Verify downloaded size
                long downloadedSize =
                    new FileInfo(temporaryPath).Length;

                if (downloadedSize != fileInfo.filesize)
                {
                    throw new Exception(
                        "Download size mismatch. Expected " +
                        fileInfo.filesize +
                        " bytes, received " +
                        downloadedSize + " bytes.");
                }

                // STEP 5: Move completed download into place
                if (File.Exists(localFilePath))
                    File.Delete(localFilePath);

                File.Move(temporaryPath, localFilePath);
                /*
                Console.WriteLine(
                    "Download completed: " + localFilePath);

                Console.WriteLine(
                    "File size: " + downloadedSize + " bytes");
                */

                return true;
            }
            catch
            {
                if (File.Exists(temporaryPath))
                    File.Delete(temporaryPath);

                throw;
            }
        }

        private HttpResponseMessage SendWithRetryAsync(
            Func<HttpRequestMessage> createRequest)
        {
            const int maxAttempts = 3;

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                using (HttpRequestMessage request = createRequest())
                {
                    HttpResponseMessage response =
                        client.SendAsync(request).GetAwaiter().GetResult();

                    int status = (int)response.StatusCode;

                    if (status != 522 && status != 502 &&
                        status != 503 && status != 504)
                    {
                        return response;
                    }

                    if (attempt == maxAttempts)
                        return response;

                    response.Dispose();
                    /*
                    Console.WriteLine(
                        "Icedrive temporarily unavailable. HTTP " +
                        status + ". Retrying in 120 seconds...");
                    */
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(120));
                }
            }

            throw new Exception("Retry attempts exhausted.");
        }

        public string GetDownloadUrl(long fileId)
        {
            if (fileId <= 0)
                throw new ArgumentException("Invalid file ID.");

            string url =
                "https://apis.icedrive.net/v3/webapp/download-multi" +
                "&items=file-" + fileId +
                "&crypto=false";

            using (HttpRequestMessage request =
                new HttpRequestMessage(HttpMethod.Get, url))
            {
                request.Version = new Version(1, 1);

                request.Headers.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer", bearerToken);

                request.Headers.TryAddWithoutValidation(
                    "User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                    "AppleWebKit/537.36 (KHTML, like Gecko) " +
                    "Chrome/156.0.0.0 Safari/537.36");

                request.Headers.TryAddWithoutValidation(
                    "sec-ch-ua-platform", "\"Windows\"");

                request.Headers.TryAddWithoutValidation(
                    "sec-ch-ua",
                    "\"Not:A-Brand\";v=\"8\", " +
                    "\"Chromium\";v=\"156\", " +
                    "\"Google Chrome\";v=\"156\"");

                request.Headers.TryAddWithoutValidation(
                    "sec-ch-ua-mobile", "?0");

                request.Headers.TryAddWithoutValidation(
                    "Cache-Control", "no-cache");

                request.Headers.TryAddWithoutValidation(
                    "DNT", "1");

                request.Headers.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(
                        "application/json"));

                request.Headers.TryAddWithoutValidation(
                    "Origin", "https://icedrive.net");

                request.Headers.Referrer =
                    new Uri("https://icedrive.net/");

                request.Headers.TryAddWithoutValidation(
                    "Sec-Fetch-Site", "same-site");

                request.Headers.TryAddWithoutValidation(
                    "Sec-Fetch-Mode", "cors");

                request.Headers.TryAddWithoutValidation(
                    "Sec-Fetch-Dest", "empty");

                request.Headers.TryAddWithoutValidation(
                    "Accept-Language",
                    "en-US,en;q=0.9,kn;q=0.8");

                // Do not set request.Content for GET.

                using (HttpResponseMessage response =
                     SendWithRetryAsync(() =>
                    {
                        HttpRequestMessage retryRequest =
                            new HttpRequestMessage(HttpMethod.Get, url);

                        retryRequest.Version = new Version(1, 1);

                        retryRequest.Headers.Authorization =
                            new AuthenticationHeaderValue(
                                "Bearer", bearerToken);

                        retryRequest.Headers.Accept.Add(
                            new MediaTypeWithQualityHeaderValue(
                                "application/json"));

                        retryRequest.Headers.TryAddWithoutValidation(
                            "User-Agent",
                            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                            "AppleWebKit/537.36 (KHTML, like Gecko) " +
                            "Chrome/156.0.0.0 Safari/537.36");

                        retryRequest.Headers.TryAddWithoutValidation(
                            "Cache-Control", "no-cache");

                        retryRequest.Headers.TryAddWithoutValidation(
                            "Origin", "https://icedrive.net");

                        retryRequest.Headers.Referrer =
                            new Uri("https://icedrive.net/");

                        retryRequest.Headers.TryAddWithoutValidation(
                            "sec-ch-ua-platform", "\"Windows\"");

                        retryRequest.Headers.TryAddWithoutValidation(
                            "sec-ch-ua",
                            "\"Not:A-Brand\";v=\"8\", " +
                            "\"Chromium\";v=\"156\", " +
                            "\"Google Chrome\";v=\"156\"");

                        retryRequest.Headers.TryAddWithoutValidation(
                            "sec-ch-ua-mobile", "?0");

                        retryRequest.Headers.TryAddWithoutValidation(
                            "DNT", "1");

                        retryRequest.Headers.TryAddWithoutValidation(
                            "Sec-Fetch-Site", "same-site");

                        retryRequest.Headers.TryAddWithoutValidation(
                            "Sec-Fetch-Mode", "cors");

                        retryRequest.Headers.TryAddWithoutValidation(
                            "Sec-Fetch-Dest", "empty");

                        return retryRequest;
                    }))
                {
                    string json =
                         response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    /*
                    Console.WriteLine("HTTP Status: " +
                        (int)response.StatusCode);

                    Console.WriteLine("Download API Response:");
                    Console.WriteLine(json);
                    */

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception(
                            "GetDownloadUrl failed. HTTP " +
                            (int)response.StatusCode +
                            Environment.NewLine +
                            json);
                    }

                    return json;
                }
            }
        }

        public bool UploadFile(
            string localFilePath,
            long parentFolderId)
        {
            if (!File.Exists(localFilePath))
                throw new FileNotFoundException(
                    "File not found.", localFilePath);

            string[] endpoints =  GetUploadEndpoints();

            if (endpoints == null || endpoints.Length == 0)
                throw new Exception("No upload endpoints available.");

            // Use the first endpoint initially.
            // Avoid automatic retries that might create duplicate files.
            string uploadUrl = endpoints[0];

            Uri uri;

            if (!Uri.TryCreate(
                    uploadUrl,
                    UriKind.Absolute,
                    out uri) ||
                uri.Scheme != Uri.UriSchemeHttps ||
                !uri.Host.EndsWith(
                    ".icedrive.io",
                    StringComparison.OrdinalIgnoreCase) ||
                uri.AbsolutePath != "/deposit")
            {
                throw new Exception("Invalid upload endpoint.");
            }

            /*
            Console.WriteLine(
                "Uploading to server: " + uri.Host);
            */

            // Call your existing three-argument method.
            string json =  UploadFile(
                localFilePath,
                parentFolderId,
                uploadUrl);

            return true;
        }

        public String[] GetUploadEndpoints()
        {
            string url =
                "https://apis.icedrive.net/v3/webapp/geo-fileserver-list";

            using (HttpResponseMessage response =
                 SendWithRetryAsync(() =>
                {
                    HttpRequestMessage retryRequest =
                        new HttpRequestMessage(HttpMethod.Get, url);

                    retryRequest.Version = new Version(1, 1);

                    retryRequest.Headers.Authorization =
                        new AuthenticationHeaderValue(
                            "Bearer", bearerToken);

                    retryRequest.Headers.Accept.Add(
                        new MediaTypeWithQualityHeaderValue(
                            "application/json"));

                    retryRequest.Headers.TryAddWithoutValidation(
                        "User-Agent",
                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                        "AppleWebKit/537.36 (KHTML, like Gecko) " +
                        "Chrome/156.0.0.0 Safari/537.36");

                    retryRequest.Headers.Referrer =
                        new Uri("https://icedrive.net/");

                    retryRequest.Headers.TryAddWithoutValidation(
                        "Origin", "https://icedrive.net");

                    return retryRequest;
                }))
            {
                string json =
                     response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                /*
                Console.WriteLine(
                    "HTTP Status: " + (int)response.StatusCode);
                */
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(
                        "GetUploadEndpoints failed. HTTP " +
                        (int)response.StatusCode +
                        Environment.NewLine +
                        json);
                }

                JavaScriptSerializer serializer =
                    new JavaScriptSerializer();

                UploadEndpointsResponse result =
                    serializer.Deserialize<UploadEndpointsResponse>(json);

                if (result == null || result.error)
                {
                    throw new Exception(
                        "Upload endpoint request failed: " + json);
                }

                if (result.upload_endpoints == null ||
                    result.upload_endpoints.Length == 0)
                {
                    throw new Exception(
                        "No upload endpoints returned.");
                }

                return result.upload_endpoints;
            }
        }

        public List<IcedriveItem> GetItem(
                 long parentFolderId, long itemId, bool brecursive=false)
        {
            var retlst = new List<IcedriveItem>();
            IcedriveResponse result =
                 GetCollection(parentFolderId);

            if (itemId == -1)
                retlst = result.data.ToList();
            else 
            {
                retlst = result.data.Where(item => item.id == itemId).ToList();
                if (retlst.Count == 0 && brecursive)
                {
                    foreach (var item in result.data)
                    {
                        if (item.isFolder == 1)
                        {
                            retlst =  GetItem(item.id, itemId, brecursive);
                            if (retlst.Count == 1)
                                break;
                        }
                    }
                }
            }
            return retlst;
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }

    
}
