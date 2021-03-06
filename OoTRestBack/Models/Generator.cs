﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.IO;
using Newtonsoft.Json;

namespace OoTRestBack.Models
{
    public class Generator
    {
        Random rnd;
        Hashtable req;

        public Result Generate(int seed, string vrs, bool hard)
        {
            Result r = new Result();
            r.seed = seed;
            rnd = new Random(seed);

            vrs = GetVersion(vrs);


            req = LoadRequirement(vrs);

            r.restrictions = GetRestrictions(LoadRestrictions(vrs), hard);
            r.goals = GetGoals(LoadGoals(vrs), r.restrictions);

            r.version = "Alpha 1.0";
            return r;
        }

        public Restriction[] GetRestrictions(ListRestrictions list, bool hard)
        {
            List<Restriction> restrictions = new List<Restriction>();

            int nrRest = rnd.Next(2, 5);

            if (!hard)
            {
                Restriction h = new Restriction();
                h.block = new Requirement("HARD");
                restrictions.Add(h);
                nrRest += 1;
            }

            do
            {
                Restriction r = list.restrictions[rnd.Next(list.restrictions.Count)];

                if (!RestrictionCheck(r, restrictions))
                {
                    list.restrictions.Remove(r);
                    continue;
                }

                restrictions.Add(r);
                list.restrictions.Remove(r);

            } while (restrictions.Count < nrRest && list.restrictions.Count > 0);

            if (!hard)
            {
                restrictions.RemoveAt(0);
            }
            return restrictions.ToArray();
        }

        public Goal[] GetGoals(ListGoals list, Restriction[] r)
        {
            List<Goal> goals = new List<Goal>();

            float time = 0;

            do
            {
                Goal g = list.goals[rnd.Next(list.goals.Count)];

                if (!CheckAgainst(g, goals, r))
                {
                    list.goals.Remove(g);
                    continue;
                }


                time += g.time;
                g.requirement.subRequirements = null;
                goals.Add(g);
                list.goals.Remove(g);

            } while (time < 30 && list.goals.Count > 0);


            return goals.ToArray();
        }


        public ListGoals LoadGoals(string v)
        {
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\Version\\" + v + "\\GoalList.json"))
            {
                return JsonConvert.DeserializeObject<ListGoals>(r.ReadToEnd());
            }
        }
        public ListRestrictions LoadRestrictions(string v)
        {
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\Version\\" + v + "\\RestrictionList.json"))
            {
                return JsonConvert.DeserializeObject<ListRestrictions>(r.ReadToEnd());
            }
        }
        public Hashtable LoadRequirement(string v)
        {
            using (StreamReader r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\Version\\" + v + "\\RequirementList.json"))
            {

                RequirementJSON[] test = JsonConvert.DeserializeObject<RequirementJSON[]>(r.ReadToEnd());
                Hashtable hash = new Hashtable();

                foreach (RequirementJSON reqdata in test)
                {
                    Requirement requirement;
                    if (hash.ContainsKey(reqdata.name))
                    {
                        requirement = (Requirement)hash[reqdata.name];
                    }
                    else
                    {
                        requirement = new Requirement(reqdata.name);
                        hash.Add(reqdata.name, requirement);
                    }

                    foreach (string[] subs in reqdata.subs)
                    {
                        List<Requirement> subGroup = new List<Requirement>();
                        foreach (string sub in subs)
                        {
                            Requirement subRequirement;
                            if (hash.ContainsKey(sub))
                            {
                                subRequirement = (Requirement)hash[sub];
                            }
                            else
                            {
                                subRequirement = new Requirement(sub);
                                hash.Add(sub, subRequirement);
                            }
                            subGroup.Add(subRequirement);
                        }

                        requirement.requires(subGroup.ToArray());
                    }
                }

                foreach (DictionaryEntry de in hash)
                {
                        if (!test.Any(rl => rl.name == (string)de.Key))
                            System.Diagnostics.Debugger.Log(1, "sub", "Subrequirement " + de.Key + " not added" + '\n');        
                }

                return hash;
            }
        }

        public bool RestrictionCheck(Restriction r, List<Restriction> rl)
        {
            if (rl.Count > 0)
            {
                Hashtable cache = new Hashtable();
                foreach (Restriction randomRestriction in rl)
                {
                    if (!((Requirement)req["Escape"]).checkRestriction(randomRestriction, cache, null))
                    {
                        Log("Escape impossible");
                        return false;
                    }

                    Hashtable newCache = new Hashtable();
                    foreach (DictionaryEntry de in cache)
                    {
                        if ((bool)de.Value == false)
                            newCache.Add(de.Key, false);
                    }
                    cache = newCache;
                }                
            }
            return true;
        }

        public bool CheckAgainst(Goal g, List<Goal> l, Restriction[] r)
        {
            if (l.Count > 0 && g.common != null && g.common.Length > 0) //Check for synnergy
            {
                for (int i = 0; i < g.common.Length; i++)
                {
                    int syn = g.common[i].weight;

                    foreach (Goal _g in l)
                    {
                        if (_g.common != null & _g.common.Length > 0)
                        {
                            for (int j = 0; j < _g.common.Length; j++)
                            {
                                if (g.common[i].commonGoal == _g.common[j].commonGoal)
                                {
                                    syn += _g.common[j].weight;
                                    if (syn > 9)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Log(g.name);
            //Check restrictions

            g.requirement = (Requirement)req[g.requirement.name];

            Hashtable cache = new Hashtable();
            foreach (Restriction randomRestriction in r)
            {
                Log("Checking " + g.name + " against " + randomRestriction.name);
                if (!g.requirement.checkRestriction(randomRestriction, cache, null))
                {
                    Log("Removed goal " + g.name);
                    return false;
                }


                Hashtable newCache = new Hashtable();
                foreach (DictionaryEntry de in cache)
                {
                    if ((bool)de.Value == false)
                        newCache.Add(de.Key, false);
                }
                cache = newCache;
            }

            return true;
        }

        string GetVersion(string v)
        {
            switch (v)
            {
                case "A1.0":
                    return "Alpha 1.0";
                default:
                    return "Alpha 1.0";
            }
        }

        void Log(string message)
        {
            System.Diagnostics.Debugger.Log(1, "DebugLog", message + '\n');
        }
    }

    

}