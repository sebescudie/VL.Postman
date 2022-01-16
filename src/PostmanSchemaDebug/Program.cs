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
                    CreateNodesInFolder(item, item.Name);
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
                        CreateNodesInFolder(folder.Item[i], folder.Name + "." + folder.Item[i].Name);
                    }
                    // Otherwise we're just looking at a query so just query it
                    else
                    {
                        CreateNode(folder.Item[i], category);
                    }
                }
            }

            void CreateNode(Items item, string category)
            {
                Console.WriteLine(item.Name + "(" + category + ")");
            }
        }
    }
}
