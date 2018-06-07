// <copyright file="Parse.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace Hilres.TiingoApi.Tests
{
    using System;

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
            return DateTime.TryParse(text, out DateTime date) ? (DateTime?)date : null;
        }
    }
}