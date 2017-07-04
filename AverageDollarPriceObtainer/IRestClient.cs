namespace AverageDollarPriceObtainer
{
    public interface IRestClient
    {
        /// <summary>
        /// Gets the USD forex rate according to UAH for a given date
        /// </summary>
        /// <param name="date">the date for USD forex rate in format YYYYMMDD</param>
        /// <returns>The rate of USD currency in accordance to UAH</returns>
        double GetAverageUsdForexRate(string year, string month);
    }
}
