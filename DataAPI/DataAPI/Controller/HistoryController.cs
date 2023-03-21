using DataAPI.Common;
using DataAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DataAPI.Controller
{
    public class HistoryController : ApiController
    {
        [HttpGet]
        [ActionName("GetAll")]
        public IHttpActionResult GetAll()
        {
            List<HistoryModel> historyList = new List<HistoryModel>();
            List<TempHistory> tempHistoryList = new List<TempHistory>();

            using (var myEntity = new DATN2022DBEntities())
            {
                tempHistoryList = myEntity.HistoryTables.Include("Id")
                            .Select(history => new TempHistory()
                            {
                                Id = history.Id,
                                PumpId = history.PumpId,
                                Date = history.Date,
                                Time = history.Time,
                                State = history.State,
                            }).ToList();
            }

            if (tempHistoryList == null)
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
                foreach (TempHistory tH in tempHistoryList)
                {
                    historyList.Add(tH.ToHistoryModel());
                }

                return Ok(new ResponseModel
                {
                    Code = ConstantHelper.APIResponseCode.CODE_SUCCESS,
                    Message = ConstantHelper.APIResponseMessage.MESSAGE_OK,
                    Data = historyList
                });
            }
        }

        [HttpGet]
        public IHttpActionResult GetByState(string StationName, string PumpPosition, int Year, int Month, int Day, int State)
        {
            DateTime myDate = new DateTime(Year, Month, Day);
            string myDateString = myDate.ToString("dd/MM/yyyy");
            Guid myPumpId = PumpModel.RetrievePumpId(StationName, PumpPosition);

            List<HistoryModel> historyList = new List<HistoryModel>();
            List<TempHistory> tempHistoryList = new List<TempHistory>();

            using (var myEntity = new DATN2022DBEntities())
            {
                tempHistoryList = myEntity.HistoryTables.Include("Id")
                            .Where(history => history.PumpId == myPumpId && history.State == State)
                            .Select(history => new TempHistory()
                            {
                                Id = history.Id,
                                PumpId = history.PumpId,
                                Date = history.Date,
                                Time = history.Time,
                                State = history.State,
                            }).ToList();
            }

            if (tempHistoryList == null)
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
                foreach (TempHistory tH in tempHistoryList)
                {
                    if (tH.Date.ToString("dd/MM/yyyy") == myDateString)
                    {
                        historyList.Add(tH.ToHistoryModel());
                    }
                }

                return Ok(new ResponseModel
                {
                    Code = ConstantHelper.APIResponseCode.CODE_SUCCESS,
                    Message = ConstantHelper.APIResponseMessage.MESSAGE_OK,
                    Data = historyList.Count
                });
            }
        }

        [HttpPost]
        [ActionName("NewRecord")]
        public IHttpActionResult NewRecord([FromBody] HistoryModel newHistory)
        {
            TempHistory tempHistory = new TempHistory();
            Guid myPumpId = PumpModel.RetrievePumpId(newHistory.StationName, newHistory.PumpPosition);

            using (var myEntity = new DATN2022DBEntities())
            {
                DateTime dateTime = DateTime.Now;
                TimeSpan timeSpan = DateTime.Now.TimeOfDay;
                HistoryTable historyTable = new HistoryTable
                {
                    Id = Guid.NewGuid(),
                    PumpId = myPumpId,
                    Date = dateTime,
                    Time = timeSpan,
                    State = newHistory.State

                };

                myEntity.HistoryTables.Add(historyTable);
                myEntity.SaveChanges();
                return Ok(new ResponseModel
                {
                    Code = ConstantHelper.APIResponseCode.CODE_SUCCESS,
                    Message = ConstantHelper.APIResponseMessage.MESSAGE_OK,
                    Data = ""
                });
            }
        }


    }
}
