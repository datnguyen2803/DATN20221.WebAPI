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
            List<PumpModel> pumpList = new List<PumpModel>();
            List<TempPump> tempPumpList = new List<TempPump>();

            using (var myEntity = new DATNDBEntities())
                        {
                tempPumpList = myEntity.PumpTables.Include("Id")
                            .Select(pump => new TempPump()
                            {
                                Id = pump.Id,
                                StationId = pump.StationId,
                                Position = pump.Position,
                                State = pump.State,
                            }).ToList();
            }
            
            if(tempPumpList == null)
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
                foreach (TempPump tP in tempPumpList)
                {
                    pumpList.Add(tP.ToPumpModel());
                }

                return Ok(new ResponseModel
                {
                    Code = ConstantHelper.APIResponseCode.CODE_SUCCESS,
                    Message = ConstantHelper.APIResponseMessage.MESSAGE_OK,
                    Data = pumpList
                });
            }
        }

        [HttpGet]
        public IHttpActionResult GetByStationName(string StationName)
        {
            Guid stationId = PumpModel.RetrieveStationId(StationName);

            if(stationId == Guid.Empty)
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
                List<PumpModel> pumpList = new List<PumpModel>();
                List<TempPump> tempPumpList = new List<TempPump>();

                var myEntity = new DATNDBEntities();
                tempPumpList = myEntity.PumpTables.Include("Id")
                    .Where(pump => pump.StationId == stationId)
                    .Select(pump => new TempPump()
                    {
                        Id = pump.Id,
                        StationId = pump.StationId,
                        Position = pump.Position,
                        State = pump.State,
                    }).ToList<TempPump>();

                foreach (TempPump tP in tempPumpList)
                {
                    pumpList.Add(tP.ToPumpModel());
                }

                if (pumpList == null)
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
            Guid stationId = PumpModel.RetrieveStationId(StationName);

            if (stationId == Guid.Empty)
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
                TempPump tempPump = new TempPump();

                tempPump = myEntity.PumpTables.Include("Id")
                    .Where(pump => (pump.StationId == stationId && pump.Position == Position))
                    .Select(pump => new TempPump()
                    {
                        Id = pump.Id,
                        StationId = pump.StationId,
                        Position = pump.Position,
                        State = pump.State,
                    }).FirstOrDefault();

                if (tempPump == null)
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
                        Data = tempPump.ToPumpModel()
                    });
                }
            }
        }

        [HttpGet]
        public IHttpActionResult GetByPumpState(string StationName, int State)
        {
            Guid stationId = PumpModel.RetrieveStationId(StationName);

            if (stationId == Guid.Empty)
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
                List<PumpModel> pumpList = new List<PumpModel>();
                List<TempPump> tempPumpList = new List<TempPump>();

                var myEntity = new DATNDBEntities();
                tempPumpList = myEntity.PumpTables.Include("Id")
                    .Where(pump => pump.StationId == stationId)
                    .Select(pump => new TempPump()
                    {
                        Id = pump.Id,
                        StationId = pump.StationId,
                        Position = pump.Position,
                        State = pump.State,
                    }).ToList<TempPump>();

                foreach (TempPump tP in tempPumpList)
                {
                    if(tP.State == State)
                    {
                        pumpList.Add(tP.ToPumpModel());
                    }
                }

                if (pumpList == null)
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

        [HttpPost]
        [ActionName("New")]
        public IHttpActionResult New([FromBody] PumpModel newPump)
        {
            TempPump tempPump = new TempPump();
            Guid newPumpStationId = PumpModel.RetrieveStationId(newPump.StationName);

            using (var myEntity = new DATNDBEntities())
            {
                tempPump = myEntity.PumpTables.Include("Id")
                    .Where(pump => (pump.StationId == newPumpStationId && pump.Position == newPump.Position))
                    .Select(pump => new TempPump()
                    {
                        Id = pump.Id,
                        StationId = pump.StationId,
                        Position = pump.Position,
                        State = pump.State
                    }).FirstOrDefault();

                // able to add station
                if (tempPump == null)
                {
                    var retPumpTable = newPump.ToPumpTable();

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
            Guid newPumpStationId = PumpModel.RetrieveStationId(checkPump.StationName);
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
            Guid newPumpStationId = PumpModel.RetrieveStationId(deletePump.StationName);
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
