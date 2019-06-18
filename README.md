# Gets stock data from the Tiingo web service

[![NuGet](https://img.shields.io/badge/NuGet-0.2.0-blue.svg)](https://www.myget.org/feed/hilres/package/nuget/Hilres.TiingoApi)

This will retrieve stock data from the Tiingo web service.  Historical price data and meta data.

Data comes from: https://www.tiingo.com

## Using Hilres.TiingoApi

### Setting up the service

To get the authorization token from the app setting file and create the Tiingo service:

```csharp
using System.IO;
using Hilres.TiingoApi;
using Microsoft.Extensions.Configuration;

IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddUserSecrets<Program>()
        .Build();

TiingoService service = new TiingoService(new TiingoSettings(configuration));
```

And the appsettings.json file should look like this:

```json
{
    "TiingoSettings":
    {
        "AuthorizationToken": "Token goes here"
    }
}
```

OR

```csharp
using Hilres.TiingoApi;

TiingoService service = new TiingoService(new TiingoSettings { AuthorizationToken = "token goes here" });
```

### Get some stock price data by frequency and date range.

- ticker = The ticker associated with the stock, Mutual Fund or ETF
- startDate = (optional) If startDate or endDate is not null, historical data will be queried. This filter limits metrics to on or later than the startDate.
- endDate = (optional) If startDate or endDate is not null, historical data will be queried. This filter limits metrics to on or less than the endDate.
- frequency = (optional) Default: daily. Allows re-sampled values that allow you to choose the values returned as daily, weekly, monthly, or annually values. Note: ONLY DAILY takes into account holidays. All others use standard business days

```csharp
TiingoList<TiingoStockPrice> result = await service.GetStockPricesAsync(
                                            ticker: "msft",
                                            startDate: new DateTime(2018, 3, 4),
                                            endDate: new DateTime(2018, 3, 9),
                                            frequency: ResampleFrequency.Daily);

```

### Getting the meta data

- ticker = The ticker associated with the stock, Mutual Fund or ETF

```csharp
TiingoStockMeta meta = service.GetStockMetaAsync("msft");
```

### List of all stocks

```csharp
TiingoList<TiingoStockTicker> result = await service.GetStockTickersAsync();
```

### Tiingo documentation

https://api.tiingo.com/documentation/end-of-day
