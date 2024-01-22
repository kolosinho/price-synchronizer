using HtmlAgilityPack;
using Synchronizer.Product;
using System.Globalization;
using System.Text.RegularExpressions;
namespace Synchronizer.PriceManager
{
    public class KubPriceManager : PriceManager
    {
        private static readonly List<string> kubCitiesUrl = new List<string>()
        {
            @"https://kub.kh.ua/",
            @"https://dnepr.kub.in.ua/",
            @"https://odessa.kub.in.ua/",
            @"https://lviv.kub.in.ua/",
            @"https://poltava.kub.in.ua/",
            @"https://kub.in.ua/"
        };

        private static double FindProductPrice(HtmlDocument htmlDocument)
        {
            var priceTag = htmlDocument.DocumentNode
                .SelectSingleNode("//div[@class='product-stock-price-info']//span[@id='price_base' or @class='price-new']");

            if (priceTag != null)
            {
                Match match = regex.Match(priceTag.InnerText);
                bool priceParse = double.TryParse(match.Groups[1].Value, CultureInfo.InvariantCulture, out double price);
                return price;
            }

            return 0;
        }

        private static void FindLvivPrice(ImportProduct product)
        {
            if (product.Kub_Lviv_Url != null)
            {
                int lvivIndex = 3;
                string url = kubCitiesUrl[lvivIndex] + product.Kub_Lviv_Url;
                var htmlDocument = GetPageContent(url);
                var price = FindProductPrice(htmlDocument);
                product.KubPrice["Львів"] = price;
            }
        }

        private static void GetProductPrice(ImportProduct product)
        {
            if (product.Kub_Url != null)
            {
                for (int i = 0; i < kubCitiesUrl.Count; i++)
                {
                    string url = kubCitiesUrl[i] + product.Kub_Url;
                    var htmlDocument = GetPageContent(url);

                    if (htmlDocument != null)
                    {
                        var price = FindProductPrice(htmlDocument);
                        var city = (City)i;

                        product.KubPrice[city.ToString()] = price;
                    }
                }
            }
            FindLvivPrice(product);
        }

        public static void GetUpdatedPrices(List<ImportProduct> products)
        {
            int listLength = products.Count;
            foreach (var product in products)
            {
                GetProductPrice(product);
                listLength--;
                Console.WriteLine($"KUB prices for product {product.SKU} were received. Products left: {listLength}");
            }
        }
    }
}
