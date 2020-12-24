using AuthenticationWS;
using EBelge.Models;
using EBelge.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Navigation;

namespace EBelge.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<NavigationItemModel> Items { get; set; }

        public void XsltEditor(object obj)
        {
            var win = new XsltEditorWin();
            RadWindowInteropHelper.SetAllowTransparency(win, false);
            win.Show();
        }

        public void LogOut(object obj)
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
        }

        public MainViewModel()
        {
            this.Items = new ObservableCollection<NavigationItemModel>();
            
            this.Items.Add(new NavigationItemModel() { Title = "Profil", IconGlyph = "&#xe801;", FrameworkElement = new ProfilView() });
            this.Items.Add(new NavigationItemModel() { Title = "Ürünler", IconGlyph = "&#xe614;", FrameworkElement = new ProductView() });
            this.Items.Add(new NavigationItemModel() { Title = "Mükellefler", IconGlyph = "&#xe81b;", FrameworkElement = new CheckUserView() });
            this.Items.Add(new NavigationItemModel() { Title = "E-Belgeler", IconGlyph = "&#xe64f;", FrameworkElement = new EDocView() });
            this.Items.Add(new NavigationItemModel() { Title = "Xslt Editor", IconGlyph = "&#xe648;", Command = new DelegateCommand(XsltEditor) });
            this.Items.Add(new NavigationItemModel() { Title = "Çıkış Yap", IconGlyph = "&#xe131;", Command = new DelegateCommand(LogOut) });
        }

        
    }
}
