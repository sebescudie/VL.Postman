using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using VL.Core;
using VL.Core.Diagnostics;

namespace VL.Postman
{
    class PostmanNodeDescription : IVLNodeDescription, IInfo
    {
        // Fields
        bool FInitialized;
        bool FError;

        string FCategory;
        string FSummary;

        public Items Item { get; set; }

        // Inputs and outputs
        List<PinDescription> inputs = new List<PinDescription>();
        List<PinDescription> outputs = new List<PinDescription>();

        public PostmanNodeDescription(IVLNodeDescriptionFactory factory, string name, string category, Items item)
        {
            Factory = factory;
            Name = name;
            FCategory = category;
            Item = item;
        }

        void Init()
        {
            if (FInitialized)
                return;

            try
            {
                foreach(var input in Item.)
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        void GetTypeDefaultAndDescription(dynamic pin, ref Type type, ref object dflt, ref string desc)
        {
            // Maybe
        }

        public IVLNodeDescriptionFactory Factory { get; }
        public string Name { get; }
        public string Category => FCategory;
        public bool Fragmented => false;

        public IReadOnlyList<IVLPinDescription> Inputs
        {
            get
            {
                Init();
                return inputs;
            }
        }

        public IReadOnlyList<IVLPinDescription> Outputs
        {
            get
            {
                Init();
                return outputs;
            }
        }

        public IEnumerable<Core.Diagnostics.Message> Messages
        {
            get
            {
                if (FError)
                    yield return new Message(MessageType.Warning, "Grrrrr");
                else
                    yield break;
            }
        }

        public string Summary => FSummary;

        public string Remarks => "";

        public IObservable<object> Invalidated => Observable.Empty<object>();

        public IVLNode CreateInstance(NodeContext context)
        {
            return new PostmanNode(this, context);
        }

        public bool OpenEditor()
        {
            return true;
        }
    }
}
