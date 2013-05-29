using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Awesomium.Core;
using Awesomium.Windows.Controls;
using Awesomium_ScreenShot.Class;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Telerik.Windows.Controls;

namespace Awesomium_ScreenShot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private WebControl _browser;     
       
        private string _savePath;
        private bool _isBusy;
		private Preferences _settings = new Preferences();				
        private ObservableCollection<string> _fileNames = new ObservableCollection<string>();        
		
        public event PropertyChangedEventHandler PropertyChanged;
		
		public Preferences Settings
        {
            get
            {
                return _settings;
            } 
            set
            {
                _settings = value;
                OnPropertyChanged("Settings");
            }
        }

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            } 
            set
            {
                _isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }
        public string SavePath
        {
            get
            {
                return _savePath;
            } 
            set { 
                _savePath = value; 
                OnPropertyChanged("SavePath"); 
            }
        }

       
        public ObservableCollection<string> FileNames
        {
            get
            {
                return _fileNames;
            }
            set
            {                
                _fileNames = value;     
            }
        }
       
        

        public MainWindow()
        {
            StyleManager.ApplicationTheme = new Windows7Theme();
            InitializeComponent();           
        }

        private void MenuPreferences_OnClick(object sender, RoutedEventArgs e)
        {
            var pw = new PreferencesWindow
                {
                    Settings = Settings,
                    Owner = this,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };
            pw.ShowDialog();

        }

        /*private void MenuOpen_OnClick(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
                {
                    Multiselect = true,
                    Filter = "Web Pages (*.htm, *.html)|*.htm;*.html|All files (*.*)|*.*"
                };
            var file = (bool)dlg.ShowDialog();
            if (!file) return;
            foreach (var fileName in dlg.FileNames.Where(fileName => FileNames.IndexOf(fileName)==-1))
            {
                FileNames.Add(fileName);
            }
            Settings.CanScreenShot = true;
        }*/
        

        private void MenuClose_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }       

        private void ScreenShot()
        {            
            _browser.LoadURL(FileNames[0]);
            _browser.LoadCompleted += FileLoaded;
        }

        private void FileLoaded(object sender, EventArgs e)
        {
            var browser = sender as WebView;
            if (browser == null) return;
            var saveFile = SavePath +"\\" +browser.Source.Segments[browser.Source.Segments.Length-1].Split('.')[0]+".png";
            //make file name parent folder name plus file name.
            TimedAction.ExecuteWithDelay(delegate
                {                    
                    browser.SaveToPNG(saveFile);
                    //remove file
                    var remaining = FileNames.Count;
                    switch (remaining)
                    {
                        case 0:
                            break;
                        case 1:                            
                            FileNames.Clear();
                            break;
                        default:
                            FileNames.RemoveAt(0);
                            ScreenShot();
                            break;
                    }                       
                },TimeSpan.FromMilliseconds(Settings.AnimationDelay) );                     
        }

        protected void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        void cmdDeleteUser_Clicked(object sender, RoutedEventArgs e)
        {
            var cmd = sender as System.Windows.Controls.Button;
            if (cmd == null) return;
            var s = cmd.DataContext as string;
            if (s == null) return;
            var deleteme = s;
            FileNames.Remove(deleteme);
        }

       

       /* private void MenuOpenFolder_OnClick(object sender, RoutedEventArgs e)
        {
            var fb = new FolderBrowserDialog {ShowNewFolderButton = false};
            var result = fb.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK) return;
            var files = Directory.GetFiles(fb.SelectedPath, "*.htm*");
            if (files.Length <= 0) return;
            foreach (var file in files.Where(file => FileNames.IndexOf(file) ==-1))
            {
                FileNames.Add(file);
            }
            Settings.CanScreenShot = true;
        }        */

        private void ScreenShotButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	var a = new FolderBrowserDialog();
            var result = a.ShowDialog();            
            if (result != System.Windows.Forms.DialogResult.OK) return;
            Settings.CanScreenShot = false;
            SavePath = a.SelectedPath;
            _browser = WebCore.CreateWebView(Settings.BrowserHeight, Settings.BrowserWidth);            
            ScreenShot();
        }       

        private void ResetButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	 FileNames.Clear();            
            Settings.CanScreenShot = false;
        }

        private void FileOpen_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Web Pages (*.htm, *.html)|*.htm;*.html|All files (*.*)|*.*"
            };
            var file = (bool)dlg.ShowDialog();
            if (!file) return;
            foreach (var fileName in dlg.FileNames.Where(fileName => FileNames.IndexOf(fileName) == -1))
            {
                FileNames.Add(fileName);
            }
            Settings.CanScreenShot = true;
        }

        private void FolderOpen_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var fb = new FolderBrowserDialog { ShowNewFolderButton = false };
            var result = fb.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK) return;
            var files = Directory.GetFiles(fb.SelectedPath, "*.htm*");
            if (files.Length <= 0) return;
            foreach (var file in files.Where(file => FileNames.IndexOf(file) == -1))
            {
                FileNames.Add(file);
            }
            Settings.CanScreenShot = true;
        }

        private void RadRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            Settings.AddNew_Click(sender,e);
        }

        private void PreferencesWindow_Click(object sender, RoutedEventArgs e)
        {
            var prefWindow = new PreferencesWindow();
            prefWindow.WindowStartupLocation=WindowStartupLocation.CenterOwner;            

            prefWindow.ShowDialog();            
        }
    }
}
