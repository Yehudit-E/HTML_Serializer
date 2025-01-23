using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HTML_Serializer
{
    internal class HtmlTree
    {
        public HtmlElement Root { get; set; }

        public HtmlTree()
        {
            Root = new HtmlElement();
            Root.Name = "html";
            Root.Children = new List<HtmlElement>();
        }
        public void BuildTree(IEnumerable<string> htmlLines)
        {
            var HtmlList = htmlLines.SkipWhile(s => !s.StartsWith("html")).ToList();

            HtmlElement currentElement = Root;
            foreach (string line in HtmlList)
            {
                var firstWord = line.Trim().Split(" ")[0];
                if (firstWord == "html")
                {
                    var attributesRoot = ExtractAttributes(line);
                    Root.Attributes = attributesRoot.Item1;
                    Root.Classes = attributesRoot.Item2;
                    if (attributesRoot.Item1.Any(s => s.StartsWith("id")))
                    {
                        Root.Id = attributesRoot.Item1.FirstOrDefault(a => a.StartsWith("id="))?.Split('=')[1];
                    }
                }
                else
                {
                    if (firstWord == "/html")
                        return;
                    if (firstWord.StartsWith("/")&& HtmlHelper.Instance.Tags.Contains(firstWord.Substring(1)))
                    {
                        currentElement = currentElement.Parent;
                    }
                    else if (HtmlHelper.Instance.Tags.Contains(firstWord)
                        || (firstWord!="" && HtmlHelper.Instance.VoidTags.Contains(firstWord.Substring(0, firstWord.Length - 1)))
                        || HtmlHelper.Instance.VoidTags.Contains(firstWord))
                    {
                        var newElement = new HtmlElement
                        {
                            Name = firstWord,
                            Children = new List<HtmlElement>(),
                            Parent = currentElement
                        };
                        if(newElement.Name.EndsWith("/"))
                            newElement.Name= newElement.Name.Substring(0,newElement.Name.Length - 1);
                        var attributes = ExtractAttributes(line);
                        newElement.Attributes = attributes.Item1;
                        newElement.Classes = attributes.Item2;

                        if (attributes.Item1.Any(s => s.StartsWith("id")))
                        {
                            newElement.Id = attributes.Item1.FirstOrDefault(a => a.StartsWith("id="))?.Trim().Split('=')[1];
                        }
                        currentElement.Children.Add(newElement);
                        if (!HtmlHelper.Instance.VoidTags.Contains(firstWord) && !firstWord.EndsWith("/"))
                        {
                            currentElement = newElement;
                        }
                    }
                    else
                    {
                        currentElement.InnerHtml = line;
                    }
                }
            }
        }
        public Tuple<List<string>, List<string>> ExtractAttributes(string line)
        {
            var attributes = new List<string>();
            var classes = new List<string>();

            var matches = Regex.Matches(line, @"(\w+)=""([^""]+)""");

            foreach (Match match in matches)
            {
                var attribute = match.Groups[1].Value;
                var value = match.Groups[2].Value;

                attributes.Add($"{attribute}={value}");

                if (attribute == "class")
                {
                    classes.AddRange(value.Trim().Split(' '));
                }
            }
            return new Tuple<List<string>, List<string>>(attributes, classes);
        }

    }
}
