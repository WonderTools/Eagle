using System;
using System.Reflection;

namespace Eagle
{
    public class TestAssemblyLocationHolder
    {
        public static implicit operator TestAssemblyLocationHolder(Assembly assembly)
        {
            return new TestAssemblyLocationHolder() {Location = assembly.Location};
        }

        public static implicit operator TestAssemblyLocationHolder(Type type)
        {
            return new TestAssemblyLocationHolder() { Location = type.Assembly.Location };
        }
        public string Location { get; private set; }
    }
}