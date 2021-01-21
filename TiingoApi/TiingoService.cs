// <copyright file="TiingoService.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace Hilres.TiingoApi
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Tiingo service class.
    /// </summary>
    public class TiingoService
    {
        private const string ApiUrl = "https://api.tiingo.com/";
        private readonly TiingoSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="TiingoService"/> class.
        /// </summary>
        /// <param name="settings">Authorization token to access service.</param>
        public TiingoService(TiingoSettings settings)
        {
            this.settings = settings ?? throw new NullReferenceException("settings must be set.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TiingoService"/> class.
        /// </summary>
        /// <param name="configuration">IConfiguration.</param>
        public TiingoService(IConfiguration configuration)
        {
            this.settings = new TiingoSettings(configuration);
        }

        /// <summary>
        /// Get stock meta.
        /// </summary>
        /// <param name="ticker">The ticker associated with the stock, Mutual Fund or ETF.</param>
        /// <returns>TiingoList of StockMetaData.</returns>
        public async Task<TiingoStockMeta> GetStockMetaAsync(string ticker)
        {
            if (string.IsNullOrWhiteSpace(ticker))
            {
                throw new ArgumentException("Null or empty ticker");
            }

            string path = $"tiingo/daily/{ticker}";
            return await this.GetJsonAsync<TiingoStockMeta, TiingoStockMeta>(path);
        }

        /// <summary>
        /// Get stock prices.
        /// </summary>
        /// <param name="ticker">The ticker associated with the stock, Mutual Fund or ETF.</param>
        /// <param name="startDate">(optional) If startDate or endDate is not null, historical data will be queried. This filter limits metrics to on or later than the startDate.</param>
        /// <param name="endDate">(optional) If startDate or endDate is not null, historical data will be queried. This filter limits metrics to on or less than the endDate.</param>
        /// <param name="frequency">(optional) Default: daily. Allows re-sampled values that allow you to choose the values returned as daily, weekly, monthly, or annually values. Note: ONLY DAILY takes into account holidays. All others use standard business days.</param>
        /// <returns>TiingoList of StockPriceData.</returns>
        public async Task<TiingoList<TiingoStockPrice>> GetStockPricesAsync(
                    string ticker,
                    DateTime? startDate = null,
                    DateTime? endDate = null,
                    ResampleFrequency frequency = ResampleFrequency.Daily)
        {
            StringBuilder paramter = new StringBuilder();
            paramter.AppendUrlParamter("startDate", startDate);
            paramter.AppendUrlParamter("endDate", endDate);
            paramter.AppendUrlParamter("resampleFreq", frequency.ToString());
            paramter.AppendUrlParamter("format", "json");

            string path = $"tiingo/daily/{ticker}/prices{paramter}";
            return await this.GetJsonAsync<TiingoList<TiingoStockPrice>, TiingoStockPrice>(path);
        }

        /// <summary>
        /// Get a list of supported tickers.
        /// </summary>
        /// <returns>A list of stock tickers.</returns>
        public async Task<TiingoList<TiingoStockTicker>> GetStockTickersAsync()
        {
            const string url = "https://apimedia.tiingo.com/docs/tiingo/daily/supported_tickers.zip";
            return await this.GetResponseAsync<TiingoList<TiingoStockTicker>>(url, responseStream =>
            {
                List<TiingoStockTicker> tickers = new List<TiingoStockTicker>();
                using (ZipArchive zip = new ZipArchive(responseStream))
                {
                    foreach (ZipArchiveEntry item in zip.Entries)
                    {
                        if (item.Name.EndsWith(".csv"))
                        {
                            using Stream stream = item.Open();
                            ExtractTickersFromStream(stream, tickers);
                        }
                    }
                }

                return new TiingoList<TiingoStockTicker>
                {
                    Items = tickers,
                    ApiSuccessful = true,
                    ApiErrorMessage = null,
                };
            });
        }

        /// <summary>
        /// Extract Tickers From Stream.
        /// </summary>
        /// <param name="stream">Source stream.</param>
        /// <param name="tickers">Ticker list to add to.</param>
        private static void ExtractTickersFromStream(Stream stream, IList<TiingoStockTicker> tickers)
        {
            using TextReader reader = new StreamReader(stream);

            // Skip header.
            string[] header = CSV.Import(reader);

            string[] item;
            while ((item = CSV.Import(reader)) != null)
            {
                tickers.Add(new TiingoStockTicker
                {
                    Ticker = item[0],
                    Exchange = item[1],
                    AssetType = item[2],
                    PriceCurrency = item[3],
                    StartDate = Parse.NullDateTime(item[4]),
                    EndDate = Parse.NullDateTime(item[5]),
                });
            }
        }

        /// <summary>
        /// Get the response text from the web exception.
        /// </summary>
        /// <param name="e">WebException.</param>
        /// <returns>Response text.</returns>
        private static string GetResponce(WebException e)
        {
            string text = string.Empty;
            if (e.Response != null
                && e.Response.GetResponseStream() != null)
            {
                using MemoryStream stream = new MemoryStream();
                using StreamReader textStream = new StreamReader(stream, System.Text.Encoding.ASCII);
                e.Response.GetResponseStream().CopyTo(stream);
                stream.Position = 0;
                text = textStream.ReadToEnd();
            }

            return text;
        }

        /// <summary>
        /// Get JSON data from API.
        /// </summary>
        /// <typeparam name="TResult">Type of result wanted.</typeparam>
        /// <typeparam name="TItem">Type of item getting.</typeparam>
        /// <param name="requestPath">Requested path to use.</param>
        /// <returns>The results.</returns>
        private async Task<TResult> GetJsonAsync<TResult, TItem>(string requestPath)
            where TResult : TiingoResponse, new()
            where TItem : class
        {
            return await this.GetResponseAsync<TResult>(requestPath, responseStream =>
            {
                using MemoryStream stream = new MemoryStream();
                using StreamReader textStream = new StreamReader(stream, System.Text.Encoding.ASCII);
                responseStream.CopyTo(stream);
                stream.Position = 0;
                string jsonText = textStream.ReadToEnd();
                stream.Position = 0;

                // Convert stream to an object.
                try
                {
                    if (typeof(TResult) == typeof(TiingoList<TItem>))
                    {
                        // Get an array.
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(TItem[]));
                        TItem[] serializedObj = (TItem[])serializer.ReadObject(stream);

                        TiingoList<TItem> result = new TiingoList<TItem>
                        {
                            ApiErrorMessage = null,
                            ApiRawJson = jsonText,
                            ApiSuccessful = true,
                            Items = serializedObj,
                        };

                        return result as TResult;
                    }
                    else
                    {
                        // Get an Item.
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(TResult));
                        TResult serializedObj = (TResult)serializer.ReadObject(stream);

                        serializedObj.ApiErrorMessage = null;
                        serializedObj.ApiRawJson = jsonText;
                        serializedObj.ApiSuccessful = true;
                        return serializedObj;
                    }
                }
                catch (Exception e)
                {
                    return new TResult
                    {
                        ApiErrorMessage = e.Message,
                        ApiRawJson = jsonText,
                        ApiSuccessful = false,
                    };
                }
            });
        }

        /// <summary>
        /// Get the results from the Tiingo web service.
        /// </summary>
        /// <typeparam name="TResult">Type of result wanted.</typeparam>
        /// <param name="requestPath">The request path requesting.</param>
        /// <param name="func">The function to call after getting the data from the web service.</param>
        /// <returns>The results.</returns>
        private async Task<TResult> GetResponseAsync<TResult>(string requestPath, Func<Stream, TResult> func)
            where TResult : TiingoResponse, new()
        {
            HttpWebRequest request;
            if (typeof(TResult) != typeof(TiingoList<TiingoStockTicker>))
            {
                string url = string.Format("{0}{1}", ApiUrl, requestPath);
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "application/json";
                request.SetTokenAuthentication(this.settings.AuthorizationToken);
                request.UserAgent = "TiingoApi in development";
            }
            else
            {
                // For the stock ticker request that does not need a token.
                request = (HttpWebRequest)WebRequest.Create(requestPath);
            }

            try
            {
                using HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync());
                using Stream responseStream = response.GetResponseStream();
                return func(responseStream);
            }
            catch (WebException e)
            {
                return new TResult
                {
                    ApiErrorMessage = e.Message,
                    ApiRawJson = GetResponce(e),
                    ApiSuccessful = false,
                };
            }
        }
    }
}