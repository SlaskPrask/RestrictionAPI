using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OoTRestBack.Models
{
    public class Goal
    {
        public string name { get; set; }
        public AreaWeights[] common { get; set; }
        public int difficulty { get; set; }
        public float time { get; set; }
        public float skill { get; set; }
        public Requirement requirement { get; set; }

        public Goal()
        {
        }

        public void addRequirements(Requirement requirement)
        {
            this.requirement = requirement;
        }  
    }

    public class ListGoals
    {
        public string version { get; set; }
        public Goal[] goals { get; set; }
    }
}