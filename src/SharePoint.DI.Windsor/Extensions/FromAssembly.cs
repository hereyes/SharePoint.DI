using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.MicroKernel.Registration;
using SharePoint.DI.Common;

namespace SharePoint.DI.Windsor
{
    public static class FromAssembly
    {
        private static FromAssemblyBase<IWindsorInstaller> from = new FromAssemblyBase<IWindsorInstaller>();

        public static ICollection<IWindsorInstaller> Containing(Type type)
        {
            return from.Containing(type);
        }

        public static ICollection<IWindsorInstaller> Containing<T>()
        {
            return from.Containing<T>();
        }

        public static ICollection<IWindsorInstaller> InDirectory(Common.AssemblyFilter filter)
        {
            return from.InDirectory(filter);
        }

        public static ICollection<IWindsorInstaller> InThisApplication()
        {
            return from.InThisApplication();
        }

        public static ICollection<IWindsorInstaller> Instance(Assembly assembly)
        {
            return from.Instance(assembly);
        }

        public static ICollection<IWindsorInstaller> Named(string assemblyName)
        {
            return from.Named(assemblyName);
        }

        public static ICollection<IWindsorInstaller> This()
        {
            return from.This();
        }
    }
}
