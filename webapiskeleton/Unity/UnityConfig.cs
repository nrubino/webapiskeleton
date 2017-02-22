using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using webapiskeleton.Interceptors;

namespace webapiskeleton.Unity
{
    public static class UnityConfig
    {
        public static void RegisterTypes(HttpConfiguration config)
        {
            var container = new UnityContainer();

            //Add interception extension
            container.AddNewExtension<Interception>();

            //Register xCache
            //container.RegisterType<ICache, MemoryCache>();
            //container.RegisterType<ICacheKeyGenerator, JsonCacheKeyGenerator>();

            //finds all class we need registered in unity
            var dalClasses = AllClasses.FromLoadedAssemblies()
                .Where(
                    t =>
                        t.Namespace != null && t.Namespace.Contains("NAME SPACES TO REGISTER HERE"));

            //registers classes with logging interceptor
            container.RegisterTypes(
                dalClasses,
                WithMappings.FromMatchingInterface,
                getInjectionMembers: t => new InjectionMember[]
                {
                    new Interceptor<InterfaceInterceptor>(),
                    //FOR CACHEING
                    //new InterceptionBehavior<PolicyInjectionBehavior>(),
                    new InterceptionBehavior<LoggingInterceptor>(),
                }
                );

            //Manual registration
            //container.RegisterType<IExcelUtil, ExcelUtil>();

            //LEAVING THIS CODE HERE AS AN EXAMPLE OF A SINGLETON REGISTRATION WITH THE HierarchicalLifetimeManager
            //HierarchicalLifetimeManager hooks into the mvc lifecycle so it runs the dispose method of any registered
            //object at the end of a page request.

            //This registration calls the SalesforceConnectionPool Singleton to get a connection everytime a connection
            //is requested.  The HierarchicalLifetimeManager is important here because it is makes sure the Dispose()
            //method is called on the SalesforceConnection that puts it back into the SalesforceConnectionPool.
            //container.RegisterType<ISalesforceConnection>(new HierarchicalLifetimeManager(),
            //    new InjectionFactory(s => container.Resolve<ISalesforceConnectionPool>().GetConnection()));

            config.DependencyResolver = new UnityResolver(container);
        }
    }
}