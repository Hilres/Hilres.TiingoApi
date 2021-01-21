// <copyright file="TiingoServiceSettings.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace Hilres.TiingoApi.Tests
{
    using System.IO;
    using Hilres.TiingoApi;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Settings wrapper class.
    /// </summary>
    internal class TiingoServiceSettings : TiingoSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TiingoServiceSettings"/> class.
        /// dot net user-secrets set TiingoSettings:AuthorizationToken "Token goes here".
        /// </summary>
        internal TiingoServiceSettings()
            : base(new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddUserSecrets<TiingoServiceSettings>()
                    .Build())
        {
        }
    }
}
