using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HTML_Serializer
{
    internal class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public Selector()
        {
            Classes = new List<string>();
        }

        public static Selector Parse(string query)
        {
            var levels = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Selector root = null;
            Selector currentSelector = root;
            
            foreach (var level in levels)
            {
                var newSelector = new Selector();
                var parts = Regex.Split(level,@"(?=[#.])");
                foreach (var part in parts)
                {
                    if (string.IsNullOrWhiteSpace(part))
                        continue;
                    if (part.StartsWith("#"))
                    {
                        newSelector.Id = part.Substring(1);
                    }
                    else if (part.StartsWith("."))
                    {
                        newSelector.Classes.Add(part.Substring(1));
                    }
                    else
                    {
                       
                        if (HtmlHelper.Instance.Tags.Contains(part))
                        {
                            newSelector.TagName = part;
                        }
                        else
                        {
                            throw new ArgumentException($"Invalid tag name: {part}");
                        }
                    }
                }
                if (currentSelector == null)
                {
                    root = newSelector;
                    currentSelector = root;
                }
                else
                {
                    currentSelector.Child = newSelector;
                    newSelector.Parent = currentSelector;

                    currentSelector = currentSelector.Child;
                }

            }
            return root;
        }



        

    }
}
