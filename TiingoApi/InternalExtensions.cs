// <copyright file="InternalExtensions.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace Hilres.TiingoApi
{
    using System;
    using System.Net;
    using System.Text;
    using System.Web;

    /// <summary>
    /// Internal extensions class.
    /// </summary>
    internal static class InternalExtensions
    {
        /// <summary>
        /// Append a date parameter if there is a date.
        /// </summary>
        /// <param name="builder">StringBuilder.</param>
        /// <param name="name">Name of parameter.</param>
        /// <param name="date">Date to append if not null.</param>
        internal static void AppendUrlParamter(this StringBuilder builder, string name, DateTime? date)
        {
            if (date != null)
            {
                builder.Append(builder.Length > 0 ? '&' : '?');
                builder.AppendFormat("{0}={1:yyyy-MM-dd}", name, date);
            }
        }

        /// <summary>
        /// Append a text parameter if there is a date.
        /// </summary>
        /// <param name="builder">StringBuilder.</param>
        /// <param name="name">Name of parameter.</param>
        /// <param name="text">Text to append if not null.</param>
        internal static void AppendUrlParamter(this StringBuilder builder, string name, string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                builder.Append(builder.Length > 0 ? '&' : '?');
                builder.AppendFormat("{0}={1}", name, HttpUtility.UrlEncode(text));
            }
        }

        /// <summary>
        /// Add token authentication to the header HttpWebRequest.
        /// </summary>
        /// <param name="request">HttpWebRequest.</param>
        /// <param name="authorizationToken">The authorization token.</param>
        internal static void SetTokenAuthentication(this HttpWebRequest request, string authorizationToken)
        {
            if (!string.IsNullOrWhiteSpace(authorizationToken))
            {
                request.Headers["Authorization"] = "Token " + authorizationToken;
            }
        }
    }
}