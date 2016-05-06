using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace BGale_WEBD3000_MyTunes.DAL
{
    public class MyTunesConfiguration : DbConfiguration
    {
        public MyTunesConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
        }
    }
}