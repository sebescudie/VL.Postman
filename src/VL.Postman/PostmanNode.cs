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
        }

        public IVLNodeDescription NodeDescription => description;

        public Pin[] Inputs { get; }
        public Pin[] Outputs { get; }

        public void Update()
        {
            if (runPin is null || !(bool)runPin.Value)
                return;

            Console.WriteLine(String.Format("There are {0} inputs", Inputs.Length));


            var query = String.Join("&", Inputs.SkipLast(1).Where(i => (string)i.Value != "").Select(i => String.Format("{0}={1}", i.Name, i.Value))).ToString();
            var url = description.Item.Request.Value.RequestClass.Url.Value.UrlClass;
            var host = String.Join(".", url.Host.Value.StringArray);
            var path = String.Join("/", url.Path.Value.AnythingArray.Select(i => i.String));
            var fullURL = String.Format("{0}://{1}/{2}?{3}", url.Protocol, host, path, query);

            var clientOptions = new RestClientOptions(fullURL){
                ThrowOnAnyError = true,
                Timeout = 1000
            };

            var client = new RestClient(clientOptions);

            var request = new RestRequest();
            
            //// Execute is deprecated, so we do this to make the node blocking
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
