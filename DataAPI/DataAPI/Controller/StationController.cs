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
        [ActionName("All")]
        public IHttpActionResult All()
        {
            StationModel retStation = new StationModel();

            using (var myEntity = new DATNDBEntities())
            {
                retStation = myEntity.StationTables.Include("Id")
                    .Select(station => new StationModel()
                    {
                        Id = station.Id,
                        Name = station.Name,
                        Address = station.Address
                    }).FirstOrDefault<StationModel>();
            }
            if (retStation == null)
            {
                return NotFound();
            }

            return Ok(retStation);
        }

        [HttpGet]
        [ActionName("Name")]
        public IHttpActionResult Name(string StationCode) 
        {
            StationModel retStation = new StationModel();

            using (var myEntity = new DATNDBEntities())
            {
                retStation = myEntity.StationTables.Include("Id")
                    .Where(station => station.Name == StationCode)
                    .Select(station => new StationModel()
                    {
                        Id = station.Id,
                        Name = station.Name,
                        Address = station.Address
                    }).FirstOrDefault<StationModel>();
            }
            if (retStation == null)
            {
                return NotFound();
            }

            return Ok(retStation);
        }

        [HttpPost]
        [ActionName("New")]
        public IHttpActionResult New([FromBody] StationModel newStation)
        {
            StationModel retStation = new StationModel();

            using (var myEntity = new DATNDBEntities())
            {
                retStation = myEntity.StationTables.Include("Id")
                    .Where(station => station.Name == newStation.Name)
                    .Select(station => new StationModel()
                    {
                        Id = station.Id,
                        Name = station.Name,
                        Address = station.Address
                    }).FirstOrDefault<StationModel>();

                // able to add station
                if (retStation == null)
                {
                    var newStationTable = new StationTable()
                    {
                        Id = newStation.Id,
                        Name = newStation.Name,
                        Address = newStation.Address
                    };

                    myEntity.StationTables.Add(newStationTable);
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
        public IHttpActionResult Edit([FromBody] StationModel checkStation)
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
        [ActionName("Delete")]
        public IHttpActionResult Delete(StationModel deleteStation)
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
