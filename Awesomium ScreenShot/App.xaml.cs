using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace Awesomium_ScreenShot
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if ( e.Args.Length>0)
            {
                /* do stuff without a GUI */
            }
            else
            {
                new MainWindow().ShowDialog();
            }
            this.Shutdown();
        }

        }
    }
