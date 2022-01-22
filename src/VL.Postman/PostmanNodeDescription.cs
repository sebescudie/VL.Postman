using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using VL.Core;
using VL.Core.Diagnostics;
using RestSharp;

namespace VL.Postman
{
    class PostmanNodeDescription : IVLNodeDescription, IInfo
    {
        // Fields
        bool FInitialized;
        bool FError;

        string FSummary;
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
                string dflt = "";
                string description = "";
                
                FSummary = "";

                // Iterate over the inputs
                foreach(var input in Item.Request.Value.RequestClass.Url.Value.UrlClass.Query)
                {
                    EnsureDefaultAndDescription(input, ref dflt, ref description);
                    inputs.Add(new PinDescription(input.Key, typeof(string), dflt, description));
                }

                // Add the trigger pin
                inputs.Add(new PinDescription("Execute", typeof(bool), false, "Sends a query as long as enabled"));
                
                // Add a fake input for now
                outputs.Add(new PinDescription("Result", typeof(RestResponse), null, "The result"));

                if(Item.Request.Value.RequestClass.Description.HasValue)
                {
                    FSummary = Item.Request.Value.RequestClass.Description.Value.String;
                }
                else
                {
                    FSummary = String.Format("Runs the {0} query", Item.Name);
                }

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

        private void EnsureDefaultAndDescription(QueryParam param, ref string dflt, ref string description)
        {
            dflt = "";
            description = "";

            // If the value does not start with a mustache, it means it's not a variable
            // In this case we just assign the value from the JSON dump
            if(!param.Value.StartsWith("{"))
            {
                dflt = param.Value;
            }

            if(param.Description.HasValue)
            {
                description = param.Description.Value.String;
            }
            else
            {
                description = "";
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
