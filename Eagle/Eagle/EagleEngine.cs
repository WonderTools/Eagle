using System.Collections.Generic;

namespace Eagle
{
    public class EagleEngine
    {
        public List<NameAndId> GetFeatureNames()
        {
            return new List<NameAndId>()
            {
                new NameAndId() {Id = "sfdas", Name = "FirstFeature"}
            };
        }
    }
}