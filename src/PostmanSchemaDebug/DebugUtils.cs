using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostmanSchemaDebug
{
    class DebugUtils
    {
        // Retunrs true of the Postman item is a folder
        public static bool IsFolderItem(PostmanSchemaDebug.Items item)
        {
            return item.Request == null;

        }
    }
}
