using Synchronizer.FileManager;
using Synchronizer.Product;
using Synchronizer.ProductManager;

public class Program
{
    public static void Main(string[] args)
    {
        string importPath = @"C:\\Users\\User\\Desktop\\test.xlsx";
        string exportPath = @"C:\\Users\\User\\Desktop\\BC_Updated_Price.xlsx";

        List<ImportProduct> importProducts = FileManager.ImportProducts(importPath);
        ProductManager.UpdateProductsPrice(importProducts);

        List<ExportProduct> exportProducts = ProductManager.GetComparedPrice(importProducts);
        FileManager.ExportProducts(exportProducts, exportPath);
    }
}
