using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VL.Core;
using System.Collections.Immutable;

namespace VL.Postman
{
    class Utils
    {
        // Returns true of this item is a folder
        public static bool IsFolderItem(Items item)
        {
            return item.Request == null;
        }

        public static void CreateNodesInFolder(Items folder, string category, ImmutableArray<IVLNodeDescription>.Builder builder, IVLNodeDescriptionFactory factory)
        {
            // We're looking at all the elents in the folder
            for(int i = 0; i < folder.Item.Count; i++)
            {
                // If we hit a folder...
                if(IsFolderItem(folder.Item[i]))
                {
                    // We recurse!
                    string newCategory = category + "." + folder.Name;
                    CreateNodesInFolder(folder.Item[i], newCategory, builder, factory);
                }
                else
                // Otherwise we're looking at a query so just create it
                {
                    CreateNode(folder.Item[i], category + "." + folder.Name, builder, factory);
                }
            }
        }

        public static void CreateNode(Items item, string category, ImmutableArray<IVLNodeDescription>.Builder builder, IVLNodeDescriptionFactory factory)
        {
            builder.Add(new PostmanNodeDescription(factory, item.Name, category));
        }
    }
}
