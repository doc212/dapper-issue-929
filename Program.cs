using System;
using System.Linq;
using Dapper;

namespace dapper_issue_929
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string _CONN_STRING = "User=root;Pwd=pass";
                using (var connection = new MySql.Data.MySqlClient.MySqlConnection(_CONN_STRING))
                {
                    var query = "SELECT * FROM test.Item WHERE Id=2005";
                    var item = connection.Query<Item>(query).First();
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(item, Newtonsoft.Json.Formatting.Indented);
                    Console.WriteLine(json);
                }
                Console.WriteLine("done");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }

    class Item
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
