using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DataAPI.Controller
{
    public class Pump
    {
        public int Id { get; set; }
        public string Area { get; set; }

    }
    public class HelloController : ApiController
    {
        List<string> pumpList = new List<string> ();

        /**
         * GET: /api/hello
         */
        public List<string> Get()
        {
            return pumpList;
        }

        /**
         * GET: /api/hello/id
         */
        public string Get(int id)
        {
            string retPump = pumpList[id];
            return retPump;
        }

        public string Get(int id, string area) 
        {
            string ret = "pump " + id.ToString() + " from " + area;
            return ret;
        }

        /**
         * POST: /api/hello
         */
        public void Post([FromBody] Pump newPump) 
        {
            string strNewPump = "pump " + newPump.Id.ToString() + " from " + newPump.Area;
            Console.WriteLine(pumpList);
            pumpList.Add(strNewPump);
            Console.WriteLine(pumpList);
            Console.WriteLine(newPump.Id.ToString());
        }

        public void Put(int id, [FromBody] string value) 
        {

        }

        public void Delete(int id) 
        {

        }


    }
}
