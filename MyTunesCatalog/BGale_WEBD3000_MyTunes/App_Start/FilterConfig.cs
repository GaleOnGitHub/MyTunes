using System.Web;
using System.Web.Mvc;

namespace BGale_WEBD3000_MyTunes
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
