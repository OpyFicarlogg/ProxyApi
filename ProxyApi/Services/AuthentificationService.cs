using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyApi.Services
{
    public class AuthentificationService
    {

        //Surement à sortir dans une autre class 
        public Boolean Authentification(String test)
        {
            test = "ok";

            return test.Equals("ok");
        }
    }
}
