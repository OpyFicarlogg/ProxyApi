using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testApi.Dao.Interfaces
{
    public interface IWsService
    {
        public String GetWs(string requestUrl);
    }
}
