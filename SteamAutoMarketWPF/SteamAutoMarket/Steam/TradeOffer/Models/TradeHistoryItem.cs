﻿namespace Steam.TradeOffer.Models
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    using Steam.TradeOffer.Enums;

    public class TradeHistoryItem
    {
        [JsonProperty("assets_given")]
        public List<TradedAsset> AssetsGiven { get; set; }

        [JsonProperty("assets_received")]
        public List<TradedAsset> AssetsReceived { get; set; }

        [JsonProperty("currency_given")]
        public List<TradedCurrency> CurrencyGiven { get; set; }

        [JsonProperty("currency_received")]
        public List<TradedCurrency> CurrencyReceived { get; set; }

        [JsonProperty("status")]
        public TradeState Status { get; set; }

        [JsonProperty("steamid_other")]
        public string SteamIdOther { get; set; }

        [JsonProperty("time_escrow_end")]
        public string TimeEscrowEnd { get; set; }

        [JsonProperty("time_init")]
        public string TimeInit { get; set; }

        [JsonProperty("tradeid")]
        public string TradeId { get; set; }
    }
}