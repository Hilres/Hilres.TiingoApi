// <copyright file="enums.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace Hilres.TiingoApi
{
    /// <summary>
    /// Re-sample frequency type.
    /// </summary>
    public enum ResampleFrequency
    {
        /// <summary>
        /// daily: Values returned as daily periods, with a holiday calendar
        /// </summary>
        Daily,

        /// <summary>
        /// weekly: Values returned as weekly data, with days ending on Friday
        /// </summary>
        Weekly,

        /// <summary>
        /// monthly: Values returned as monthly data, with days ending on the last standard business day (Mon-Fri) of each month
        /// </summary>
        Monthly,

        /// <summary>
        /// annually: Values returned as annual data, with days ending on the last standard business day(Mon-Fri) of each year
        /// </summary>
        Annually
    }
}
