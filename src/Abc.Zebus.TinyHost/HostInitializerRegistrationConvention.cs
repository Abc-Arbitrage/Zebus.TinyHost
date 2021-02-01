using Abc.Zebus.Hosting;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;
using StructureMap.TypeRules;

namespace Abc.Zebus.TinyHost
{
    public class HostInitializerRegistrationConvention : IRegistrationConvention
    {
        public void ScanTypes(TypeSet types, Registry registry)
        {
            foreach (var type in types.AllTypes())
            {
                if (!typeof(HostInitializer).IsAssignableFrom(type))
                    continue;

                if (!type.CanBeCreated() || type.ContainsGenericParameters)
                    continue;

                registry.For(typeof(HostInitializer)).Singleton().Add(ctx => ctx.GetInstance(type));
            }
        }
    }
}
