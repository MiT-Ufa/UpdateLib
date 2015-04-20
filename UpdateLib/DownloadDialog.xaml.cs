using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Windows;

namespace UpdateLib
{
    internal partial class DownloadDialog
    {
        private readonly string _downloadUrl;
        private string _tempPath;
        private WebClient _webClient;

        public DownloadDialog(string downloadUrl)
        {
            InitializeComponent();
            _downloadUrl = downloadUrl;
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            _webClient = new WebClient();

            var uri = new Uri(_downloadUrl);

            _tempPath = Path.Combine(Path.GetTempPath(), GetFileName(_downloadUrl));

            _webClient.DownloadProgressChanged += OnDownloadProgressChanged;

            _webClient.DownloadFileCompleted += OnDownloadComplete;

            _webClient.DownloadFileAsync(uri, _tempPath);
        }

        private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
        }

        private void OnDownloadComplete(object sender, AsyncCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = _tempPath,
                    UseShellExecute = true
                };
                Process.Start(processStartInfo);
                Application.Current.Shutdown();
            }
        }

        private static string GetFileName(string url)
        {
            var fileName = string.Empty;
            var uri = new Uri(url);
            if (uri.Scheme.Equals(Uri.UriSchemeHttp) || uri.Scheme.Equals(Uri.UriSchemeHttps))
            {
                var httpWebRequest = (HttpWebRequest) WebRequest.Create(uri);
                httpWebRequest.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                httpWebRequest.Method = "HEAD";
                httpWebRequest.AllowAutoRedirect = false;
                var httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse();
                if (httpWebResponse.StatusCode.Equals(HttpStatusCode.Redirect) ||
                    httpWebResponse.StatusCode.Equals(HttpStatusCode.Moved) ||
                    httpWebResponse.StatusCode.Equals(HttpStatusCode.MovedPermanently))
                {
                    if (httpWebResponse.Headers["Location"] != null)
                    {
                        var location = httpWebResponse.Headers["Location"];
                        fileName = GetFileName(location);
                        return fileName;
                    }
                }
                var contentDisposition = httpWebResponse.Headers["content-disposition"];
                if (!string.IsNullOrEmpty(contentDisposition))
                {
                    const string lookForFileName = "filename=";
                    var index = contentDisposition.IndexOf(
                        lookForFileName,
                        StringComparison.CurrentCultureIgnoreCase);

                    if (index >= 0)
                    {
                        fileName = contentDisposition.Substring(index + lookForFileName.Length);
                    }
                    if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                    {
                        fileName = fileName.Substring(1, fileName.Length - 2);
                    }
                }
            }
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = Path.GetFileName(uri.LocalPath);
            }
            return fileName;
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            _webClient.CancelAsync();
        }
    }
}