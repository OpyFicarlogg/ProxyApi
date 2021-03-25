using ProxyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyApi.Dao.Interfaces
{
    public interface IWsService
    {
        public Service GetService(string requestUrl);
    }
}
