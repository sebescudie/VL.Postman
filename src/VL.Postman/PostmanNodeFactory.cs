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
    public class PostmanNodeFactory : IVLNodeDescriptionFactory
    {
        const string postmanSubDir = "postman";
        const string identifier = "VL.Postman-Factory";

        public readonly string Directory;
        public readonly string DirectoryToWatch;

        // The node factory cache will invalidate in case a cached factory or one of its nodes invalidates
        private readonly NodeFactoryCache factoryCache = new NodeFactoryCache();

        public PostmanNodeFactory(string directory = default, string directoryToWatch = default)
        {
            Directory = directory;
            DirectoryToWatch = directoryToWatch;

            var builder = ImmutableArray.CreateBuilder<IVLNodeDescription>();

            if (directory != null)
            {
                // Iterate over all postman files
                foreach (var file in System.IO.Directory.GetFiles(directory, "*.json"))
                {
                    Console.WriteLine(file);
                    var coordinate = Coordinate.FromJson(System.IO.File.ReadAllText(file));
                    
                    // We iterate over all the elements. These could be folder or requests, who knows.
                    foreach (var item in coordinate.Item)
                    {
                        // Well, we do
                        if(Utils.IsFolderItem(item))
                        {
                            // If we are in a folder, we iterate over all its queries and make nodes out of it
                            Utils.CreateNodesInFolder(item, coordinate.Info.Name, builder, this);
                        }
                        else
                        {
                            // Otherwise, we just create the node
                            Utils.CreateNode(item, coordinate.Info.Name, builder, this);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Could not find /postman subdir");
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
                if (Directory != null)
                {
                    return NodeBuilding.WatchDir(Directory).Where(e => e.ChangeType == WatcherChangeTypes.All);
                }
                else if (DirectoryToWatch != null)
                {
                    return NodeBuilding.WatchDir(DirectoryToWatch).Where(e => e.ChangeType == WatcherChangeTypes.All);
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
                    return new PostmanNodeFactory(postmanDir);
                return new PostmanNodeFactory(directoryToWatch: path);
            });
        }
    }
}