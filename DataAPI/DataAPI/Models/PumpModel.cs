﻿using System;
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
        public int StationId { get; set; }
        public string Position { get; set; }
        public int State { get; set; }
    }
}
