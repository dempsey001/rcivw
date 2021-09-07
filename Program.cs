using Microsoft.Extensions.DependencyInjection;
using System;
using CommandLine;
using System.Threading.Tasks;

namespace RecountInterview
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var parser = new CommandLine.Parser(c => 
            {
                c.CaseSensitive = false;
                c.HelpWriter = Console.Error;
            });

            var opts = default(CliArgs);
            parser.ParseArguments<CliArgs>(args).WithParsed( c => opts = c );

            if (opts is null)
            {
                Console.Error.WriteLine("Unable to parse command line args. Quitting...");
                return -1;
            }

            SetupServiceCollection(opts);
            var engine = new WorkEngine(opts.Url);
            await engine.Run();

            return 0;
        }


        static void SetupServiceCollection(CliArgs opts)
        {
            var sp = new ServiceCollection();

            sp.AddTransient<IDataAcquisitionService, WebDataAcquisition>();
            sp.AddTransient<IOutputChannel, StdOutChannel>();
            sp.AddSingleton<ILinkStore, BasicLinkStore>();
            sp.AddSingleton<IParserService, WebParser>();
            sp.AddSingleton<CliArgs>(opts);

            AppServices.Provider = sp.BuildServiceProvider();
        }
    }
}
