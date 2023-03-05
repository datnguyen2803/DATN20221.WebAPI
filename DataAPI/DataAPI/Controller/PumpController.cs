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
        [ActionName("All")]
        public IHttpActionResult All()
        {
            IList<PumpModel> pumpList = null;

            using (var myEntity = new DATNDBEntities())
            {
                pumpList = myEntity.PumpTables.Include("Id")
                            .Select(pump => new PumpModel()
                            {
                                Id = pump.Id,
                                StationId = pump.StationId,
                                Position = pump.Position,
                                State = pump.State,
                            }).ToList<PumpModel>();
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
        [ActionName("AllPump")]
        public IHttpActionResult AllPump(string StationName)
        {
            int stationId = RetrieveStationId(StationName);

            if(stationId == 0)
            {
                return NotFound();
            }
            else
            {
                IList<PumpModel> pumpList = null;
                var myEntity = new DATNDBEntities();
                pumpList = myEntity.PumpTables.Include("Id")
                    .Where(pump => pump.StationId == stationId)
                    .Select(pump => new PumpModel()
                    {
                        Id = pump.Id,
                        StationId = pump.StationId,
                        Position = pump.Position,
                        State = pump.State,
                    }).ToList<PumpModel>();

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
        [ActionName("OnePump")]
        public IHttpActionResult OnePump(string StationName, string PumpName)
        {
            int stationId = RetrieveStationId(StationName);

            if (stationId == 0)
            {
                return NotFound();
            }
            else
            {
                var myEntity = new DATNDBEntities();
                PumpModel retPump = new PumpModel();

                retPump = myEntity.PumpTables.Include("Id")
                    .Where(pump => (pump.StationId == stationId && pump.Position == PumpName))
                    .Select(pump => new PumpModel()
                    {
                        Id = pump.Id,
                        StationId = pump.StationId,
                        Position = pump.Position,
                        State = pump.State,
                    }).FirstOrDefault<PumpModel>();

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
        [ActionName("New")]
        public IHttpActionResult New([FromBody] PumpModel newPump)
        {
            PumpModel retPump = new PumpModel();

            using (var myEntity = new DATNDBEntities())
            {
                retPump = myEntity.PumpTables.Include("Id")
                    .Where(pump => (pump.StationId == newPump.StationId && pump.Position == newPump.Position))
                    .Select(pump => new PumpModel()
                    {
                        Id = pump.Id,
                        StationId = pump.StationId,
                        Position = pump.Position,
                        State = pump.State
                    }).FirstOrDefault<PumpModel>();

                var newPumpTable = new PumpTable()
                {
                    Id = newPump.Id,
                    StationId = newPump.StationId,
                    Position = newPump.Position,
                    State = newPump.State
                };

                // able to add station
                if (retPump == null)
                {
                    myEntity.PumpTables.Add(newPumpTable);
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
        [ActionName("Edit")]
        public IHttpActionResult Edit([FromBody] PumpModel checkPump)
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
        [ActionName("Delete")]
        public IHttpActionResult Delete(PumpModel deletePump)
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
