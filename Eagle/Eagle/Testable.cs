namespace Eagle
{
    public abstract class Testable
    {
        public string Id => "id"+ FullName;

        public string FullName { get; set; }

        public string Name { get; set; }
    }
}