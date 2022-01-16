using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace PostmanSchemaDebug
{
    class DebugUtils
    {
        internal const int RootLevel = 0;

        // Retunrs true of the Postman item is a folder
        public static bool IsFolderItem(PostmanSchemaDebug.Items item)
        {
            return item.Request == null;
        }

        public static void PrintCollectionTree(Items item)
        {
            try
            {
                var rootDirectory = item;
                PrintCollectionTree(rootDirectory, RootLevel);
            }
            catch(Exception e)
            {
                Console.WriteLine("Error : " + e.Message);
            }
        }

        private static void PrintCollectionTree(Items item, int currentLevel)
        {
            var indentation = string.Empty;

            for (var i = RootLevel; i < currentLevel; i++)
            {
                indentation += "-";
            }

            Console.WriteLine($"{indentation}-{item.Name}");
            
            var nextLevel = currentLevel + 1;
            try
            {
                if(item.Item != null)
                {
                    foreach (var subDirectory in item.Item)
                    {
                        PrintCollectionTree(subDirectory, nextLevel);
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error : " + e.Message);
            }
        }
    }
}