using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;

namespace DataAPI.Models
{
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
                return Guid.Empty;
            }
            else
            {
                return retStation.Id;
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
