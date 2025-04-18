using System.Text.Json;

namespace Utility.Helpers
{
    public static class HelperObjectSerializer
    {
        public static string ConvertObjectToString(Object obj) 
        {
            return JsonSerializer.Serialize(obj);
        } 
    }
}
