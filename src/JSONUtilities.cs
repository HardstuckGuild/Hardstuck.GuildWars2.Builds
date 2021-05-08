using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Hardstuck.GuildWars2.Builds
{
    public static class JSONUtilities
    {
        public static dynamic IsJSON(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) { return false; }
            input = input.Trim();
            if ((input.StartsWith("{") && input.EndsWith("}")) || //For object
                (input.StartsWith("[") && input.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(input);
                    return obj;
                }
                catch (JsonReaderException)
                {
                    //Exception in parsing json
                    return false;
                }
                catch (Exception) //some other exception
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static dynamic SearchJSONArrayByVariable(JArray array, string variable, object value)
        {
            for (int x = 0; x < array.Count; x++)
            {
                object search = array[x][variable];
                if (search.ToString() == value.ToString())
                    return array[x];
            }
            return null;
        }
    }
}
