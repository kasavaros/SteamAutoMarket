﻿namespace SteamAutoMarket.UI.Models
{
    using System;
    using System.Linq;

    using SteamAutoMarket.UI.Repository.Settings;

    using SteamKit2;

    [Serializable]
    public class SteamAccountHyperlinkModel
    {
        public SteamAccountHyperlinkModel(SteamID steamId)
        {
            this.AccountName =
                SettingsProvider.GetInstance().SteamAccounts.FirstOrDefault(a => a.SteamId == steamId)?.Login
                ?? steamId.ConvertToUInt64().ToString();

            this.AccountLink = $"https://steamcommunity.com/profiles/{steamId}/";
        }

        public SteamAccountHyperlinkModel(int accountId)
        {
            var steamId = new SteamID((uint)accountId, EUniverse.Public, EAccountType.Individual).ConvertToUInt64();

            this.AccountName =
                SettingsProvider.GetInstance().SteamAccounts.FirstOrDefault(a => a.SteamId == steamId)?.Login
                ?? steamId.ToString();

            this.AccountLink = $"https://steamcommunity.com/profiles/{steamId}/";
        }

        public string AccountLink { get; }

        public string AccountName { get; }
    }
}