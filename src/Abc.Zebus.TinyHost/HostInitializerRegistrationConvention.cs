using System;
using Abc.Zebus.Hosting;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using StructureMap.TypeRules;

namespace Abc.Zebus.TinyHost
{
    public class HostInitializerRegistrationConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (!typeof(HostInitializer).IsAssignableFrom(type))
                return;

            if (!type.CanBeCreated() || type.ContainsGenericParameters)
                return;

            registry.For(typeof(HostInitializer)).Singleton().Add(ctx => ctx.GetInstance(type));
        }
    }
}