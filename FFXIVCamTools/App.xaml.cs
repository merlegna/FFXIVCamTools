using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using FFXIVCamTools.Model;
using FFXIVCamTools.View;

namespace FFXIVCamTools
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow _mainWindow;
        private ProgressDialog _progressDialog;

        public static readonly string AppPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FFXIVCamTools");

        public static readonly string RemoteUri =
            "https://raw.githubusercontent.com/merlegna/FFXIVCamTools/main/Settings.xml";

        public App()
        {

        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                _progressDialog = new ProgressDialog();
                _progressDialog.Show();
            });

            Updates updates = new Updates();
            updates.AppPath = AppPath;
            updates.Url = RemoteUri;
            updates.UpdateFinished += Updates_OnFinished;

            updates.Run();
        }

        private void Updates_OnFinished(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (_progressDialog != null)
                    _progressDialog.Hide();

                _mainWindow = new MainWindow();
                _mainWindow.Initialize();
            });
        }
    }
}
