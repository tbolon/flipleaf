using System;
using System.Collections.Generic;

using Fluid;

namespace FlipLeaf.Core.Text.FluidLiquid
{
    internal class IgnoreCaseMemberAccessStrategy : IMemberAccessStrategy
    {
        private Dictionary<string, IMemberAccessor> _map = new Dictionary<string, IMemberAccessor>(StringComparer.OrdinalIgnoreCase);

        public IMemberAccessor GetAccessor(object obj, string name)
        {
            // Look for specific property map
            if (_map.TryGetValue(Key(obj.GetType(), name), out var getter))
            {
                return getter;
            }

            // Look for a catch-all getter
            if (_map.TryGetValue(Key(obj.GetType(), "*"), out getter))
            {
                return getter;
            }

            return null;
        }

        public void Register(Type type, string name, IMemberAccessor getter)
        {
            _map[Key(type, name)] = getter;
        }

        private string Key(Type type, string name) => $"{type.Name}.{name}";
    }
}
