using Microsoft.Extensions.DependencyInjection;
using System;

namespace RecountInterview
{
    public interface IOutputChannel
    {
        void WritePhoneNumber(string number, string source);
    }

    public class StdOutChannel : IOutputChannel
    {
        public void WritePhoneNumber(string number, string source)
        {
            var opts = AppServices.Get<CliArgs>();
            if (opts.IncludeSource)
            {
                Console.WriteLine($"Phone: {number}  /  Source URL: {source}");
            }
            else
            {
                Console.WriteLine($"Phone: {number}");
            }
        }
    }
}
