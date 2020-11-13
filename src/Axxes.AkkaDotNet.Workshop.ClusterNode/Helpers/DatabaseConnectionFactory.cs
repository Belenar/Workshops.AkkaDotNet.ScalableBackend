using System.Data.SqlClient;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Helpers
{
    class DatabaseConnectionFactory
    {
        public static string HistoryConnectionString { get; set; }

        public SqlConnection GetHistoryConnection()
        {
            return new SqlConnection(HistoryConnectionString);
        }
    }
}
