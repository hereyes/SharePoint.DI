using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using SharePoint.DI.Common;

namespace SharePoint.DI.Common
{
    /// <summary>
    /// A lot of this class was borrowed from Castle Windsor.  It searches for objets within one or more assemblies
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FromAssemblyBase<T>
    {
        /// <summary>
        ///   Scans the assembly containing specified type for types implementing the given T type,
        ///  instanciates the results, and returns them as a collection.
        /// </summary>
        /// <returns></returns>
        public ICollection<T> Containing(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            var assembly = type.Assembly;
            return Instance(assembly);
        }

        /// <summary>
        ///   Scans the assembly containing specified type for types implementing the given T type, 
        ///   instanciates the results, and returns them as a collection.
        /// </summary>
        /// <returns></returns>
        public ICollection<T> Containing<TK>()
        {
            return Containing(typeof(TK));
        }

        /// <summary>
        ///   Scans assemblies in directory specified by <paramref name = "filter" /> for types implementing the given T type, 
        /// instanciates the results, and returns them as a collection.
        /// </summary>
        /// <param name = "filter"></param>
        /// <returns></returns>
        public ICollection<T> InDirectory(AssemblyFilter filter)
        {
            return InDirectory(filter);
        }

        /// <summary>
        ///   Scans current assembly and all refernced assemblies with the same first part of the name for types implementing the given T type, 
        /// instanciates the results, and returns them as a collection.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        ///   Assemblies are considered to belong to the same application based on the first part of the name.
        ///   For example if the method is called from within <c>MyApp.exe</c> and <c>MyApp.exe</c> references <c>MyApp.SuperFeatures.dll</c>,
        ///   <c>mscorlib.dll</c> and <c>ThirdPartyCompany.UberControls.dll</c> the <c>MyApp.exe</c> and <c>MyApp.SuperFeatures.dll</c> 
        ///   will be scanned for installers, and other assemblies will be ignored.
        /// </remarks>
        public ICollection<T> InThisApplication()
        {
            var assembly = Assembly.GetCallingAssembly();
            return ApplicationAssemblies(assembly);
        }

        /// <summary>
        ///   Scans the specified assembly with specified name for types implementing the given T type, 
        /// instanciates the results, and returns them as a collection.
        /// <returns></returns>
        public ICollection<T> Instance(Assembly assembly)
        {
            var installerTypes = FilterInstallerTypes(assembly.GetAvailableTypes());
            if (installerTypes == null)
            {
                return new Collection<T>();
            }
            List<T> objects = new List<T>();
            foreach (var installerType in installerTypes)
            {
                var obj = installerType.CreateInstance<T>();
                objects.Add(obj);
            }
            return objects;
        }

        /// <summary>
        ///   Scans the assembly with specified name for types implementing the given T type, 
        /// instanciates the results, and returns them as a collection.
        /// </summary>
        /// <returns></returns>
        public ICollection<T> Named(string assemblyName)
        {
            var assembly = ReflectionUtil.GetAssemblyNamed(assemblyName);
            return Instance(assembly);
        }

        /// <summary>
        ///   Scans assembly that contains code calling this method for types implementing the given T type, 
        /// instanciates the results, and returns them as a collection.
        /// </summary>
        /// <returns></returns>
        public ICollection<T> This()
        {
            return Instance(Assembly.GetCallingAssembly());
        }

        private ICollection<T> ApplicationAssemblies(Assembly rootAssembly)
        {
            List<T> objects = new List<T>();
            var assemblies = new HashSet<Assembly>(ReflectionUtil.GetApplicationAssemblies(rootAssembly));
            foreach (var assembly in assemblies)
            {
                objects.AddRange(Instance(assembly));
            }
            return objects;
        }

        private IEnumerable<Type> FilterInstallerTypes(IEnumerable<Type> types)
        {
            return types.Where(t => t.IsClass &&
                                    t.IsAbstract == false &&
                                    t.IsGenericTypeDefinition == false &&
                                    t.Is<T>());
        }
    }
}