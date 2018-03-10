using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OoTRestBack.Models
{
    public class Restriction
    {
        public string name { get; set; }
        public int difficulty { get; set; }
        public float time { get; set; }
        public float skill { get; set; }
        public Requirements block;
    }


    public class ListRestrictions
    {
        public string version { get; set; }
        public Restriction[] restrictions { get; set; }
    }
}