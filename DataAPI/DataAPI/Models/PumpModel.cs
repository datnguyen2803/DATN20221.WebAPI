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
        public int Id { get; set; }
        public string Area { get; set; }
        public string Code { get; set; }
        public int State { get; set; }
    }
}
