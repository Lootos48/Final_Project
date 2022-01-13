using CardFile.Web.Enums;

namespace CardFile.Web.Models
{
    public class PageFiltration
    {
        public FilterOptions SearchBy { get; set; }
        
        public string SearchString { get; set; }
    }
}