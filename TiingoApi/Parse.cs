// <copyright file="Parse.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace Hilres.TiingoApi
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Data parser helper class.
    /// </summary>
    internal static class Parse
    {
        /// <summary>
        /// Parse a string into a null-able date time.
        /// </summary>
        /// <param name="text">String to parse into a date time field.</param>
        /// <returns>Null-able date time value.  Null if unable to parse.</returns>
        internal static DateTime? NullDateTime(string text)
        {
            return DateTimeOffset.TryParse(text, null, DateTimeStyles.AssumeLocal, out DateTimeOffset date)
                ? date.Date
                : (DateTime?)null;
        }

        /// <summary>
        /// Parse a string into a floating point number.
        /// </summary>
        /// <param name="text">>String to parse into a float field.</param>
        /// <returns>Null-able float value.  Null if unable to parse.</returns>
        internal static float? NullFloat(string text)
        {
            return float.TryParse(text, out float value) ? value : (float?)null;
        }

        /// <summary>
        /// Parse a string into a integer.
        /// </summary>
        /// <param name="text">>String to parse into a integer field.</param>
        /// <returns>Null-able integer value.  Null if unable to parse.</returns>
        internal static int? NullInt(string text)
        {
            return int.TryParse(text, out int value) ? value : (int?)null;
        }
    }
}