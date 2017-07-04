using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace AverageDollarPriceObtainer
{
    public class RestClient : IRestClient
    {
        private const string UsdCurrencyCode = "USD";
        private const string MediaType = "application/json";
        private const string NationalBankOfUkraineWebsiteApi = 
            "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?valcode={0}&date={1}&json";

        public double GetAverageUsdForexRate(string year, string month)
        {
            var usdForexRatesPerEachDayInMonth = new List<double>();
            var httpClient = new HttpClient();
            var daysInCertainMonth = DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month));

            httpClient.BaseAddress = new Uri(NationalBankOfUkraineWebsiteApi);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));

            for (var i = 1; i <= daysInCertainMonth; i++)
            {
                var requestUrl = CreateRequestUrl(year, month, ConvertIntDayToString(i));

                HttpResponseMessage response = httpClient.GetAsync(requestUrl).Result;

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(string.Format("The data was not obtained! Status Code: {0}, Reason: {1}",
                        (int)response.StatusCode, response.ReasonPhrase));
                }

                var currencyValue = ParseResponseMessageForForexRate(response);

                usdForexRatesPerEachDayInMonth.Add(currencyValue);
            }

            return usdForexRatesPerEachDayInMonth.Average();
        }

        private double ParseResponseMessageForForexRate(HttpResponseMessage responseMessage)
        {
            var content = responseMessage.Content.ReadAsStringAsync().Result;
            var parsedJson = JArray.Parse(content);
            var currencyValue = 0.0;

            if (parsedJson.Count != 0)
            {
                currencyValue = parsedJson.Select(v => v["rate"]).Single().Value<double>();
            }

            return currencyValue;
        }

        private string CreateRequestUrl(string year, string month, string day)
        {
            var date = string.Join("", year, month, day);
            return string.Format(NationalBankOfUkraineWebsiteApi, UsdCurrencyCode, date);
        }

        private string ConvertIntDayToString(int day)
        {
            return day >= 10 ? 
                day.ToString() : 
                string.Join("", "0", day.ToString());
        }

    }
}
