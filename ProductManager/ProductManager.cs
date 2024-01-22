using Synchronizer.Product;
using Synchronizer.PriceManager;


namespace Synchronizer.ProductManager
{
    public class ProductManager
    {
        private const double _discountRate = 0.96;
        private static Dictionary<string, double> GetDiscountedPrice(Dictionary<string, double> KubPrices, Dictionary<string, double> m2Prices)
        {
            Dictionary<string, double> discountedPrice = new Dictionary<string, double>();

            foreach (var city in KubPrices.Keys)
            {
                double kubPrice = KubPrices[city];
                double m2Price = m2Prices[city];

                double price = (kubPrice == 0 || m2Price == 0) ? Math.Max(kubPrice, m2Price) : Math.Min(kubPrice, m2Price);
                price = Math.Round(price * _discountRate, 1);
                discountedPrice[city] = price;

                Console.WriteLine($"KUB price: {kubPrice}, M2 price: {m2Price}, discounted price: {price}");
            }

            return discountedPrice;
        }

        private static ExportProduct ConvertProductForExport(ImportProduct product, Dictionary<string, double> discountedPrice)
        {
            ExportProduct convertedProduct = new ExportProduct()
            {
                SKU = product.SKU,
                KyivPrice = discountedPrice.TryGetValue("Київ", out double kyivPrice) ? kyivPrice : 0,
                KharkivPrice = discountedPrice.TryGetValue("Харків", out double kharkivPrice) ? kharkivPrice : 0,
                DniproPrice = discountedPrice.TryGetValue("Дніпро", out double dniproPrice) ? dniproPrice : 0,
                OdesaPrice = discountedPrice.TryGetValue("Одеса", out double odesaPrice) ? odesaPrice : 0,
                PoltavaPrice = discountedPrice.TryGetValue("Полтава", out double poltavaPrice) ? poltavaPrice : 0,
                LvivPrice = discountedPrice.TryGetValue("Львів", out double lvivPrice) ? lvivPrice : 0
            };

            return convertedProduct;
        }

        public static void UpdateProductsPrice(List<ImportProduct> importedProducts)
        {
            KubPriceManager.GetUpdatedPrices(importedProducts);
            Console.WriteLine("KUB prices successfully received.");
            M2PriceManager.GetUpdatedPrices(importedProducts);
            Console.WriteLine("M2 prices successfully received.");
        }

        public static List<ExportProduct> GetComparedPrice(List<ImportProduct> importedProducts)
        {
            List<ExportProduct> exportProducts = new List<ExportProduct>();

            foreach (var product in importedProducts)
            {
                Console.WriteLine($"Product SKU: {product.SKU}");
                var discountedPrice = GetDiscountedPrice(product.KubPrice, product.M2Price);
                var exportProduct = ConvertProductForExport(product, discountedPrice);
                exportProducts.Add(exportProduct);
            }

            Console.WriteLine("Prices successfully compared and ready for export.");

            return exportProducts;
        }
    }
}
