using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Services.Description;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt;
using System.Web.UI;
using System.Runtime.Remoting.Messaging;

namespace DataAPI.Controller
{
    //public class Pump
    //{
    //    public int Id { get; set; }
    //    public string Area { get; set; }

    //    public Pump(int _id, string _area) { Id = _id; Area = _area; }

    //}



    //public class HelloController : ApiController
    //{
    //    static List<Pump> pumpList = new List<Pump> {
    //        new Pump (0, "my room"), 
    //        new Pump (1, "bath room")
    //    };


    //    /**
    //     * GET: /api/hello
    //     */
    //    public List<Pump> Get()
    //    {
    //        return pumpList;
    //    }

    //    /**
    //     * GET: /api/hello/id
    //     */
    //    public IHttpActionResult Get(int id)
    //    {
    //        Pump retPump = pumpList[id];

    //        if(retPump == null)
    //        {
    //            return NotFound();
    //        }
    //        else
    //        {
    //            return Ok("datchaos");
    //        }
    //    }

    //    public string Get(int id, string area) 
    //    {
    //        return "123";
    //    }

    //    /**
    //     * POST: /api/hello
    //     */
    //    public List<Pump> Post([FromBody] Pump newPump) 
    //    {
    //        pumpList.Add(newPump);

    //        return pumpList;
    //    }

    //    public List<Pump> Put([FromBody] Pump newPump) 
    //    {
    //        foreach (Pump tempPump in pumpList)
    //        {
    //            if(newPump.Id == tempPump.Id)
    //            {
    //                tempPump.Area = newPump.Area;
    //                break;
    //            }
    //        }

    //        return pumpList;
    //    }

    //    public List<Pump> Delete([FromBody] Pump newPump) 
    //    {
    //        foreach (Pump tempPump in pumpList)
    //        {
    //            if (newPump.Id == tempPump.Id)
    //            {
    //                pumpList.Remove(tempPump);
    //                break;
    //            }
    //        }

    //        return pumpList;
    //    }


    //}

}
