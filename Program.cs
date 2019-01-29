using System;
using System.Data;
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
                SqlMapper.AddTypeHandler(new KeywordsHandler());
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
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(item, Newtonsoft.Json.Formatting.Indented);
                    Console.WriteLine(json);
                    //returns
                    // {
                    //     "Id": "2005",
                    //     "Name": "John",
                    //     "Keywords": [
                    //         "foo",
                    //         "bar",
                    //         "baz"
                    //     ]
                    // }
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

    class KeywordsHandler : SqlMapper.TypeHandler<string[]>
    {
        public override string[] Parse(object value)
        {
            return value.ToString().Split(",").ToArray();
        }

        public override void SetValue(IDbDataParameter parameter, string[] value)
        {
            throw new NotImplementedException();
        }
    }
}
