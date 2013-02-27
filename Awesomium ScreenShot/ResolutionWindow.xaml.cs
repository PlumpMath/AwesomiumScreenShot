using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Awesomium_ScreenShot.Class;
using Telerik.Windows.Controls;

namespace Awesomium_ScreenShot
{
    /// <summary>
    /// Interaction logic for ResolutionWindow.xaml
    /// </summary>
    public partial class ResolutionWindow : INotifyPropertyChanged
    {       
        public ResolutionWindow()
        {
            InitializeComponent();            
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

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            var newRes = this.DataContext as OutputTemplate;
            if(newRes==null)return;
            if (newRes.Validate())
            {
                this.DialogResult = true;                
                this.Close();
            }

        }
    }
}
