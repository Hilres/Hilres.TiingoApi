// <copyright file="Extensions.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace Hilres.TiingoApi
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Extensions class.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Add database services.
        /// </summary>
        /// <param name="services">Service Collection.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddTiingoService(this IServiceCollection services)
        {
            services.AddSingleton<TiingoService>();
            return services;
        }
    }
}