// <copyright file="TiingoStockTicker.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace Hilres.TiingoApi
{
    using System;

    /// <summary>
    /// Stock Ticker class.
    /// </summary>
    public class TiingoStockTicker
    {
        /// <summary>
        /// Gets ticker.
        /// </summary>
        public string Ticker { get; internal set; }

        /// <summary>
        /// Gets Exchange.
        /// </summary>
        public string Exchange { get; internal set; }

        /// <summary>
        /// Gets asset type.
        /// </summary>
        public string AssetType { get; internal set; }

        /// <summary>
        /// Gets price currency.
        /// </summary>
        public string PriceCurrency { get; internal set; }

        /// <summary>
        /// Gets start date.
        /// </summary>
        public DateTime? StartDate { get; internal set; }

        /// <summary>
        /// Gets end date.
        /// </summary>
        public DateTime? EndDate { get; internal set; }
    }
}