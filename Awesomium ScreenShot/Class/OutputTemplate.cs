using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Telerik.Windows.Controls;

namespace Awesomium_ScreenShot.Class
{
    public class OutputTemplate : INotifyPropertyChanged
    {
        private string _templateName;
        private int _height;
        private int _width;
        private bool _isActive;
        public bool IsActive { get { return _isActive; } set { _isActive = value;OnPropertyChanged("IsActive"); } }
        private string _label;
        public string Label
        {
            get
            {
                _label = string.Format("{0} - {1} x {2}", _templateName, _height, _width);
                return _label;
            }
            set { 
                _label = value;
                OnPropertyChanged("Label");
            }
        }
        public string TemplateName { get { return _templateName; } set { _templateName = value;OnPropertyChanged("TemplateName"); } }
        public int Height
        {
            get
            {
                return _height;
            } 
            set
            {
                _height = value; 
                OnPropertyChanged("Height");
            }
        }
        public int Width
        {
            get
            {
                return _width;
            } 
            set
            {
                _width = value; 
                OnPropertyChanged("Width");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        
        public bool Validate()
        {
            return _height > 0 && _width > 0 && !string.IsNullOrEmpty(_templateName);
        }
    }
}
