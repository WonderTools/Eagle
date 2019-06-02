namespace Eagle
{
    public abstract class Testable
    {
        public string Id
        {
            //TBD: Id has to be computed
            get { return FullName; }
        }

        public string FullName { get; set; }

        public string Name { get; set; }
    }
}