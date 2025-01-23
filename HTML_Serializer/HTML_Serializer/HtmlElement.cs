using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTML_Serializer
{
    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }



        public override string ToString()
        {
            string s = "Name: " + Name + "\n";
            if (Id != null)
                s += "Id: " + Id + "\n";
            if (Attributes.Count != 0)
            {
                s += "Attributes: ";
                foreach (string atr in Attributes)
                {
                    s += atr + " ";
                }
                s += "\n";
            }
            if (Classes.Count != 0)
            {
                s += "Classes: ";
                foreach (string class1 in Classes)
                {
                    s += class1 + " ";
                }
                s += "\n";
            }
            if (InnerHtml != null)
                s += "InnerHtml: " + InnerHtml + "\n";
            return s;
        }
        public IEnumerable<HtmlElement> Descendants()
        {
            var queue = new Queue<HtmlElement>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                yield return current;

                foreach (var child in current.Children)
                {
                    queue.Enqueue(child);
                }
            }

        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            var current = this;

            while (current != null)
            {
                yield return current; 
                current = current.Parent; 
            }
        }
        private void RecursiveSearch(HtmlElement currentElement, Selector currentSelector, HashSet<HtmlElement> result)
        {
            if (currentSelector == null)
                return;

            var descendants = currentElement.Descendants();

            var filtered = descendants.Where(el =>
                ((string.IsNullOrEmpty(currentSelector.TagName) || el.Name == currentSelector.TagName)) &&
                ((string.IsNullOrEmpty(currentSelector.Id) || el.Id == currentSelector.Id)) &&
                ((currentSelector.Classes == null || currentSelector.Classes.All(cls => el.Classes.Contains(cls))))
            );

            if (currentSelector.Child == null)
            {
                foreach (var match in filtered)
                    result.Add(match);
            }
            else
            {
                foreach (var match in filtered)
                    RecursiveSearch(match, currentSelector.Child, result);
            }
        }

        public  IEnumerable<HtmlElement> FindElementsBySelector( Selector selector)
        {
            var resultSet = new HashSet<HtmlElement>();
            RecursiveSearch(this, selector, resultSet);
            return resultSet.ToList();
        }

    }
}
