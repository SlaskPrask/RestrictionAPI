using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OoTRestBack.Models
{
    public class RequirementJSON
    {
        public string name { get; set; }
        public List<string[]> subs { get; set; }
    }

    public class Requirement
    {
        public string name;
        //public List<JSONSubRequirement[]> jsonSub {get; set;}
        public List<Requirement[]> subRequirements;


        public Requirement(string name)
        {
            this.name = name;
        }

        //add sub requirements
        public void requires(Requirement[] group)
        {
            if (subRequirements == null)
            {
                 subRequirements = new List<Requirement[]>();
            }
            subRequirements.Add(group);
        }


        //check against a each group
        private bool checkRequirementGroup(Requirement[] group, Restriction restriction, Hashtable alreadyChecked, List<Requirement> loopCheck)
        {
            //check that everything in this group is allowed recursively (including their requirements)
            foreach (Requirement requirement in group)
            {
                if (!requirement.checkRestriction(restriction, alreadyChecked, loopCheck))
                {
                    //if the sub requirement is banned, this requirement is banned
                    return false;
                }
            }

            //no checks said they're banned, so this is allowed
            return true;
        }

        //check against a restriction
        public bool checkRestriction(Restriction restriction, Hashtable alreadyChecked, List<Requirement> loopCheck)
        {
            //begin loop checking here or continue previous
            if (loopCheck == null)
            {
                loopCheck = new List<Requirement>();
            }
            else
            {
                if (loopCheck.Contains(this))
                {
                    return false; //infinite loop, can't complete because it needs itself
                }
                else
                {
                    loopCheck = new List<Requirement>(loopCheck); //clone list so groups don't mix into each other
                }
            }
            loopCheck.Add(this);

            if (alreadyChecked.Contains(this))
            {
                return (bool)alreadyChecked[this];
            }

            //check if this requirement is banned in restriction
            if (restriction.GetRequirement() == this)
            {
                alreadyChecked.Add(this, false);
                return false;
            }
            //this isn't banned

            //check what this needs
            if (subRequirements != null)
            {
                foreach (Requirement[] group in subRequirements)
                {
                    if (checkRequirementGroup(group, restriction, alreadyChecked, loopCheck))
                    {
                        //a group is allowed, means this requirement isn't banned
                        alreadyChecked.Add(this, true);
                        return true;
                    }
                }
            }

            //no group is allowed, this requirement is banned
            alreadyChecked.Add(this, false);
            return false;
        }
    }
}