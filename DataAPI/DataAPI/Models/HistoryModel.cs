using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAPI.Models
{
    public class TempHistory
    {
        public System.Guid Id { get; set; }
        public System.Guid PumpId { get; set; }
        public System.DateTime Date { get; set; }
        public System.TimeSpan Time { get; set; }
        public int State { get; set; }

        public TempHistory() 
        {
            Id = Guid.Empty;
            PumpId = Guid.Empty;
            Date = DateTime.MinValue;
            Time = TimeSpan.MinValue;
            State = 0;
        }

        public HistoryModel ToHistoryModel()
        {
            return new HistoryModel
            {
                StationName = PumpModel.RetrieveStationNameByPumpId(PumpId),
                PumpPosition = PumpModel.RetrievePumpPositionByPumpId(PumpId),
                Date = Date,
                Time = Time,
                State = State
            };
        }
    }

    public class HistoryModel
    {
        public string StationName { get; set; }
        public string PumpPosition { get; set; }
        public System.DateTime Date { get; set; }
        public System.TimeSpan Time { get; set; }
        public int State { get; set; }

        public HistoryModel()
        {
            StationName = string.Empty;
            PumpPosition = string.Empty;
            Date = DateTime.MinValue;
            Time = TimeSpan.MinValue;
            State = 0;

        }

    }
}
