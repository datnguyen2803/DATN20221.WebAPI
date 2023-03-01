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
    public class StationController : ApiController
    {
        [HttpGet]
        public IHttpActionResult All()
        {
            StationTable retStation = new StationTable();

            using (var myEntity = new DATNDBEntities())
            {
                retStation = myEntity.StationTables.Include("Id")
                    .Select(station => new StationTable()
                    {
                        Id = station.Id,
                        Name = station.Name,
                        Address = station.Address
                    }).FirstOrDefault<StationTable>();
            }
            if (retStation == null)
            {
                return NotFound();
            }

            return Ok(retStation);
        }

        [HttpGet]
        public IHttpActionResult Name(string StationCode) 
        {
            StationTable retStation = new StationTable();

            using (var myEntity = new DATNDBEntities())
            {
                retStation = myEntity.StationTables.Include("Id")
                    .Where(station => station.Name == StationCode)
                    .Select(station => new StationTable()
                    {
                        Id = station.Id,
                        Name = station.Name,
                        Address = station.Address
                    }).FirstOrDefault<StationTable>();
            }
            if (retStation == null)
            {
                return NotFound();
            }

            return Ok(retStation);
        }

        [HttpPost]
        public IHttpActionResult New([FromBody] StationTable newStation)
        {
            StationTable retStation = new StationTable();

            using (var myEntity = new DATNDBEntities())
            {
                retStation = myEntity.StationTables.Include("Id")
                    .Where(station => station.Name == newStation.Name)
                    .Select(station => new StationTable()
                    {
                        Id = station.Id,
                        Name = station.Name,
                        Address = station.Address
                    }).FirstOrDefault<StationTable>();

                // able to add station
                if (retStation == null)
                {
                    myEntity.StationTables.Add(newStation);
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
        public IHttpActionResult Edit([FromBody] StationTable checkStation)
        {
            using (var myEntity = new DATNDBEntities())
            {
                var oldStation = myEntity.StationTables
                    .Where(station => station.Name == checkStation.Name)
                    .FirstOrDefault<StationTable>();

                if (oldStation != null)
                {
                    oldStation.Address = checkStation.Address;
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
        public IHttpActionResult Delete(StationTable deleteStation)
        {
            using (var myEntity = new DATNDBEntities())
            {
                var oldStation = myEntity.StationTables
                    .Where(station => station.Name == deleteStation.Name)
                    .FirstOrDefault<StationTable>();

                if (oldStation != null)
                {
                    myEntity.Entry(oldStation).State = System.Data.Entity.EntityState.Deleted;
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
