using System.Diagnostics;

namespace Eagle
{
    public abstract class Testable
    {
        public string Id => FullName.GetIdFromFullName();

        public string FullName { get; set; }

        public string Name { get; set; }
    }
}

