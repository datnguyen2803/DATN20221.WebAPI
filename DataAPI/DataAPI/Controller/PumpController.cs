using DataAPI.Common;
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
        [HttpGet]
        [ActionName("GetAll")]
        public IHttpActionResult All()
        {
            IList<PumpModel> pumpList = null;

            using (var myEntity = new DATNDBEntities())
            {
                pumpList = myEntity.PumpTables.Include("Id")
                            .Select(pump => new PumpTable()
                            {
                                Id = pump.Id,
                                StationId = pump.StationId,
                                Position = pump.Position,
                                State = pump.State,
                            }.ToPumpModel()).ToList<PumpModel>();
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
        public IHttpActionResult GetByStationName(string StationName)
        {
            int stationId = PumpModel.RetrieveStationId(StationName);

            if(stationId == 0)
            {
                return Ok(new ResponseModel
                {
                    Code = ConstantHelper.APIResponseCode.CODE_RESOURCE_NOT_FOUND,
                    Message = ConstantHelper.APIResponseMessage.MESSAGE_STATION_NOT_FOUND,
                    Data = null
                });
            }
            else
            {
                IList<PumpModel> pumpList = null;
                var myEntity = new DATNDBEntities();
                pumpList = myEntity.PumpTables.Include("Id")
                    .Where(pump => pump.StationId == stationId)
                    .Select(pump => new PumpTable()
                    {
                        Id = pump.Id,
                        StationId = pump.StationId,
                        Position = pump.Position,
                        State = pump.State,
                    }.ToPumpModel()).ToList<PumpModel>();

                if (pumpList.Count == 0)
                {
                    return Ok(new ResponseModel
                    {
                        Code = ConstantHelper.APIResponseCode.CODE_RESOURCE_NOT_FOUND,
                        Message = ConstantHelper.APIResponseMessage.MESSAGE_PUMP_EMPTY_FOUND,
                        Data = null
                    });
                }
                else
                {
                    return Ok(new ResponseModel
                    {
                        Code = ConstantHelper.APIResponseCode.CODE_SUCCESS,
                        Message = ConstantHelper.APIResponseMessage.MESSAGE_OK,
                        Data = pumpList
                    });
                }
            }
        }

        [HttpGet]
        public IHttpActionResult GetByPumpName(string StationName, string Position)
        {
            int stationId = PumpModel.RetrieveStationId(StationName);

            if (stationId == 0)
            {
                return Ok(new ResponseModel
                {
                    Code = ConstantHelper.APIResponseCode.CODE_RESOURCE_NOT_FOUND,
                    Message = ConstantHelper.APIResponseMessage.MESSAGE_STATION_NOT_FOUND,
                    Data = null
                });
            }
            else
            {
                var myEntity = new DATNDBEntities();
                PumpModel retPump = new PumpModel();

                retPump = myEntity.PumpTables.Include("Id")
                    .Where(pump => (pump.StationId == stationId && pump.Position == Position))
                    .Select(pump => new PumpTable()
                    {
                        Id = pump.Id,
                        StationId = pump.StationId,
                        Position = pump.Position,
                        State = pump.State,
                    }.ToPumpModel()).FirstOrDefault<PumpModel>();

                if (retPump == null)
                {
                    return Ok(new ResponseModel
                    {
                        Code = ConstantHelper.APIResponseCode.CODE_RESOURCE_NOT_FOUND,
                        Message = ConstantHelper.APIResponseMessage.MESSAGE_PUMP_NOT_FOUND,
                        Data = null
                    });
                }
                else
                {
                    return Ok(new ResponseModel
                    {
                        Code = ConstantHelper.APIResponseCode.CODE_SUCCESS,
                        Message = ConstantHelper.APIResponseMessage.MESSAGE_OK,
                        Data = retPump
                    });
                }
            }
        }

        [HttpPost]
        [ActionName("New")]
        public IHttpActionResult New([FromBody] PumpModel newPump)
        {
            PumpModel retPump = new PumpModel();
            int newPumpStationId = PumpModel.RetrieveStationId(newPump.StationName);

            using (var myEntity = new DATNDBEntities())
            {
                retPump = myEntity.PumpTables.Include("Id")
                    .Where(pump => (pump.StationId == newPumpStationId && pump.Position == newPump.Position))
                    .Select(pump => new PumpTable()
                    {
                        Id = pump.Id,
                        StationId = pump.StationId,
                        Position = pump.Position,
                        State = pump.State
                    }.ToPumpModel()).FirstOrDefault<PumpModel>();

                // able to add station
                if (retPump == null)
                {
                    var retPumpTable = retPump.ToPumpTable();

                    myEntity.PumpTables.Add(retPumpTable);
                    myEntity.SaveChanges();
                    return Ok(new ResponseModel
                    {
                        Code = ConstantHelper.APIResponseCode.CODE_SUCCESS,
                        Message = ConstantHelper.APIResponseMessage.MESSAGE_OK,
                        Data = null
                    });
                }
                else
                {
                    return Ok(new ResponseModel
                    {
                        Code = ConstantHelper.APIResponseCode.CODE_RESOURCE_DUPLICATE,
                        Message = ConstantHelper.APIResponseMessage.MESSAGE_PUMP_DUPLICATE,
                        Data = null
                    });
                }
            }
        }

        [HttpPut]
        [ActionName("Edit")]
        public IHttpActionResult Edit([FromBody] PumpModel checkPump)
        {
            int newPumpStationId = PumpModel.RetrieveStationId(checkPump.StationName);
            using (var myEntity = new DATNDBEntities())
            {
                var oldPump = myEntity.PumpTables
                    .Where(pump => (pump.StationId == newPumpStationId && pump.Position == checkPump.Position))
                    .FirstOrDefault<PumpTable>();

                if (oldPump != null)
                {
                    oldPump.State = checkPump.State;
                    myEntity.SaveChanges();

                    return Ok(new ResponseModel
                    {
                        Code = ConstantHelper.APIResponseCode.CODE_SUCCESS,
                        Message = ConstantHelper.APIResponseMessage.MESSAGE_OK,
                        Data = null
                    });
                }
                else
                {
                    return Ok(new ResponseModel
                    {
                        Code = ConstantHelper.APIResponseCode.CODE_RESOURCE_NOT_FOUND,
                        Message = ConstantHelper.APIResponseMessage.MESSAGE_PUMP_NOT_FOUND,
                        Data = null
                    });
                }
            }
        }

        [HttpDelete]
        public IHttpActionResult Delete(PumpModel deletePump)
        {
            int newPumpStationId = PumpModel.RetrieveStationId(deletePump.StationName);
            using (var myEntity = new DATNDBEntities())
            {
                var oldPump = myEntity.PumpTables
                    .Where(pump => (pump.StationId == newPumpStationId && pump.Position == deletePump.Position))
                    .FirstOrDefault<PumpTable>();

                if (oldPump != null)
                {
                    myEntity.Entry(oldPump).State = System.Data.Entity.EntityState.Deleted;
                    myEntity.SaveChanges();

                    return Ok(new ResponseModel
                    {
                        Code = ConstantHelper.APIResponseCode.CODE_SUCCESS,
                        Message = ConstantHelper.APIResponseMessage.MESSAGE_OK,
                        Data = null
                    });
                }
                else
                {
                    return Ok(new ResponseModel
                    {
                        Code = ConstantHelper.APIResponseCode.CODE_RESOURCE_NOT_FOUND,
                        Message = ConstantHelper.APIResponseMessage.MESSAGE_PUMP_NOT_FOUND,
                        Data = null
                    });
                }
            }
        }
    }
}
