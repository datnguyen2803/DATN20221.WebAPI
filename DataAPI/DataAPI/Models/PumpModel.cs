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
            var myEntity = new DATNDBEntities();
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
            var myEntity = new DATNDBEntities();
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
