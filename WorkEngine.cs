using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecountInterview
{
    public class WorkEngine
    {
        public string RootUrl { get; }
        public List<Task> Parsers { get; set; } = new List<Task>();


        public WorkEngine(string startUrl)
        {
            if (string.IsNullOrEmpty(startUrl))
                throw new ArgumentException(nameof(startUrl));
            
            RootUrl = startUrl;
            AppServices.Get<ILinkStore>().AddLinkForProcessing(RootUrl);
        }

        public async Task<int> Run()
        {       
            var linkStore = AppServices.Get<ILinkStore>();
            var link = linkStore.GetNextLinkToProcess();
            while(!(link is null))
            {                    
                await ProcessLink(link);
                link = linkStore.GetNextLinkToProcess();
            }

            return 0;
        }

        public async Task ProcessLink(string link)
        {   
            if (string.IsNullOrEmpty(link))
                throw new ArgumentNullException(nameof(link));

            var daqs = AppServices.Get<IDataAcquisitionService>();
            daqs.Configuration = new WebSourceConfiguration()
            {
                Url = link
            };
            var resultHtml = await daqs.ReadDataFromSource();
            var processor = AppServices.Get<IParserService>();
            await processor.ParseContent(link, resultHtml);
        }
    }
}
