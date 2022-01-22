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
                    // Retrieve REST client (gets domain name + port)
                    Uri uriAddress = new Uri(query.Request.Value.RequestClass.Url.Value.UrlClass.Raw);
                    Console.WriteLine(uriAddress.GetLeftPart(UriPartial.Authority));

                    // Get parameters as dictionary
                    var parameters = System.Web.HttpUtility.ParseQueryString(uriAddress.Query);
                    foreach(var param in parameters)
                    {

                    }
                }
            }
        }
    }
}
