﻿namespace Steam.TradeOffer.Models
{
    using Newtonsoft.Json;

    public class Description
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}