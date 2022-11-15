using DotNetCore.CAP;
using DotNetCore.CAP.MySql;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CAPTest.ConsoleApp.Services
{
    public class MyTableInitalizer : MySqlStorageInitializer
    {
        public MyTableInitalizer(ILogger<MySqlStorageInitializer> logger, IOptions<MySqlOptions> options) : base(logger, options)
        {
        }
        public override string GetPublishedTableName()
        {
            return "orders_published";
        }
        public override string GetReceivedTableName()
        {
            return "orders_received";
        }
    }
}
