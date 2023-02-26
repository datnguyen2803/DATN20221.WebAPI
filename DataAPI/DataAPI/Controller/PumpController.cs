using DataAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.DynamicData;
using System.Web.Http;

namespace DataAPI.Controller
{
    public class PumpController : ApiController
    {
        /**
         * GET methods
         */
        public IHttpActionResult GetPump (int _id)
        {
            PumpModel retModel = new PumpModel();

            using (var myEntity = new DBDATNEntities())
            {
                retModel = myEntity.PumpTables.Include("Id")
                    .Where(p => p.Id == _id)
                    .Select(p => new PumpModel()
                    {
                        Id = p.Id,
                        Area = p.Area,
                        Code = p.Code,
                        State = p.State
                    }).FirstOrDefault<PumpModel>();
            }

            if(retModel == null) 
            {
                return NotFound();
            }

            return Ok(retModel);
        }

        public IHttpActionResult GetAllPumps()
        {
            IList<PumpModel> pumps = null;

            using (var myEntity = new DBDATNEntities())
            {
                pumps = myEntity.PumpTables.Include("Id")
                    .Select(p => new PumpModel()
                    {
                        Id = p.Id,
                        Area = p.Area,
                        Code = p.Code,
                        State = p.State
                    }).ToList<PumpModel>();
            }

            if (pumps.Count == 0)
            {
                return NotFound();
            }

            return Ok(pumps);

        }
    
        /**
         * POST methods
         */ 
        public IHttpActionResult PostNewPump(PumpModel _pump)
        {
            using (var myEntity = new DBDATNEntities())
            {
                myEntity.PumpTables.Add(new PumpTable()
                {
                    Id = _pump.Id,
                    Area = _pump.Area,
                    Code = _pump.Code,
                    State = _pump.State
                });

                myEntity.SaveChanges();
            }

            return Ok();
        }

        /**
         * PUT methods
         */
        public IHttpActionResult PutPump(PumpModel _pump)
        {
            using (var myEntity = new DBDATNEntities())
            {
                var oldPump = myEntity.PumpTables
                    .Where(p => (p.Area == _pump.Area && p.Code == _pump.Code))
                    .FirstOrDefault<PumpTable>();

                if(oldPump != null)
                {
                    oldPump.State = _pump.State;

                    myEntity.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }

        /**
         * DELETE methods
         */
        public IHttpActionResult DeletePump(PumpModel _pump)
        {
            using (var myEntity = new DBDATNEntities())
            {
                var oldPump = myEntity.PumpTables
                    .Where(p => (p.Area == _pump.Area && p.Code == _pump.Code))
                    .FirstOrDefault<PumpTable>();

                if (oldPump != null)
                {
                    myEntity.Entry(oldPump).State = System.Data.Entity.EntityState.Deleted;

                    myEntity.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }

    }

}
