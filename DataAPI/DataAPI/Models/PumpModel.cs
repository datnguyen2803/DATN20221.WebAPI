using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;

namespace DataAPI.Models
{

    public class TempPump
    {
        public System.Guid Id { get; set; }
        public System.Guid StationId { get; set; }
        public string Position { get; set; }
        public int State { get; set; }

        public TempPump()
        {
            Id = Guid.Empty;
            StationId = Guid.Empty;
            Position = string.Empty;
            State = 0;
        }

        public PumpModel ToPumpModel()
        {
            return new PumpModel
            {
                StationName = PumpModel.RetrieveStationName(StationId),
                Position = Position,
                State = State
            };
        }
    }

    public class PumpModel
    {
        public string StationName { get; set; }
        public string Position { get; set; }
        public int State { get; set; }

        public PumpModel() 
        {
            StationName = string.Empty;
            Position = string.Empty;
            State = 0;
        }

        public static Guid RetrieveStationId(string StationName)
        {
            var myEntity = new DATN2022DBEntities();
            TempStation retStation = myEntity.StationTables.Include("Id")
                                      .Where(station => station.Name == StationName)
                                      .Select(station => new TempStation()
                                      {
                                          Id = station.Id,
                                          Name = station.Name,
                                          Address = station.Address
                                      }).FirstOrDefault();

            if (retStation == null)
            {
                return Guid.Empty;
            }
            else
            {
                return retStation.Id;
            }
        }

        public static string RetrieveStationName(Guid StationId)
        {
            var myEntity = new DATN2022DBEntities();
            TempStation retStation = myEntity.StationTables.Include("Id")
                                      .Where(station => station.Id == StationId)
                                      .Select(station => new TempStation()
                                      {
                                          Id = station.Id,
                                          Name = station.Name,
                                          Address = station.Address
                                      }).FirstOrDefault();

            if (retStation == null)
            {
                return "";
            }
            else
            {
                return retStation.Name;
            }
        }
    
        public static Guid RetrievePumpId(string StationName, string PumpPosition)
        {
            Guid StationId = RetrieveStationId(StationName);
            if(StationId == Guid.Empty)
            {
                return Guid.Empty;
            }

            var myEntity = new DATN2022DBEntities();
            TempPump retPump = myEntity.PumpTables.Include("Id")
                                      .Where(pump => pump.StationId == StationId && pump.Position == PumpPosition)
                                      .Select(pump => new TempPump()
                                      {
                                          Id = pump.Id,
                                          StationId = pump.StationId,
                                          Position = pump.Position,
                                          State = pump.State
                                      }).FirstOrDefault();

            if (retPump == null)
            {
                return Guid.Empty;
            }
            else
            {
                return retPump.Id;
            }
        }

        public static string RetrievePumpPositionByPumpId(Guid PumpId)
        {
            var myEntity = new DATN2022DBEntities();
            TempPump retPump = myEntity.PumpTables.Include("Id")
                                      .Where(pump => pump.Id == PumpId)
                                      .Select(pump => new TempPump()
                                      {
                                          Id = pump.Id,
                                          StationId= pump.StationId,
                                          Position = pump.Position,
                                          State = pump.State
                                      }).FirstOrDefault();

            if (retPump == null)
            {
                return string.Empty;
            }
            else
            {
                return retPump.Position;
            }
        }

        public static string RetrieveStationNameByPumpId(Guid PumpId)
        {
            var myEntity = new DATN2022DBEntities();
            TempPump retPump = myEntity.PumpTables.Include("Id")
                                      .Where(pump => pump.Id == PumpId)
                                      .Select(pump => new TempPump()
                                      {
                                          Id = pump.Id,
                                          StationId = pump.StationId,
                                          Position = pump.Position,
                                          State = pump.State
                                      }).FirstOrDefault();

            if (retPump == null)
            {
                return string.Empty;
            }
            else
            {
                return RetrieveStationName(retPump.StationId);
            }
        }

        public PumpTable ToPumpTable()
        {
            return new PumpTable
            {
                Id = Guid.NewGuid(),
                StationId = RetrieveStationId(StationName),
                Position = Position,
                State = State,
            };
        }
    }
}
