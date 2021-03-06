using System;
using System.Linq;
using System.Reflection;

namespace Pat.Subscriber.SubscriberRules
{
    public class RuleVersionResolver: IRuleVersionResolver
    {
        private readonly Assembly[] _handlerAssemblies;

        public RuleVersionResolver(Assembly[] handlerAssemblies = null)
        {
            _handlerAssemblies = handlerAssemblies;
        }

        public Version GetVersion()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            return GetVersion(entryAssembly);
        }

        public Version GetVersion(Assembly entryAssembly)
        {
            if (entryAssembly != null)
            {
                return entryAssembly.GetName().Version;
            }

            if (_handlerAssemblies == null || !_handlerAssemblies.Any())
            {
                throw new InvalidOperationException("Cannot retrieve rule version as entry assembly and handler assemblies list are both null or empty.");
            }

            return HighestHandlerAssemblyVersion;
        }

        private Version HighestHandlerAssemblyVersion
            => _handlerAssemblies.Select(assembly => assembly.GetName().Version).Max();
    }
}