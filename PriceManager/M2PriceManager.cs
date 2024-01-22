using HtmlAgilityPack;
using Synchronizer.Product;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Synchronizer.PriceManager
{
    public class M2PriceManager : PriceManager
    {
        private static void FindKyivPrice(HtmlDocument htmlDocument, Dictionary<string, double> prices)
        {
            var priceTag = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='rm-product-center-price']/span");

            if (priceTag != null)
            {
                Match match = regex.Match(priceTag.InnerText);
                if (double.TryParse(match.Groups[1].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double price))
                {
                    prices["Київ"] = price;
                }
            }
        }

        private static double FindCityPrice(HtmlNode priceTableRow)
        {
            var priceWithCurrency = priceTableRow.SelectSingleNode(".//div[@class='tr']");

            if (priceWithCurrency != null)
            {
                Match match = regex.Match(priceWithCurrency.InnerText.Trim());
                bool priceParse = double.TryParse(match.Groups[1].Value, CultureInfo.InvariantCulture, out double price);
                return price;
            }

            return 0;
        }

        private static void FindAnotherCitiesPrice(HtmlDocument htmlDocument, Dictionary<string, double> prices)
        {
            var priceTable = htmlDocument.DocumentNode.SelectNodes("//div[@class='product-available-wrap']/div[@class='available-list city-select']/div[@class='item']");

            if (priceTable != null)
            {
                for (int i = 0; i < priceTable.Count; i++)
                {
                    string city = priceTable[i].SelectSingleNode(".//div/a[@class='city-item']").InnerText.Trim();
                    double price = FindCityPrice(priceTable[i]);

                    prices[city] = price;
                }
            }
        }

        private static void GetProductPrice(ImportProduct product)
        {
            if (product.M2_Url != null)
            {
                var htmlDocument = GetPageContent(product.M2_Url);
                FindKyivPrice(htmlDocument, product.M2Price);
                FindAnotherCitiesPrice(htmlDocument, product.M2Price);
            }
        }

        public static void GetUpdatedPrices(List<ImportProduct> products)
        {
            int listLength = products.Count;
            foreach (var product in products)
            {
                GetProductPrice(product);
                listLength--;
                Console.WriteLine($"M2 prices for product {product.SKU} were received. Products left: {listLength}");
            }
        }
    }
}
