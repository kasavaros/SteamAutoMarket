﻿namespace SteamAutoMarket.UI.Utils.Logger
{
    using System;

    using log4net.Appender;
    using log4net.Core;

    using SteamAutoMarket.UI.Pages;

    public class UiLogAppender : AppenderSkeleton
    {
        private const string DateFormat = "HH:mm:ss";

        public static string GetCurrentDate() => DateTime.Now.ToString(DateFormat);

        protected override void Append(LoggingEvent e)
        {
            if (e.Level == Level.Debug)
            {
                return;
            }

            LogsWindow.GlobalLogs += $"{GetCurrentDate()} ({e.Level}) - {e.RenderedMessage}{Environment.NewLine}";
        }
    }
}