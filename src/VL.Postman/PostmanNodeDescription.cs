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
                // Iterate over the inputs
                foreach(var input in Item.Request.Value.RequestClass.Url.Value.UrlClass.Query)
                {
                    inputs.Add(new PinDescription(input.Key, typeof(string), input.Value, input.Description.Value.String));
                }

                // Add the trigger pin
                inputs.Add(new PinDescription("Execute", typeof(bool), false, "Sends a query as long as enabled"));
                
                // Add a fake input for now
                outputs.Add(new PinDescription("Result", typeof(string), "", "The result"));

                FInitialized = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
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

        public string Summary => "";

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
