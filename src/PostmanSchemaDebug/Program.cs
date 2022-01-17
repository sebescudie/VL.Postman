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

            foreach(var item in coordinate.Item)
            {
                if(DebugUtils.IsFolderItem(item))
                {
                    // If we are in a folder, we iterate over all its nodes and instantiate them
                    CreateNodesInFolder(item, coordinate.Info.Name);
                }
                else
                {
                    // Otherwise, it means we're at the root level so just create the node
                    CreateNode(item, coordinate.Info.Name);

                }
            }

            void CreateNodesInFolder(Items folder, string category)
            {
                // We're looking at all the elements in the folder
                for(int i = 0; i < folder.Item.Count; i++)
                {
                    // If we hit a folder...
                    if(DebugUtils.IsFolderItem(folder.Item[i]))
                    {
                        // We recurse!
                        string newCategory = category + "." + folder.Name;
                        CreateNodesInFolder(folder.Item[i], newCategory);
                    }
                    // Otherwise we're just looking at a query so just create it
                    else
                    {
                        CreateNode(folder.Item[i], category + "." + folder.Name);
                    }
                }
            }

            void CreateNode(Items item, string category)
            {
                Console.WriteLine(item.Name + "(" + category + ")");
                foreach (var input in item.Request.Value.RequestClass.Url.Value.UrlClass.Query)
                {
                    Console.WriteLine(input.Key + "//" + input.Value + "//" + input.Description.Value.String);
                }
            }
        }
    }
}
