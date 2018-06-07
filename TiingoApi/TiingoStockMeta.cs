// <copyright file="TiingoStockMeta.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace Hilres.TiingoApi
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Stock meta data class.
    /// </summary>
    [DataContract]
    public class TiingoStockMeta : TiingoResponse
    {
        /// <summary>
        /// Gets (String) Long-form description of the asset
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; internal set; }

        /// <summary>
        /// Gets (date) The latest date we have price data available
        /// </summary>
        public DateTime? EndDate => Parse.NullDateTime(this.EndDateRaw);

        /// <summary>
        /// Gets (string) What exchange the price data belongs to. If null it means the data is a mutual fund or a composite of different exchanges.
        /// </summary>
        [DataMember(Name = "exchangeCode")]
        public string Exchange { get; internal set; }

        /// <summary>
        /// Gets (String) Full-length name of the fund
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; internal set; }

        /// <summary>
        /// Gets (date) The earliest date we have price data available
        /// </summary>
        public DateTime? StartDate => Parse.NullDateTime(this.StartDateRaw);

        /// <summary>
        /// Gets (String) Ticker related to the asset
        /// </summary>
        [DataMember(Name = "ticker")]
        public string Ticker { get; internal set; }

        /// <summary>
        /// Gets or sets (date) The latest date we have price data available
        /// </summary>
        [DataMember(Name = "endDate")]
        internal string EndDateRaw { get; set; }

        /// <summary>
        /// Gets or sets (date) The earliest date we have price data available
        /// </summary>
        [DataMember(Name = "startDate")]
        internal string StartDateRaw { get; set; }
    }
}