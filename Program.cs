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
                string _CONN_STRING = "User=root;Pwd=pass;Database=test";
                // Item table
                // +------+-------------+------+
                // | Id   | Keywords    | Name |
                // +------+-------------+------+
                // | 2005 | foo,bar,baz | John |
                // +------+-------------+------+

                using (var connection = new MySql.Data.MySqlClient.MySqlConnection(_CONN_STRING))
                {
                    var query = "SELECT * FROM Item WHERE Id=2005";
                    var item = connection.Query<Item>(query).First();
                    // Query throws an exception:
                    // System.Data.DataException: Error parsing column 1 (Keywords=foo,bar,baz - String) ---> System.InvalidCastException: Invalid cast from 'System.String' to 'System.String[]'.
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
        public string[] Keywords {get; set;}
    }
}
