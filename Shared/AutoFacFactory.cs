using System;
using Autofac;
using IContainer = Autofac.IContainer;

namespace Shared
{
    public static class AutoFacFactory
    {
        private static IContainer _container;
        private static readonly ContainerBuilder ContainerBuilder = new ContainerBuilder();

        public static void Build()
        {
            _container = ContainerBuilder.Build();
        }

        public static void RegisterType(Type implementation, Type service)
        {
            ContainerBuilder.RegisterType(implementation).As(service);
        }

        public static void RegisterInstance(object singleObject, Type service)
        {
            ContainerBuilder.RegisterInstance(singleObject).As(service);
        }
    }
}