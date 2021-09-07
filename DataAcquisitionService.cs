using System;
using System.Net;
using System.Threading.Tasks;
using CommandLine;

namespace RecountInterview
{
    public interface ISourceConfiguration
    {
        bool IsValidForService(IDataAcquisitionService srvObj);
    }

    public interface IDataAcquisitionService 
    {
        ISourceConfiguration Configuration { get; set; }
        Task<string> ReadDataFromSource();
    }

    public class WebSourceConfiguration : ISourceConfiguration
    {
        public string Url { get; set; }
        public bool IsValidForService(IDataAcquisitionService srvObj)
        {
            if (srvObj is null) 
                throw new ArgumentNullException(nameof(srvObj));
            return null != (srvObj as WebDataAcquisition);
        }
    }

    public class WebDataAcquisition : IDataAcquisitionService
    {
        public ISourceConfiguration Configuration { get; set; }

        
        public async Task<string> ReadDataFromSource()
        {
            var webConf = Configuration as WebSourceConfiguration;
            using var wc = new WebClient();
            return await wc.DownloadStringTaskAsync(new Uri(webConf.Url));
        }
    }
}
