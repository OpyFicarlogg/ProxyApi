using System;
using System.Collections.Generic;
using System.Linq;
using testApi.Dao.Interfaces;
using testApi.Extension;

namespace testApi.Services
{
    public class WsService : IWsService
    {
        private static readonly Dictionary<string, string> ws = new Dictionary<string, string>(){
            {"WeatherForecast", "http://192.168.1.55/api/WeatherForecast"},
            {"USA", "Chicago, New York, Washington"},
            {"India", "Mumbai, New Delhi, Pune"}
        };
        public String GetWs(string requestUrl)
        {
            if (!String.IsNullOrEmpty(requestUrl))
            {
                //Dois faire un appel à la database 
                //return ws.ContainsKey(RequestUrl) ? ws.GetValueOrDefault(RequestUrl) : null;
                //return ws.GetValueOrDefault(requestUrl);
                var query = ws.Where(dic => requestUrl.Contains(dic.Key));
                return query.SingleOrDefault().Value + requestUrl.RemoveValue(query.SingleOrDefault().Key);
            }
            else
            {
                return null;
                //TODO: Ajouter un log 
            }

        }
    }
}
