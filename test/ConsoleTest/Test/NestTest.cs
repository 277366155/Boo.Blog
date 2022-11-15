using Boo.Blog.ToolKits.Configurations;
using Boo.Blog.ToolKits.Extensions;
using Elasticsearch.Net;
using Nest;

namespace ConsoleTest.Test
{
    public class NestTest : ITest
    {
        public void TestStart()
        {
            NestTester();
        }

        public Task TestStartAsync()
        {
            throw new NotImplementedException();
        }

        public void NestTester()
        {
            //创建连接
            var nodes = new[] { new Uri(AppSettings.Root["ElasticSearchUri"]) };
            var pool = new StaticConnectionPool(nodes);
            var settings = new ConnectionSettings(pool);
            //索引名称只能是小写
            settings.DefaultIndex("boo_database");
            var client = new ElasticClient(settings);

            //获取所有索引
            var index = client.Cat.Indices();

            //创建一个索引
            //client.Index();
            client.Indices.Create("index_test1", c => c.Map(m => m.AutoMap<Company>().AutoMap<Employee>()));
            //在索引区插入数据
            var qr1 = client.IndexDocument(new Company { CName = "testName_"+Guid.NewGuid().ToString() });
          
            //查询指定索引内部所有数据
            var val1 = client.Search<Company>(s => s.Index("boo_database")
                                                                            .Query(q => q.MatchAll()));
            var val2 = client.Search<Company>(s => s.Index("boo_database").Explain().Fields(f=>f.Field(a=>a.Employees)));

           Console.WriteLine(val2.Documents.ToJson()) ;

        }
    }

    public abstract class Document
    {
        public JoinField Join { get; set; }
    }
    public class Company : Document
    {
        public string CName { get; set; }
        public List<Employee> Employees { get; set; }
    }
    [ElasticsearchType(RelationName = "employee")]
    public class Employee
    {
        [Text(Name = "first_name", Norms = false, Similarity = "LMDirichlet")]
        public string FirstName { get; set; }

        [Text(Name = "last_name")]
        public string LastName { get; set; }

        [Number(DocValues = false, IgnoreMalformed = true, Coerce = true)]
        public int Salary { get; set; }

        [Date(Format = "MMddyyyy")]
        public DateTime Birthday { get; set; }

        [Boolean(NullValue = false, Store = true)]
        public bool IsManager { get; set; }

        [Nested]
        [PropertyName("empl")]
        public List<Employee> Employees { get; set; }

        [Text(Name = "office_hours")]
        public TimeSpan? OfficeHours { get; set; }

        [Object]
        public List<Skill> Skills { get; set; }
    }

    public class Skill
    {
        [Text]
        public string Name { get; set; }

        [Number(NumberType.Byte, Name = "level")]
        public int Proficiency { get; set; }
    }
}
