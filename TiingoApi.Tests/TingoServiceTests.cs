// <copyright file="TingoServiceTests.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace Hilres.TiingoApi.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    /// <summary>
    /// Tiingo service tests class.
    /// </summary>
    public class TingoServiceTests
    {
        private readonly TiingoService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="TingoServiceTests"/> class.
        /// </summary>
        public TingoServiceTests()
        {
            this.service = new TiingoService(new TiingoServiceSettings());
        }

        /// <summary>
        /// Pass a null or blank to get stock meta should return an exception test.
        /// </summary>
        /// <param name="ticker">The ticker associated with the stock, Mutual Fund or ETF</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task GetStockMeta_InvaliedTicker_ArgumentException(string ticker)
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await this.service.GetStockMetaAsync(ticker));
        }

        /// <summary>
        /// Get one stock meta data.
        /// </summary>
        /// <param name="ticker">The ticker associated with the stock, Mutual Fund or ETF</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Theory]
        [InlineData("msft")]
        [InlineData("BA")]
        public async Task GetStockMeta_WithTicker(string ticker)
        {
            TiingoStockMeta result = await this.service.GetStockMetaAsync(ticker);

            Assert.NotNull(result);

            ////Assert.IsType<DateTime?>(result.EndDate);
            ////Assert.IsType<DateTime?>(result.StartDate);
            Assert.NotNull(result.ApiRawJson);
            Assert.NotNull(result.Description);
            Assert.NotNull(result.EndDate);
            Assert.NotNull(result.Exchange);
            Assert.NotNull(result.Name);
            Assert.NotNull(result.StartDate);
            Assert.NotNull(result.Ticker);
            Assert.Null(result.ApiErrorMessage);
            Assert.True(result.ApiSuccessful);
        }

        /// <summary>
        /// Get some stock price data by frequency and date range.
        /// </summary>
        /// <param name="ticker">The ticker associated with the stock, Mutual Fund or ETF</param>
        /// <param name="startDate">(optional) If startDate or endDate is not null, historical data will be queried. This filter limits metrics to on or later than the startDate.</param>
        /// <param name="endDate">(optional) If startDate or endDate is not null, historical data will be queried. This filter limits metrics to on or less than the endDate.</param>
        /// <param name="frequency">(optional) Default: daily. Allows re-sampled values that allow you to choose the values returned as daily, weekly, monthly, or annually values. Note: ONLY DAILY takes into account holidays. All others use standard business days</param>
        /// <param name="count">Expect count of items in the results</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Theory]
        [InlineData("msft", "2018-03-04", "2018-03-04", ResampleFrequency.Daily, 0)]
        [InlineData("msft", "2018-03-05", "2018-03-05", ResampleFrequency.Daily, 1)]
        [InlineData("msft", "2018-03-05", "2018-03-09", ResampleFrequency.Daily, 5)]
        [InlineData("msft", "2017-12-31", "2018-02-28", ResampleFrequency.Daily, 40)]
        [InlineData("msft", "2017-12-31", "2018-02-28", ResampleFrequency.Weekly, 10)]
        [InlineData("msft", "2017-12-31", "2018-02-28", ResampleFrequency.Monthly, 3)]
        [InlineData("msft", "2017-12-31", "2018-02-28", ResampleFrequency.Annually, 2)]
        public async Task GetStockPrices_DateRange(string ticker, string startDate, string endDate, ResampleFrequency frequency, int count)
        {
            TiingoList<TiingoStockPrice> result = await this.service.GetStockPricesAsync(
                                                        ticker: ticker,
                                                        startDate: Parse.NullDateTime(startDate),
                                                        endDate: Parse.NullDateTime(endDate),
                                                        frequency: frequency);

            Assert.NotNull(result);

            Assert.True(result.ApiSuccessful);
            Assert.Equal(count, result.Count);
        }

        /// <summary>
        /// Get some stock price data by frequency.
        /// </summary>
        /// <param name="ticker">The ticker associated with the stock, Mutual Fund or ETF</param>
        /// <param name="frequency">(optional) Default: daily. Allows re-sampled values that allow you to choose the values returned as daily, weekly, monthly, or annually values. Note: ONLY DAILY takes into account holidays. All others use standard business days</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Theory]
        [InlineData("msft", ResampleFrequency.Daily)]
        [InlineData("msft", ResampleFrequency.Weekly)]
        [InlineData("msft", ResampleFrequency.Monthly)]
        [InlineData("msft", ResampleFrequency.Annually)]
        public async Task GetStockPrices_TickerFrequency(string ticker, ResampleFrequency frequency)
        {
            TiingoList<TiingoStockPrice> result = await this.service.GetStockPricesAsync(ticker: ticker, frequency: frequency);

            Assert.NotNull(result);

            Assert.True(result.ApiSuccessful);
            Assert.Equal(1, result.Count);
        }

        /// <summary>
        /// Pass the ticker only.
        /// </summary>
        /// <param name="ticker">The ticker associated with the stock, Mutual Fund or ETF</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Theory]
        [InlineData("msft")]
        public async Task GetStockPrices_TickerOnly(string ticker)
        {
            TiingoList<TiingoStockPrice> result = await this.service.GetStockPricesAsync(ticker: ticker);

            Assert.NotNull(result);

            Assert.True(result.ApiSuccessful);
            Assert.Equal(1, result.Count);
        }

        /// <summary>
        /// Get the list of available stock tickers.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetStockTickers_Get()
        {
            TiingoList<TiingoStockTicker> result = await this.service.GetStockTickersAsync();

            Assert.NotNull(result);

            Assert.True(result.ApiSuccessful);
            Assert.Null(result.ApiErrorMessage);
            Assert.True(result.Count > 0);

            var exchanges = result
                            .GroupBy(r => r.Exchange)
                            .Select(g => g.Key.ToUpper());

            Assert.Contains("NYSE", exchanges);
            Assert.Contains("NASDAQ", exchanges);

            TiingoStockTicker ticker = result[0];

            ////Assert.IsType<DateTime?>(ticker.EndDate);
            ////Assert.IsType<DateTime?>(ticker.StartDate);
            Assert.IsType<string>(ticker.Exchange);
            Assert.IsType<string>(ticker.PriceCurrency);
            Assert.IsType<string>(ticker.Ticker);

            Assert.NotEmpty(ticker.Ticker);
        }
    }
}