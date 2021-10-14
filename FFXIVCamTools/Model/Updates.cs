using System;
using System.Windows;
using System.Net;
using System.IO;
using System.Windows.Threading;
using System.Threading.Tasks;
using FFXIVCamTools.View;

namespace FFXIVCamTools.Model
{
    class Updates
    {
        public event EventHandler UpdateFinished;

        public string Url;
        public string AppPath;

        public Updates()
        {

        }

        public void Run()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            try
            {
                WebClient webClient = new WebClient();
                Uri uri = new Uri(Url);

                webClient.DownloadDataCompleted += WebClient_DownloadDataCompleted;
                webClient.DownloadDataAsync(uri);
            }
            catch (Exception ex)
            {
                MessageDialog.ShowMessageDialog("Alert", "Error", ex.Message, (o, e) => { Application.Current.Shutdown(); });
            } 
        }

        private void Update(Settings source, Settings dest)
        {
            dest.StructureAddress = source.StructureAddress;
            dest.ZoomCurrent = source.ZoomCurrent;
            dest.ZoomMax = source.ZoomMax;
            dest.FovCurrent = source.FovCurrent;
            dest.FovMax = source.FovMax;
            dest.LastUpdate = source.LastUpdate;
        }

        private void ShowMessageDialog(string iconKind, string title, string message, EventHandler closedEvent)
        {
            MessageDialog messageDialog = new MessageDialog();

            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                messageDialog.Closed += closedEvent;
                messageDialog.Initialize(iconKind, title, message);
            });
        }

        private void WebClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                //Connection timed out
                MessageDialog.ShowMessageDialog("Alert", "Error", "Connection timed out", (o, e) => { Application.Current.Shutdown(); });
            }
            else if (e.Error != null)
            {
                //WebClient Exception
                MessageDialog.ShowMessageDialog("Alert", "Error", e.Error.Message, (o, e) => { Application.Current.Shutdown(); });
            }
            else
            {
                string settingsPath = Path.Combine(AppPath, "Settings.xml");
                string updatesPath = Path.Combine(AppPath, "Updates.xml");

                //File and update check
                if (!File.Exists(AppPath))
                    Directory.CreateDirectory(AppPath);

                byte[] source = e.Result;

                if (source != null)
                {
                    try
                    {
                        FileStream fs = new FileStream(updatesPath, FileMode.Create, FileAccess.Write);
                        fs.Write(source, 0, source.Length);
                        fs.Close();
                    }
                    catch(Exception ex)
                    {
                        MessageDialog.ShowMessageDialog("Alert", "Error", ex.Message, (o, e) => { Application.Current.Shutdown(); });
                    }
                }

                if (File.Exists(settingsPath))
                {
                    if (File.Exists(updatesPath))
                    {
                        Settings settings = Settings.Load(settingsPath);
                        Settings updates = Settings.Load(updatesPath);

                        if (settings.LastUpdate != updates.LastUpdate)
                        {
                            Update(updates, settings);
                            settings.Save(settingsPath);
                        }
                    }
                }
                else
                {
                    if (File.Exists(updatesPath))
                        File.Copy(updatesPath, settingsPath);
                }

                File.Delete(updatesPath);
                UpdateFinished(this, EventArgs.Empty);
            }
        }
    }
}
