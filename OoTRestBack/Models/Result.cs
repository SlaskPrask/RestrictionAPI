using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OoTRestBack.Models
{
    public class Result
    {
        public int seed { get; set; }
        public string version { get; set; }
        public Goal[] goals { get; set; }
        public Restriction[] restrictions { get; set; }
    }
}