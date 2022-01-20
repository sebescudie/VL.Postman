using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace PostmanSchemaDebug
{
    class Program
    {
        static void Main(string[] args)
        {
            var coordinate = Coordinate.FromJson(System.IO.File.ReadAllText(@"D:\documents\dev\vvvv\libraries\VL.Postman\help\postman\ACME.postman_collection.json"));

            foreach(var folder in coordinate.Item)
            {
                foreach(var query in folder.Item)
                {
                    Console.WriteLine("=======");
                    Console.WriteLine(query.Name);
                    Console.WriteLine(String.Format("Has description : {0}", query.Request.Value.RequestClass.Description.HasValue));
                    if (query.Request.Value.RequestClass.Description.HasValue)
                        Console.WriteLine(String.Format("Description : {0}", query.Request.Value.RequestClass.Description.Value.String));
                }

            }

        }
    }
}
