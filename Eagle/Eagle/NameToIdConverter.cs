namespace WonderTools.Eagle.Core
{
    public static class NameToIdConverter
    {
        public static string GetIdFromFullName(this string fullName)
        {
            return "id" + fullName;
        }
    }
}