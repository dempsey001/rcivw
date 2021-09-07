using System;
using Microsoft.Extensions.DependencyInjection;

namespace RecountInterview
{
    public static class AppServices
    {        
        private static ServiceProvider _srvProv;
        public static ServiceProvider Provider 
        {
            get => _srvProv;
            set => System.Threading.Interlocked.Exchange(ref _srvProv, value);
        }

        public static ObjectType Get<ObjectType>() where ObjectType : class => _srvProv?.GetService<ObjectType>();        
    }
}
