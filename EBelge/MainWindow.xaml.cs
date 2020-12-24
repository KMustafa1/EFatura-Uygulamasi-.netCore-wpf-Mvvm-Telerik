using AuthenticationWS;
using EBelge.ViewModels;
using Microsoft.Win32;
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
using Telerik.Windows.Controls;

namespace EBelge
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RadWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
            
            RadSplashScreenManager.Close();
        }
        
        protected override void OnClosed(WindowClosedEventArgs e)
        {
            AuthenticationServicePortClient authenticationServicePortClient = new AuthenticationServicePortClient();
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\EBelge");

            LogoutRequest req = new LogoutRequest
            {
                REQUEST_HEADER = new REQUEST_HEADERType
                {
                    SESSION_ID = key.GetValue("SessionId").ToString()
                }
            };
            authenticationServicePortClient.Logout(req);
            
            
            key.Close();
            System.Environment.Exit(1);
            base.OnClosed(e);
        }
    }
}
