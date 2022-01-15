using VL.Core;
using VL.Core.CompilerServices;

// Tell VL where to find our initializer
[assembly: AssemblyInitializer(typeof(VL.Postman.Initialization))]

namespace VL.Postman
{
    public class Initialization : AssemblyInitializer<Initialization>
    {
        protected override void RegisterServices(IVLFactory factory)
        {
            factory.RegisterNodeFactory(postmanFactory);
        }

        static IVLNodeDescriptionFactory postmanFactory = new PostmanFactory();
    }
}
