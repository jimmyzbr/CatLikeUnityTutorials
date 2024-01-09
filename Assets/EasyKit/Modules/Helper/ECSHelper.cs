namespace EasyKit
{
    public static class ECSHelper
    {
        public static string GetTypeName(System.Type type)
        {
            var namespaces = type.ToString().Split('.');
            return namespaces[namespaces.Length - 1];
        }
    }
}