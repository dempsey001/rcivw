using System;
using CommandLine;

namespace RecountInterview
{
    public class CliArgs
    {        
        [Option('p', "phoneRegEx", Default = "(\\d{1,2}[-.]{1})?\\(?\\d{3}\\)?[-.]{1}\\d{3}[-.]{1}\\d{4}", HelpText = "Regular expression to use to find phone numbers.")]
        public string PhoneNumberRegex { get; set; }

        [Option('u', "url", Default = "https://therecount.github.io/interview-materials/project-a/1.html", HelpText = "Default Source URL")]
        public string Url { get; set; } 

        [Option('i', "includesrc", Default = false, HelpText = "Whether or not to include source URL in output with phone numbers.")]
        public bool IncludeSource { get; set; } 
    }
}
