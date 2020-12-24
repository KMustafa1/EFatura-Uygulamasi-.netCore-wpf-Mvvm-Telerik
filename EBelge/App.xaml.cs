using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.SplashScreen;

namespace EBelge
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            StyleManager.ApplicationTheme = new MaterialTheme();
            
            var dataContext = (SplashScreenDataContext)RadSplashScreenManager.SplashScreenDataContext;
            //dataContext.ImagePath = AppDomain.CurrentDomain.BaseDirectory+"\\"+"splashImage.png";
            dataContext.Footer = @"Telif hakkı © 2020 Mustafa Kuyucuoğlu. Her hakkı saklıdır.";
            dataContext.ImageWidth = 547;
            dataContext.ImageHeight = 400;

            MyContext context = new MyContext();
            if (context.Profils.Find(1) == null)
            {
                var profil = new Profil() { };
                context.Profils.Add(profil);
                context.SaveChanges();
            }

            //this.InitializeComponent();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            new LogInWin().Show();
            //new MainWindow().Show();
            base.OnStartup(e);
        }
    }
}
