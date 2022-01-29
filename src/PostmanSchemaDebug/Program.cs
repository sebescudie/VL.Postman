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
            var coordinate = Coordinate.FromJson(System.IO.File.ReadAllText(@"D:\Documents\dev\libraries\VL.Postman\help\postman\DummyAPI.postman_collection.json"));

            foreach(var folder in coordinate.Item)
            {
                foreach(var query in folder.Item)
                {
                    foreach(var pathItem in query.Request.Value.RequestClass.Url.Value.UrlClass.Path.Value.AnythingArray)
                    {
                        Console.WriteLine(String.Format("Voilà donc {0}", pathItem.String));
                    }
                }
            }
        }
    }
}
