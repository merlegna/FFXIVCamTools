using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows;
using System.Threading;
using System.IO;
using System.Net;
using FFXIVCamTools.Model;
using FFXIVCamTools.View;

namespace FFXIVCamTools.ViewModel
{
    class CameraControlViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Settings _settings;
        private Process _process;
        private Timer _timer;
        private MemoryAddress _memoryAddress;
        private string _settingsPath;
        private int _status;

        public float ZoomValue;
        public float FovValue;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public int Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (value != _status)
                {
                    _status = value;
                    NotifyPropertyChanged(nameof(Status));
                }
            }
        }

        public CameraControlViewModel()
        {
            _settingsPath = Path.Combine(App.AppPath, "Settings.xml");

            LoadSettings(_settingsPath);
            TimerInitialize();
        }

        private void LoadSettings(string location)
        {
            if (File.Exists(location))
            {
                _settings = Settings.Load(location);

                if (_settings != null)
                {
                    ZoomValue = _settings.DesiredZoom;
                    FovValue = _settings.DesiredFov;
                }
            }
        }

        private void TimerInitialize()
        {
            TimerCallback timerCallback = new TimerCallback(ProcessMonitor);
            _timer = new Timer(timerCallback, null, 0, 1000);
        }

        private void ProcessMonitor(object sender)
        {
            if (_process == null)
                _process = GetProcess("ffxiv_dx11");

            if (_process != null)
            {
                if (!_process.HasExited)
                {
                    Status = 1;
                }
                else
                {
                    _process = null;
                }
            }
            else
            {
                Status = 2;
            }
        }

        private Process GetProcess(string name)
        {
            Process[] processes;

            try
            {
                processes = Process.GetProcesses();

                foreach (var p in processes)
                {
                    if (p.ProcessName == name)
                        return p;
                }
            }
            catch
            {

            }

            return null;
        }

        public void Apply()
        {
            try
            {
                _memoryAddress = Memory.GetMemoryAddress(_process, _settings);

                if (_memoryAddress != null)
                {
                    Memory.Write(_process.Handle, _memoryAddress.ZoomMax, BitConverter.GetBytes(ZoomValue));
                    Memory.Write(_process.Handle, _memoryAddress.ZoomCurrent, BitConverter.GetBytes(ZoomValue));
                    Memory.Write(_process.Handle, _memoryAddress.FovMax, BitConverter.GetBytes(FovValue / 100));
                    Memory.Write(_process.Handle, _memoryAddress.FovCurrent, BitConverter.GetBytes(FovValue / 100));

                    _settings.DesiredZoom = ZoomValue;
                    _settings.DesiredFov = FovValue;
                    _settings.Save(_settingsPath);
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowMessageDialog("Alert", "Error", ex.Message, (o, e) => { Application.Current.Shutdown(); });
            }
        }
    }
}
