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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FFXIVCamTools.ViewModel;

namespace FFXIVCamTools.View
{
    /// <summary>
    /// CameraControl.xaml の相互作用ロジック
    /// </summary>
    public partial class CameraControl : UserControl
    {
        private CameraControlViewModel ViewModel;
        public int Status;

        public CameraControl()
        {
            InitializeComponent();

            ViewModel = new CameraControlViewModel();
            DataContext = ViewModel;

            ViewModel.PropertyChanged += CameraControl_OnStatusChanged;
            ZoomSlider.Value = ViewModel.ZoomValue;
            FovSlider.Value = ViewModel.FovValue;
        }

        private void CameraControl_OnStatusChanged(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                transitioner.SelectedIndex = ViewModel.Status;
            });
        }

        private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ViewModel != null)
                ViewModel.ZoomValue = (float)ZoomSlider.Value;
        }

        private void FovSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ViewModel != null)
                ViewModel.FovValue = (float)FovSlider.Value;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Apply();
        }
    }
}
