using Ganss.Xss;
using System.Reflection;

namespace Utility.Helpers
{
    public static class HelperHtmlSanitizer
    {
        public static void Sanitize<T>(T obj)
        {
            if (obj == null) 
            {
                return;
            }

            var sanitizer = new HtmlSanitizer();

            var stringProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));

            foreach (var prop in stringProperties)
            {
                var originalValue = (string)prop.GetValue(obj)!;
                if (!string.IsNullOrEmpty(originalValue))
                {
                    // Sanitize and set back the cleaned string
                    var cleanedValue = sanitizer.Sanitize(originalValue);
                    prop.SetValue(obj, cleanedValue);
                }
            }
        }
    }
}
