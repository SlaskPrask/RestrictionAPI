using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace OoTRestBack.Models
{
    public class Restriction
    {
        public string name { get; set; }
        public int difficulty { get; set; }
        public float time { get; set; }
        public float skill { get; set; }
        public Requirement block;

        public Requirement GetRequirement()
        {
            return block;
        }
    }


    public class ListRestrictions
    {
        public string version { get; set; }
        public Restriction[] restrictions { get; set; }
    }
}