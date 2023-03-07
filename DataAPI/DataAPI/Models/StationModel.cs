using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;

namespace DataAPI.Models
{
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
                Id = 0,
                Name = Name,
                Address = Address
            };
        }
    }

}
