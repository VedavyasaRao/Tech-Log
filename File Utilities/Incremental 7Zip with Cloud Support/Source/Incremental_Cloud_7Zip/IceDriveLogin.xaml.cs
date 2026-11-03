using System;
using System.Windows;
using Microsoft.Web.WebView2.Core;

namespace BackupRestoreTool
{
    public partial class IceDriveLogin : Window
    {
        public string BearerToken { get; private set; }
        public bool LoginSuccessful { get; private set; }

        public IceDriveLogin()
        {
            InitializeComponent();

            Loaded += IceDriveLogin_Loaded;
        }

        private async void IceDriveLogin_Loaded(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                await Browser.EnsureCoreWebView2Async();

                Browser.CoreWebView2
                    .AddWebResourceRequestedFilter(
                        "https://apis.icedrive.net/*",
                        CoreWebView2WebResourceContext.All);

                Browser.CoreWebView2.WebResourceRequested +=
                    CoreWebView2_WebResourceRequested;

                Browser.CoreWebView2.Navigate(
                    "https://icedrive.net/dashboard/#/cloud");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Login Error");
            }
        }

        private void CoreWebView2_WebResourceRequested(
            object sender,
            CoreWebView2WebResourceRequestedEventArgs e)
        {
            try
            {
                string authorization;

                if (!e.Request.Headers.Contains("Authorization"))
                    return;

                authorization =
                    e.Request.Headers.GetHeader("Authorization");

                if (string.IsNullOrWhiteSpace(authorization))
                    return;

                if (!authorization.StartsWith(
                    "Bearer ",
                    StringComparison.OrdinalIgnoreCase))
                    return;

                BearerToken = authorization.Substring(7).Trim();

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    LoginSuccessful = true;

                    Console.WriteLine("Icedrive authentication successful.");

                    Close();
                }));

            }
            catch (Exception)
            {
                // Ignore requests without usable headers.
            }
        }
    }
}