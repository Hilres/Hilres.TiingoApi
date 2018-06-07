// <copyright file="TiingoStockPrice.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace Hilres.TiingoApi
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Stock price data class.
    /// </summary>
    [DataContract]
    public class TiingoStockPrice
    {
        /// <summary>
        /// Gets (float) the adjusted close price of the asset on the specific date. Returns null if not available.
        /// </summary>
        public float? AdjustedClose => Parse.NullFloat(this.AdjustedCloseRqw);

        /// <summary>
        /// Gets (float) the adjusted high price of the asset on the specific date. Returns null if not available.
        /// </summary>
        public float? AdjustedHigh => Parse.NullFloat(this.AdjustedHighRaw);

        /// <summary>
        /// Gets (float) the adjusted low price of the asset on the specific date. Returns null if not available.
        /// </summary>
        public float? AdjustedLow => Parse.NullFloat(this.AdjustedLowRaw);

        /// <summary>
        /// Gets (float) the adjusted opening price of the asset on the specific date. Returns null if not available.
        /// </summary>
        public float? AdjustedOpen => Parse.NullFloat(this.AdjustedOpenRaw);

        /// <summary>
        /// Gets (int) the adjusted number of shares traded during the day - adjusted for splits.Returns null if not available.
        /// </summary>
        public int? AdjustedVolume => Parse.NullInt(this.AdjustedOpenRaw);

        /// <summary>
        /// Gets (float) the actual (not adjusted) closing price of the asset on the specific date.
        /// </summary>
        public float? Close => Parse.NullFloat(this.AdjustedOpenRaw);

        /// <summary>
        /// Gets (date) the date this data pertains to.
        /// </summary>
        public DateTime? Date => Parse.NullDateTime(this.DateRaw);

        /// <summary>
        /// Gets (float) the dividend paid out on "date" (note that "date" will be the "exDate" for the dividend)
        /// </summary>
        public float? Dividend => Parse.NullFloat(this.DividendRaw);

        /// <summary>
        /// Gets (float) the actual (not adjusted) high price of the asset on the specific date.
        /// </summary>
        public float? High => Parse.NullFloat(this.HighRaw);

        /// <summary>
        /// Gets (float) the actual (not adjusted) low price of the asset on the specific date.
        /// </summary>
        public float? Low => Parse.NullFloat(this.LowRaw);

        /// <summary>
        /// Gets (float) the actual (not adjusted) open price of the asset on the specific date.
        /// </summary>
        public float? Open => Parse.NullFloat(this.OpenRaw);

        /// <summary>
        /// Gets (float) a factor used when a company splits or reverse splits. On days where there is ONLY a split (no dividend payment), you can calculate the adjusted close as follows: adjClose = "Previous Close"/splitFactor
        /// </summary>
        public float? SplitFactor => Parse.NullFloat(this.SplitFactorRaw);

        /// <summary>
        /// Gets (int) the actual (not adjusted) number of shares traded during the day.
        /// </summary>
        public int? Volume => Parse.NullInt(this.VolumeRaw);

        /// <summary>
        /// Gets or sets (float) the adjusted close price of the asset on the specific date. Returns null if not available.
        /// </summary>
        [DataMember(Name = "adjClose")]
        internal string AdjustedCloseRqw { get; set; }

        /// <summary>
        /// Gets or sets (float) the adjusted high price of the asset on the specific date. Returns null if not available.
        /// </summary>
        [DataMember(Name = "adjHigh")]
        internal string AdjustedHighRaw { get; set; }

        /// <summary>
        /// Gets or sets (float) the adjusted low price of the asset on the specific date. Returns null if not available.
        /// </summary>
        [DataMember(Name = "adjLow")]
        internal string AdjustedLowRaw { get; set; }

        /// <summary>
        /// Gets or sets (float) the adjusted opening price of the asset on the specific date. Returns null if not available.
        /// </summary>
        [DataMember(Name = "adjOpen")]
        internal string AdjustedOpenRaw { get; set; }

        /// <summary>
        /// Gets or sets (int) the adjusted number of shares traded during the day - adjusted for splits.Returns null if not available.
        /// </summary>
        [DataMember(Name = "adjVolume")]
        internal string AdjustedVolumeRaw { get; set; }

        /// <summary>
        /// Gets or sets (float) the actual (not adjusted) closing price of the asset on the specific date.
        /// </summary>
        [DataMember(Name = "close")]
        internal string CloseRaw { get; set; }

        /// <summary>
        /// Gets or sets (date) the date this data pertains to.
        /// </summary>
        [DataMember(Name = "date")]
        internal string DateRaw { get; set; }

        /// <summary>
        /// Gets or sets (float) the dividend paid out on "date" (note that "date" will be the "exDate" for the dividend)
        /// </summary>
        [DataMember(Name = "divCash")]
        internal string DividendRaw { get; set; }

        /// <summary>
        /// Gets or sets (float) the actual (not adjusted) high price of the asset on the specific date.
        /// </summary>
        [DataMember(Name = "high")]
        internal string HighRaw { get; set; }

        /// <summary>
        /// Gets or sets (float) the actual (not adjusted) low price of the asset on the specific date.
        /// </summary>
        [DataMember(Name = "low")]
        internal string LowRaw { get; set; }

        /// <summary>
        /// Gets or sets (float) the actual (not adjusted) open price of the asset on the specific date.
        /// </summary>
        [DataMember(Name = "open")]
        internal string OpenRaw { get; set; }

        /// <summary>
        /// Gets or sets (float) a factor used when a company splits or reverse splits. On days where there is ONLY a split (no dividend payment), you can calculate the adjusted close as follows: adjClose = "Previous Close"/splitFactor
        /// </summary>
        [DataMember(Name = "splitFactor")]
        internal string SplitFactorRaw { get; set; }

        /// <summary>
        /// Gets or sets (int) the actual (not adjusted) number of shares traded during the day.
        /// </summary>
        [DataMember(Name = "volume")]
        internal string VolumeRaw { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{this.Date}, O:{this.Open}, H:{this.High}, L:{this.Low}, C:{this.Close}, V:{this.Volume}";
        }
    }
}