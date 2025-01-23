
using HTML_Serializer;
using System.Text.RegularExpressions;

async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}

var html = await Load("https://hebrewbooks.org/beis");
//var html = "<html    id=\"html_id\"   >\r\nInner text html\r\n<div   id=\"div1_id\" style=\"green\"    class=\"aa bb cc\">\r\n<br id=\"55\"/>\r\n</div>\r\n<div id=\"div2_id\" style=\"green\" class=\"bb cc\">\r\nInner text div2\r\n</div   >\r\n<   /html>\r\n";
var cleanHtml = new Regex("[\\t\\n\\r\\v\\f]").Replace(html, "");
cleanHtml = Regex.Replace(cleanHtml, @"[ ]{2,}", " ");
var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0);


HtmlTree tree = new HtmlTree();
tree.BuildTree(htmlLines);
var s = Selector.Parse("div a.inactBG");
var res = tree.Root.FindElementsBySelector(s);

foreach (var e in res)
{
    Console.WriteLine(e);
}



