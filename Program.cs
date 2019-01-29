using System;
using System.Collections.Generic;
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

    class KeywordsList : List<string>
    {

    }

    class Item
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public KeywordsList Keywords {get; set;}
    }

    class KeywordsHandler : SqlMapper.TypeHandler<KeywordsList>
    {
        public override KeywordsList Parse(object value)
        {
            var result = new KeywordsList();
            result.AddRange(value.ToString().Split(","));
            return result;
        }

        public override void SetValue(IDbDataParameter parameter, KeywordsList value)
        {
            throw new NotImplementedException();
        }
    }
}
