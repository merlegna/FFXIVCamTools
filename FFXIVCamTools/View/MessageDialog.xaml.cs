using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using FFXIVCamTools.ViewModel;

namespace FFXIVCamTools.View
{
    /// <summary>
    /// MessageDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class MessageDialog : Window
    {
        private MessageDialogViewModel ViewModel;

        public MessageDialog()
        {
            InitializeComponent();

            ViewModel = new MessageDialogViewModel();
            DataContext = ViewModel;
        }

        public static void ShowMessageDialog(string iconKind, string title, string message, EventHandler closedEvent)
        {
            MessageDialog messageDialog = new MessageDialog();

            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                if (messageDialog.ViewModel != null)
                {
                    messageDialog.ViewModel.IconKind = iconKind;
                    messageDialog.ViewModel.Title = title;
                    messageDialog.ViewModel.Message = message;
                    messageDialog.Closed += closedEvent;

                    messageDialog.Show();
                }
            });
        }

        public void Initialize(string iconKind, string title, string message)
        {
            if (ViewModel != null)
            {
                ViewModel.IconKind = iconKind;
                ViewModel.Title = title;
                ViewModel.Message = message;
            }

            Show();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
