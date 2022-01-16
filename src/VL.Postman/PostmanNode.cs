using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VL.Core;

namespace VL.Postman
{
    class PostmanNode : VLObject, IVLNode
    {
        class MyPin : IVLPin
        {
            public object Value { get; set; }
            public Type Type { get; set; }
            public string Name { get; set; }
            public string OriginalName { get; set; }
        }

        readonly PostmanNodeDescription description;

        public PostmanNode(PostmanNodeDescription description, NodeContext nodeContext) : base(nodeContext)
        {
            this.description = description;
            Inputs = description.Inputs.Select(p => new MyPin() { Name = p.Name, OriginalName = ((PinDescription)p).OriginalName, Type = p.Type, Value = p.DefaultValue }).ToArray();
            Outputs = description.Outputs.Select(p => new MyPin() { Name = p.Name, OriginalName = ((PinDescription)p).OriginalName, Type = p.Type, Value = p.DefaultValue }).ToArray();
        }

        public IVLNodeDescription NodeDescription => description;

        public IVLPin[] Inputs { get; }
        public IVLPin[] Outputs { get; }

        public void Update()
        {
            if (!Inputs.Any())
                return;

            if ((bool)Inputs.Last().Value)
            {
                // Do the query!
            }
        }

        public void Dispose()
        {
            Console.WriteLine("ok bye");
        }
    }
}
