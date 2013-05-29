using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;

namespace Awesomium_ScreenShot.Class
{
    public class Preferences : INotifyPropertyChanged
    {
        private int _browserHeight = 1024;
        private int _browserWidth = 768;
        private int _animationDelay = 1000;
        private bool _canScreenShot;
        private string _animationClass;
        private string _animatedClass;
        private ObservableCollection<OutputTemplate> _outputTemplates = new ObservableCollection<OutputTemplate> { new OutputTemplate {Height = 200,Width = 200,TemplateName = "Test"}};
        private OutputTemplate _activeTemplate = new OutputTemplate();
        
        public OutputTemplate ActiveTemplate
        {
            get
            {
                return _activeTemplate;
            } 
            set
            {
                _activeTemplate = value;
                BrowserHeight = _activeTemplate.Height;
                BrowserWidth = _activeTemplate.Width;
                OnPropertyChanged("ActiveTemplate");
            }
        }
        public string AnimationClass
        {
            get
            {
                return _animationClass;
            }
            set { 
                _animationClass = value;
                OnPropertyChanged("AnimationClass");
            }
        }
        public string AnimatedClass
        {
            get
            {
                return _animatedClass;
            }
            set
            {
                _animatedClass = value;
                OnPropertyChanged("AnimatedClass");
            }
        }
        public int BrowserHeight
        {
            get { return _browserHeight; }
            set
            {
                _browserHeight = value;
                OnPropertyChanged("BrowserHeight");
            }
        }
        public int BrowserWidth
        {
            get { return _browserWidth; }
            set
            {
                _browserWidth = value;
                OnPropertyChanged("BrowserWidth");
            }
        }
        public int AnimationDelay
        {
            get { return _animationDelay; }
            set
            {
                _animationDelay = value;
                OnPropertyChanged("AnimationDelay");
            }
        }        
        public ObservableCollection<OutputTemplate> OutputTemplates
        {
            get
            {
                return _outputTemplates;
            } 
            set
            {
                _outputTemplates = value;
                OnPropertyChanged("OutputTemplate");
            }
        }

        private RadContextMenu _menuItemList;
        public RadContextMenu MenuItemsList
        {
            get 
            {
                if (_menuItemList == null)
                {
                    var container = new RadContextMenu();
                    
                    
                    foreach (var item in OutputTemplates.Select(outputTemplate => new RadMenuItem { IsCheckable = true, Header = string.Format("{0} - {1} x {2}", outputTemplate.TemplateName, outputTemplate.Height, outputTemplate.Width), Name = outputTemplate.TemplateName, Tag = 1,StaysOpenOnClick = true}))
                    {
                        item.Click += item_Click;
                        container.Items.Add(item);
                    }
                                        
                    return container;
                    
                }
                return _menuItemList;
            }   
            set
            {
                _menuItemList = value;
                OnPropertyChanged("MenuItemsList");
            }
        }

        public void AddNew_Click(object sender, RoutedEventArgs e)
        {
            var rw = new ResolutionWindow
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };
            rw.Closed += rw_Closed;
            rw.ShowDialog();
        }
        

        void rw_Closed(object sender, WindowClosedEventArgs e)
        {
            var temp = sender as ResolutionWindow;
            if (temp == null) return;
            if(temp.DialogResult==null || temp.DialogResult == false) return;            
            var a = temp.DataContext as OutputTemplate;
            OutputTemplates.Add(a);            
            MenuItemsList.Items.Add(new RadMenuItem {Name = a.TemplateName, Tag = 1, IsCheckable = true,StaysOpenOnClick = true});
            OnPropertyChanged("MenuItemsList");            
        }

        void item_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var currentItem = e.OriginalSource as RadMenuItem;
            if (currentItem == null) return;
            if (currentItem.IsCheckable && currentItem.Tag != null)
            {
                var siblingItems = this.GetSiblingGroupItems(currentItem);
                if (siblingItems == null)
                {
                    return;
                }
                foreach (var item in siblingItems)
                {
                    if (item != currentItem)
                    {
                        item.IsChecked = false;
                    }
                }
            }

            var temp = sender as RadMenuItem;
            if (temp == null) return;
            foreach (var outputTemplate in OutputTemplates.Where(outputTemplate => outputTemplate.TemplateName == temp.Name))
            {
                ActiveTemplate = outputTemplate;
            }
        }        

        [Browsable(false)]
        public bool CanScreenShot
        {
            get
            {
                return _canScreenShot;
            }
            set
            {
                _canScreenShot = value;
                OnPropertyChanged("CanScreenShot");
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

        private List<RadMenuItem> GetSiblingGroupItems(RadMenuItem currentItem)
        {
            var parentItem = currentItem.ParentOfType<RadMenuItem>();
            if (parentItem == null)
            {
                return null;
            }
            List<RadMenuItem> items = new List<RadMenuItem>();
            foreach (var item in parentItem.Items)
            {
                RadMenuItem container = parentItem.ItemContainerGenerator.ContainerFromItem(item) as RadMenuItem;
                if (container == null || container.Tag == null)
                {
                    continue;
                }
                if (container.Tag.Equals(currentItem.Tag))
                {
                    items.Add(container);
                }
            }
            return items;
        }

        
    }
}
