using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Telerik.Windows.Controls;
using System.Windows;
using AuthenticationWS;
using Microsoft.Win32;

namespace EBelge.ViewModels
{
    public class LogInViewModel : ViewModelBase
    {
		private bool isBusy;
		
		private string password = "izi321";
		private string userName = "izibiz-test2";
		private LoginResponse loginRes;

		public LogInViewModel()
		{
		}

		public bool IsBusy
		{
			get { return this.isBusy; }
			set
			{
				if (this.isBusy != value)
				{
					this.isBusy = value;
					this.OnPropertyChanged(() => this.IsBusy);

					if (this.isBusy)
					{
						var backgroundWorker = new BackgroundWorker();
						backgroundWorker.DoWork += this.OnBackgroundWorkerDoWork;
						backgroundWorker.RunWorkerCompleted += OnBackgroundWorkerRunWorkerCompleted;
						backgroundWorker.RunWorkerAsync();
					}
				}
			}
		}

		public string Password
		{
			get { return this.password; }
			set
			{
				if (this.password != value)
				{
					this.password = value;
					this.OnPropertyChanged(() => this.Password);
				}
			}
		}

		public string UserName
		{
			get { return this.userName; }
			set
			{
				if (this.userName != value)
				{
					this.userName = value;
					this.OnPropertyChanged(() => this.UserName);
				}
			}
		}

		private void OnBackgroundWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			var backgroundWorker = sender as BackgroundWorker;
			backgroundWorker.DoWork -= this.OnBackgroundWorkerDoWork;
			backgroundWorker.RunWorkerCompleted -= OnBackgroundWorkerRunWorkerCompleted;

			InvokeOnUIThread(() =>
			{
				this.IsBusy = false;
				if (loginRes.ERROR_TYPE == null)
				{
					RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\EBElge");
					key.SetValue("SessionId", loginRes.SESSION_ID);
					key.Close();
					
					RadSplashScreenManager.Show();
					RadWindowManager.Current.CloseAllWindows();
					new MainWindow().Show();
				}
				else
				{
					RadWindow.Alert("Kullanıcı Adı veya Parola yanlış. Daha sonra tekrar deneyin.");
				}
				
			});
		}

		private void OnBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
		{
			AuthenticationServicePortClient authenticationServicePortClient = new AuthenticationServicePortClient();

			LoginRequest req = new LoginRequest
			{
				REQUEST_HEADER = new REQUEST_HEADERType
				{
					SESSION_ID = "-1",
					APPLICATION_NAME = "izibiz.Application"
				},
				USER_NAME = UserName,
				PASSWORD = Password
			};
			
			loginRes = authenticationServicePortClient.Login(req);
		}
	}
}
