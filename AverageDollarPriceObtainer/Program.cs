using System;
using System.Diagnostics;
using OfficeOpenXml;

namespace AverageDollarPriceObtainer
{
    class Program
    {
        static void Main(string[] args)
        {
            IRestClient restClient = new RestClient();

            //Console.Write("Please, enter the year and the month (YYYYMM):");

            //var month = Console.ReadLine();

            try
            {
                var sw = new Stopwatch();
                sw.Start();

                var val = restClient.GetAverageUsdForexRate("2017", "02");
                sw.Stop();

                Console.WriteLine(val);
                Console.WriteLine("Spent time: {0} seconds", sw.ElapsedMilliseconds / 1000);

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
