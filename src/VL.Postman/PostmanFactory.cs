using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using VL.Core;
using VL.Core.Diagnostics;
using Newtonsoft.Json;
using VL.Lib.Basics.Imaging;
using VL.Lang.PublicAPI;
using VL.Lib.Collections;
using Stride.Core.Mathematics;
using System.Reactive.Linq;

namespace VL.Postman
{
    public class PostmanFactory : IVLNodeDescriptionFactory
        {
            const string postmanSubDir = "postman";
            const string identifier = "VL.Postman-Factory";

            public readonly string Directory;
            public readonly string DirectoryToWatch;

            // The node factory cache will invalidate in case a cached factory or one of its nodes invalidates
            private readonly NodeFactoryCache factoryCache = new NodeFactoryCache();

            public PostmanFactory(string directory = default, string directoryToWatch = default)
            {
                Directory = directory;
                DirectoryToWatch = directoryToWatch;

                var builder = ImmutableArray.CreateBuilder<IVLNodeDescription>();
                if(directory != null)
                {
                    // Iterate over all postman files
                    foreach(var file in System.IO.Directory.GetFiles(directory))
                    {
                        var coordinate = Coordinate.FromJson(System.IO.File.ReadAllText(file));
                        foreach(var request in coordinate.Item)
                        {
                            builder.Add(new PostmanNodeDescription(this, request.Name));
                        }
                    }
                }
                NodeDescriptions = builder.ToImmutable();
            }

            public ImmutableArray<IVLNodeDescription> NodeDescriptions { get; }
            
            public string Identifier
            {
                get
                {
                    if (Directory != null)
                        return GetIdentifierForPath(Directory);
                    else
                        return identifier;
                }
            }

            public IObservable<object> Invalidated
            {
                get
                {
                    if(Directory != null)
                    {
                        return Observable.Empty<object>();
                    }
                    else if (DirectoryToWatch != null)
                    {
                        return Observable.Empty<object>();
                    }
                    else
                    {
                        return Observable.Empty<object>();
                    }
                }
            }

            public void Export(ExportContext exportContext)
            {

            }

            string GetIdentifierForPath(string path) => $"{identifier} ({path})";

            public IVLNodeDescriptionFactory ForPath(string path)
            {
                // VL caches node factories per compilation only, not across compilations -> use our own cache
                var identifier = GetIdentifierForPath(path);
                return factoryCache.GetOrAdd(identifier, () =>
                {
                    var postmanDir = Path.Combine(path, postmanSubDir);
                    if (System.IO.Directory.Exists(postmanDir))
                        return new PostmanFactory(postmanDir);
                    return new PostmanFactory(directoryToWatch: path);
                });
            }

            class PinDescription : IVLPinDescription, IInfo
            {
                static string BeautifyPin(string s)
                {
                    // Sanitize string
                    return s;
                }

                public PinDescription(string name, Type type, object defaultValue, string description)
                {
                    name = BeautifyPin(name);
                    OriginalName = name;
                    Type = type;
                    DefaultValue = defaultValue;
                    Summary = description;
                }

                public string Name { get; }
                public string OriginalName { get; }
                public Type Type { get; set; }
                public object DefaultValue { get; }

                public string Summary { get; }

                public string Remarks => "";
            }

            class PostmanNodeDescription : IVLNodeDescription, IInfo
            {
                bool FInitialized;

                string FUrl;
                string FFullName;
                string FSummary;
                List<PinDescription> inputs = new List<PinDescription>();
                List<PinDescription> outputs = new List<PinDescription>();

                public PostmanNodeDescription(IVLNodeDescriptionFactory factory, string name)
                {
                    Factory = factory;
                    FFullName = name;
                }

                void Init()
                {
                    if (FInitialized)
                        return;

                    try
                    {
                        // This is where we retrieve stuff from the json dump
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                void GetTypeDefaultAndDescription(dynamic pin, ref Type type, ref object dflt, ref string desc)
                {

                }

                public IVLNodeDescriptionFactory Factory { get; }
                public string Name { get; }
                public string Category => "IO.HTTP";
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
                        yield break;
                    }
                }

                public string Summary => FSummary;

                public string Remarks => "";

                public IObservable<object> Invalidated => Observable.Empty<object>();

                public IVLNode CreateInstance(NodeContext context)
                {
                    return new MyNode(this, context);
                }

                public bool OpenEditor()
                {
                    return true;
                }
            }

            class MyNode : VLObject, IVLNode
            {
                class MyPin : IVLPin
                {
                    public object Value { get; set; }
                    public Type Type { get; set; }
                    public string Name { get; set; }
                    public string OriginalName { get; set; }
                }

                readonly PostmanNodeDescription description;

                public MyNode(PostmanNodeDescription description, NodeContext nodeContext) : base(nodeContext)
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

                    if((bool)Inputs.Last().Value)
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
}
