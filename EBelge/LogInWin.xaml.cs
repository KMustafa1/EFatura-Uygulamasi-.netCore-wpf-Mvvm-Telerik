﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace EBelge
{
    /// <summary>
    /// LogInWin.xaml etkileşim mantığı
    /// </summary>
    public partial class LogInWin : RadWindow
    {
        public LogInWin()
        {
            InitializeComponent();

            RadSplashScreenManager.Close();      
        }

    }
}
