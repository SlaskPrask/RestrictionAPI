using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Newtonsoft.Json;

namespace OoTRestBack.Models
{
    public class Generator
    {
        Random rnd;

        public Result Generate(int seed, string vrs)
        {
            Result r = new Result();
            r.seed = seed;
            rnd = new Random(seed);

            vrs = GetVersion(vrs);

            r.restrictions = GetRestrictions(LoadRestrictions(vrs));

            r.goals = GetGoals(LoadGoals(vrs), r.restrictions);

            r.version = "Alpha 1.0";
            return r;
        }

        public Restriction[] GetRestrictions(ListRestrictions list)
        {
            List<Restriction> restrictions = new List<Restriction>();
            int nrRest = rnd.Next(2, 5);

            do
            {
                Restriction r = list.restrictions[rnd.Next(list.restrictions.Length)];

                if (restrictions.Contains(r))
                    continue;

                if (!RestrictionCheck(r, restrictions))
                    continue;

                restrictions.Add(r);
            } while (restrictions.Count < nrRest);

            return restrictions.ToArray();
        }

        public Goal[] GetGoals(ListGoals list, Restriction[] r)
        {
            List<Goal> goals = new List<Goal>();

            float time = 0;

            do
            {
                Goal g = list.goals[rnd.Next(list.goals.Length)];

                if (goals.Contains(g))
                    continue;

                if (!CheckAgainst(g, goals, r))
                    continue;

                time += g.time;
                goals.Add(g);

            } while (time < 30);


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

        public bool RestrictionCheck(Restriction r, List<Restriction> rl)
        {
            if (rl.Count < 1)
                return true;

            switch (r.block)
            {
                case Requirements.HARD:
                    break;
                case Requirements.EXPLOSIVE:
                    break;
                case Requirements.HAMMER:
                    break;
                case Requirements.RUPEE:
                    break;
                case Requirements.HOVER:
                    break;
                case Requirements.SWORD:
                    if (rl.Any(l => l.block == Requirements.KSWORD || l.block == Requirements.MS) || rl.Any(l => l.block == Requirements.STICK) && rl.Any(l => l.block == Requirements.NUT))
                        return false;
                    break;
                case Requirements.ZL:
                    break;
                case Requirements.BOMB:
                    break;
                case Requirements.NUT:
                    if (rl.Any(l => l.block == Requirements.SWORD || l.block == Requirements.KSWORD) && rl.Any(l => l.block == Requirements.STICK))
                        return false;
                    break;
                case Requirements.STICK:
                    if (rl.Any(l => l.block == Requirements.SWORD || l.block == Requirements.KSWORD) && rl.Any(l => l.block == Requirements.NUT))
                        return false;
                    break;
                case Requirements.KSWORD:
                    if (rl.Any(l => l.block == Requirements.SWORD) || rl.Any(l => l.block == Requirements.STICK) && rl.Any(l => l.block == Requirements.NUT))
                        return false;
                    break;
                case Requirements.HSHIELD:
                    break;
                case Requirements.DSHIELD:
                    break;
                default:
                    break;
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

            //Check restrictions
            int block = 0;   
            for (int i = 0; i < g.requirements.Count; i++)
            {
                for (int j = 0; j < r.Length; j++)
                {
                    if (g.requirements[i].Contains(r[j].block))
                    {
                        block++;
                        break;
                    }
                }
            }
            if (block >= g.requirements.Count)
                return false;

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


    }


}