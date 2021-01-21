// <copyright file="TiingoSettings.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace Hilres.TiingoApi
{
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Tiingo settings class.
    /// </summary>
    public class TiingoSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TiingoSettings"/> class.
        /// In the appsettings.json file, it should look like this:
        /// {
        ///   "TiingoSettings":
        ///   {
        ///     "AuthorizationToken": "Token goes here"
        ///   }
        /// }.
        /// </summary>
        /// <param name="configuration">TiingoSettings will be bind to IConfiguration.</param>
        public TiingoSettings(IConfiguration configuration)
        {
            configuration.GetSection("TiingoSettings").Bind(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TiingoSettings"/> class.
        /// </summary>
        public TiingoSettings()
        {
        }

        /// <summary>
        /// Gets or sets authorization token.
        /// </summary>
        public string AuthorizationToken { get; set; }
    }
}