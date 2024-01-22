using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Synchronizer.PriceManager
{
    public abstract class PriceManager
    {
        public enum City
        {
            Харків,
            Дніпро,
            Одеса,
            Львів,
            Полтава,
            Київ
        }

        protected static Regex regex = new Regex(@"(\d+(\.\d+)?)");

        protected static HtmlDocument? GetPageContent(string url)
        {
            if (url != null || url != string.Empty)
            {
                HtmlWeb web = new HtmlWeb();
                var htmlDocument = web.Load(url);
                return htmlDocument;
            }

            return null;
        }
    }
}
