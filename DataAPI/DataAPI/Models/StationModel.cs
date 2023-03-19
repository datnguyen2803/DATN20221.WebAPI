using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;

namespace DataAPI.Models
{
    public class TempStation
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public TempStation()
        {
            Id = Guid.Empty;
            Name = string.Empty;
            Address = string.Empty;
        }

    }

    public class StationModel
    {
        public string Name { get; set; }
        public string Address { get; set; }

        public StationModel() 
        {
            Name = string.Empty;
            Address = string.Empty;
        }

        public StationTable ToStationTable() 
        {
            return new StationTable
            {
                Id = Guid.NewGuid(),
                Name = Name,
                Address = Address
            };
        }
    }

}
