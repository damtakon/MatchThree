using System;
using System.Collections.Generic;
using Autofac;
using IContainer = Autofac.IContainer;

namespace MatchThree.Core
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

        public static object[] TypesResolve(params Type[] types)
        {
            var typesResolve = new List<object>();
            using (var scope = _container.BeginLifetimeScope())
            {
                foreach (var type in types)
                    typesResolve.Add(scope.Resolve(type));
            }

            return typesResolve.ToArray();
        }
    }
}