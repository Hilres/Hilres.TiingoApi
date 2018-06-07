// <copyright file="TiingoResponse.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace Hilres.TiingoApi
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Tiingo API response class.
    /// </summary>
    [DataContract]
    public abstract class TiingoResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TiingoResponse"/> class.
        /// </summary>
        internal TiingoResponse()
        {
            this.ApiSuccessful = false;
            this.ApiErrorMessage = "Unspecified error";
        }

        /// <summary>
        /// Gets API error message if any.
        /// </summary>
        public string ApiErrorMessage { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the API was successful.
        /// </summary>
        public bool ApiSuccessful { get; internal set; }

        /// <summary>
        /// Gets the API raw JSON response data.
        /// </summary>
        public string ApiRawJson { get; internal set; }
    }
}