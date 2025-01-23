using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HTML_Serializer
{
    internal class HtmlHelper
    {
        private static readonly HtmlHelper _instance;
        public static HtmlHelper Instance => _instance;

        public string[] Tags { get; }
        public string[] VoidTags { get; }
        static HtmlHelper()
        {
            _instance = new HtmlHelper();
        }
        private HtmlHelper ()
        {
            string jsonContent = File.ReadAllText("JSON_files/HtmlTags.json");
            Tags = JsonSerializer.Deserialize<string[]>(jsonContent);
            jsonContent = File.ReadAllText("JSON_files/HtmlVoidTags.json");
            VoidTags = JsonSerializer.Deserialize<string[]>(jsonContent);
        }
    }
}
