using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FFXIVCamTools.ViewModel
{
    class MessageDialogViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _iconKind;
        private string _title;
        private string _message;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string IconKind
        {
            get
            {
                return _iconKind;
            }
            set
            {
                if(value != _iconKind)
                {
                    _iconKind = value;
                    NotifyPropertyChanged(nameof(IconKind));
                }
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if(value != _title)
                {
                    _title = value;
                    NotifyPropertyChanged(nameof(Title));
                }
            }
        }

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if(value != _message)
                {
                    _message = value;
                    NotifyPropertyChanged(nameof(Message));
                }
            }
        }
    }
}
