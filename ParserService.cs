using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace RecountInterview
{
    public interface IParserService
    {
        Task<bool> ParseContent(string fromUrl, string data);
    }

    public class WebParser : IParserService
    {
        public async Task<bool> ParseContent(string fromUrl, string data)
        {
            var htmlDoc = new HtmlDocument();   
            try {
                await Task.Run( () => htmlDoc.LoadHtml(data) );
            } catch(Exception ex) {
                Console.Error.WriteLine(ex.Message);
                return false;
            }

            PrintPhoneNumbersOnPage(data, fromUrl);

            var nodes = htmlDoc.DocumentNode.SelectNodes("//a[@href]");
            if (nodes != null)
            {
                nodes.Select( x => {
                    var v = x.Attributes["href"].Value;                
                    if (v != null && !v.StartsWith("http")) 
                    {
                        var url = new Uri(new Uri(fromUrl), v);
                        v = url.ToString();
                    }
                    return v;
                })
                .ToList()
                .ForEach( url => AppServices.Get<ILinkStore>().AddLinkForProcessing( url ) );
            }

            return true;
        }

        protected void PrintPhoneNumbersOnPage(string html, string fromPage)
        {
            var conf = AppServices.Get<CliArgs>();
            var rgx = new Regex(conf.PhoneNumberRegex);

            var output = AppServices.Get<IOutputChannel>();

            MatchCollection matches = rgx.Matches(html);
            foreach(Match m in matches)
            {               
                output.WritePhoneNumber(m.Groups[0].Value, fromPage);
            }
        }
    }


}
