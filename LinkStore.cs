using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RecountInterview
{
    public interface ILinkStore
    {
        void AddLinkForProcessing(string url);
        string GetNextLinkToProcess();
    } 
    
    public class BasicLinkStore : ILinkStore
    {
        public ConcurrentQueue<string> Links { get; }
        public List<string> ProcessUrls { get;  }

        public BasicLinkStore()
        {
            Links = new ConcurrentQueue<string>();
            ProcessUrls = new List<string>();
        }

        public void AddLinkForProcessing(string url)
        {
            if (!ProcessUrls.Contains(url))
                Links.Enqueue(url);
        }

        public string GetNextLinkToProcess()
        {
            var found = Links.TryDequeue(out string item);
            ProcessUrls.Add(item);
            return !found ? null : item;
        }
    }
}
