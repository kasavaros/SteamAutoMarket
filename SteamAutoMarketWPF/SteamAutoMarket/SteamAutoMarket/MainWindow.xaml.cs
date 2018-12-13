﻿using System.Reflection;
using log4net;

namespace SteamAutoMarket
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Windows.Forms;

    using FirstFloor.ModernUI.Presentation;
    using FirstFloor.ModernUI.Windows.Controls;

    using SteamAutoMarket.Core;
    using SteamAutoMarket.UI.Repository.Context;
    using SteamAutoMarket.UI.Repository.Settings;
    using SteamAutoMarket.UI.Utils.Logger;
    using SteamAutoMarket.UI.Utils.Resources;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            if (!File.Exists("license.txt"))
            {
                MessageBox.Show(
                    @"license.txt file not found!",
                    @"License check error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                System.Windows.Application.Current.Shutdown();
            }

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            ServicePointManager.ServerCertificateValidationCallback += OnServerCertificateValidationCallback;

            Logger.Setup(Logger.NewFileAppender(), new UiLogAppender());
            Logger.UpdateLoggerLevel(SettingsProvider.GetInstance().LoggerLevel);

            AppearanceManager.Current.ThemeSource = ModernUiThemeUtils.GetTheme(SettingsProvider.GetInstance().Theme);
            AppearanceManager.Current.AccentColor = ModernUiThemeUtils.GetColor(SettingsProvider.GetInstance().Color);

            UiGlobalVariables.MainWindow = this;
            this.DataContext = this;
            this.InitializeComponent();
            this.UpdateProgram();
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ErrorNotify.CriticalMessageBox("Oops. Seems application is crushed", (Exception)e.ExceptionObject);
            using (var wc = new WebClient())
            {
                wc.UploadString("www.steambiz.store/api/logerror", ((Exception)e.ExceptionObject).ToString());
            }
        }

        private static bool OnServerCertificateValidationCallback(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            if (certificate.Subject.Contains("pythonanywhere"))
            {
                return true;
            }

            return sslPolicyErrors.ToString() == "None";
        }

        private void UpdateProgram()
        {
            AutoUpdater.AutoUpdater.RunUpdateAsAdmin = true;
            AutoUpdater.AutoUpdater.DownloadPath = Environment.CurrentDirectory;
            AutoUpdater.AutoUpdater.AppCastURL = "https://www.steambiz.store/release/release.xml";
            AutoUpdater.AutoUpdater.Start();
        }
    }
}