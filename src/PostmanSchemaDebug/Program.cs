using System;
using System.Linq;

namespace PostmanSchemaDebug
{
    class Program
    {
        static void Main(string[] args)
        {
            var coordinate = Coordinate.FromJson(System.IO.File.ReadAllText(@"D:\documents\dev\vvvv\libraries\VL.Postman\help\postman\ACME.postman_collection.json"));
            foreach(var item in coordinate.Item)
            {
                Console.WriteLine("===");
                Console.WriteLine(item.Name);
                Console.WriteLine("Is it a folder?");
                Console.WriteLine(DebugUtils.IsFolderItem(item));
                coordinate
            }
        }
    }
}
