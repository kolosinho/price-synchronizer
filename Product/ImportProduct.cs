using Ganss.Excel;

namespace Synchronizer.Product
{
    public class ImportProduct
    {
        [Column("SKU")]
        public int SKU { get; set; }
        [Column("Kub")]
        public string Kub_Url { get; set; }
        [Column("KubLviv")]
        public string Kub_Lviv_Url { get; set; }
        [Column("M2URL")]
        public string M2_Url { get; set; }
        [Ignore]
        public Dictionary<string, double> KubPrice { get; set; }
        [Ignore]
        public Dictionary<string, double> M2Price { get; set; }

        public ImportProduct(int SKU, string @kub_URL, string @kub_URL_lviv, string @m2_URL)
        {
            this.SKU = SKU;
            Kub_Url = kub_URL;
            Kub_Lviv_Url = kub_URL_lviv;
            M2_Url = m2_URL;

            KubPrice = new Dictionary<string, double>
            {
                {"Київ", 0},
                {"Харків", 0},
                {"Дніпро", 0},
                {"Одеса", 0},
                {"Львів", 0},
                {"Полтава", 0}
            };

            M2Price = new Dictionary<string, double>
            {
                {"Київ", 0},
                {"Харків", 0},
                {"Дніпро", 0},
                {"Одеса", 0},
                {"Львів", 0},
                {"Полтава", 0}
            };
        }
    }
}
