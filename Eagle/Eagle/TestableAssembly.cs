using System;
using System.Reflection;

namespace Eagle
{
    public class TestableAssembly
    {
        public static implicit operator TestableAssembly(Assembly assembly)
        {
            return new TestableAssembly() {Location = assembly.Location};
        }

        public static implicit operator TestableAssembly(Type type)
        {
            return new TestableAssembly() { Location = type.Assembly.Location };
        }
        public string Location { get; private set; }
    }
}