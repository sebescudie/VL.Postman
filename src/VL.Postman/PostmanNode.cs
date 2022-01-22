using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VL.Core;
using RestSharp;

namespace VL.Postman
{
    class PostmanNode : VLObject, IVLNode
    {
        readonly PostmanNodeDescription description;
        readonly Pin resultPin;
        readonly Pin runPin;

        // REST Client
        private RestClient client;

        public PostmanNode(PostmanNodeDescription description, NodeContext nodeContext) : base(nodeContext)
        {
            this.description = description;
            Inputs = description.Inputs.Select(p => new Pin() { Name = p.Name, Type = p.Type, Value = p.DefaultValue }).ToArray();
            Outputs = description.Outputs.Select(p => new Pin() { Name = p.Name, Type = p.Type, Value = p.DefaultValue }).ToArray();

            resultPin = Outputs.FirstOrDefault(o => o.Name == "Result");
            runPin = Inputs.LastOrDefault();
            
            var rawUrl = new Uri(description.Item.Request.Value.RequestClass.Url.Value.UrlClass.Raw);
            client = new RestClient(rawUrl.GetLeftPart(UriPartial.Authority));
        }

        public IVLNodeDescription NodeDescription => description;

        public Pin[] Inputs { get; }
        public Pin[] Outputs { get; }

        public void Update()
        {
            if (runPin is null || !(bool)runPin.Value)
                return;

            Console.WriteLine("COUILLE");

            var request = new RestRequest();
            request.Method = (Method)Enum.Parse(typeof(Method), description.Item.Request.Value.RequestClass.Method);

            // string query = String.Join("&", Inputs.SkipLast(1).Where(i => i.Value != "").Select(i => String.Format("{0}={1}", i.Name, i.Value))).ToString();

            foreach (var input in Inputs)
            {
                request.AddQueryParameter(input.Name, (string)input.Value);
                Console.WriteLine(String.Format("Just added {0}={1}", input.Name, (string)input.Value));
            }

            foreach(var param in request.Parameters)
            {
                Console.WriteLine(param.Name + " " + param.Value);
            }

            // Execute is deprecated, so we do this to make the node blocking
            resultPin.Value = client.ExecuteAsync(request).GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            Console.WriteLine("ok bye");
        }

        IVLPin[] IVLNode.Inputs => Inputs;
        IVLPin[] IVLNode.Outputs => Outputs;
    }
}
