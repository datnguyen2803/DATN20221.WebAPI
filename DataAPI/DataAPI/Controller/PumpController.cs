using DataAPI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DataAPI.Controller
{
    public class PumpController : ApiController
    {

        private int RetrieveStationId(string StationName)
        {
            var myEntity = new DATNDBEntities();
            StationTable retStation = myEntity.StationTables.Include("Id")
                                      .Where(station => station.Name == StationName)
                                      .Select(station => new StationTable()
                                      {
                                          Id = station.Id,
                                          Name = station.Name,
                                          Address = station.Address
                                      }).FirstOrDefault<StationTable>();

            if (retStation == null)
            {
                return 0;
            }
            else
            {
                return retStation.Id;
            }
        }

        [HttpGet]
        public IHttpActionResult All()
        {
            IList<PumpTable> pumpList = null;

            using (var myEntity = new DATNDBEntities())
            {
                pumpList = myEntity.PumpTables.Include("Id")
                            .Select(pump => new PumpTable()
                            {
                                Id = pump.Id,
                                StationId = pump.StationId,
                                Position = pump.Position,
                                State = pump.State,
                            }).ToList<PumpTable>();
            }

            if(pumpList.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(pumpList);
            }
        }

        [HttpGet]
        public IHttpActionResult AllPump(string StationName)
        {
            int stationId = RetrieveStationId(StationName);

            if(stationId == 0)
            {
                return NotFound();
            }
            else
            {
                IList<PumpTable> pumpList = null;
                var myEntity = new DATNDBEntities();
                pumpList = myEntity.PumpTables.Include("Id")
                    .Where(pump => pump.StationId == stationId)
                    .Select(pump => new PumpTable()
                    {
                        Id = pump.Id,
                        StationId = pump.StationId,
                        Position = pump.Position,
                        State = pump.State,
                    }).ToList<PumpTable>();

                if (pumpList.Count == 0)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(pumpList);
                }
            }

        }

        [HttpGet]
        public IHttpActionResult OnePump(string StationName, string PumpName)
        {
            int stationId = RetrieveStationId(StationName);

            if (stationId == 0)
            {
                return NotFound();
            }
            else
            {
                IList<PumpTable> pumpList = null;
                var myEntity = new DATNDBEntities();
                PumpTable retPump = new PumpTable();

                retPump = myEntity.PumpTables.Include("Id")
                    .Where(pump => (pump.StationId == stationId && pump.Position == PumpName))
                    .Select(pump => new PumpTable()
                    {
                        Id = pump.Id,
                        StationId = pump.StationId,
                        Position = pump.Position,
                        State = pump.State,
                    }).FirstOrDefault<PumpTable>();

                if (retPump == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(retPump);
                }
            }
        }

        [HttpPost]
        public IHttpActionResult New([FromBody] PumpTable newPump)
        {
            PumpTable retPump = new PumpTable();

            using (var myEntity = new DATNDBEntities())
            {
                retPump = myEntity.PumpTables.Include("Id")
                    .Where(pump => (pump.StationId == newPump.StationId && pump.Position == newPump.Position))
                    .Select(pump => new PumpTable()
                    {
                        Id = pump.Id,
                        StationId = pump.StationId,
                        Position = pump.Position,
                        State = pump.State
                    }).FirstOrDefault<PumpTable>();

                // able to add station
                if (retPump == null)
                {
                    myEntity.PumpTables.Add(newPump);
                    myEntity.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
        }


        [HttpPut]
        public IHttpActionResult Edit([FromBody] PumpTable checkPump)
        {
            using (var myEntity = new DATNDBEntities())
            {
                var oldPump = myEntity.PumpTables
                    .Where(pump => (pump.StationId == checkPump.StationId && pump.Position == checkPump.Position))
                    .FirstOrDefault<PumpTable>();

                if (oldPump != null)
                {
                    oldPump.State = checkPump.State;
                    myEntity.SaveChanges();

                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpDelete]
        public IHttpActionResult Delete(PumpTable deletePump)
        {
            using (var myEntity = new DATNDBEntities())
            {
                var oldPump = myEntity.PumpTables
                    .Where(pump => (pump.StationId == deletePump.StationId && pump.Position == deletePump.Position))
                    .FirstOrDefault<PumpTable>();

                if (oldPump != null)
                {
                    myEntity.Entry(oldPump).State = System.Data.Entity.EntityState.Deleted;
                    myEntity.SaveChanges();

                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
        }
    }
}
