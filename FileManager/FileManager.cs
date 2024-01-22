using Ganss.Excel;
using Synchronizer.Product;

namespace Synchronizer.FileManager
{ 
    public static class FileManager
    {        
        public static List<ImportProduct> ImportProducts(string filePath)
        {
            var products = new ExcelMapper(filePath).Fetch<ImportProduct>().ToList();
            Console.WriteLine($"Products successfully imported.");

            return products;
        }

        public static void ExportProducts(List<ExportProduct> products, string filePath)
        {
            DateTime now = DateTime.Now;
            var excelMapper = new ExcelMapper();
            excelMapper.Save(filePath, products, now.ToString("dd.MM.yyyy"));

            Console.WriteLine($"File successfully saved: {filePath}.");
        }
    }
}
