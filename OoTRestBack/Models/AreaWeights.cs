using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OoTRestBack.Models
{
    public enum Area { DEKU, DC, JABU, FOREST, FIRE, WATER, SHADOW, SPIRIT, GANON, HEALTH, STICK, NUT, QUIVER, SEED, BAG, WALLET }

    public class AreaWeights
    {
        public Area commonGoal { get; set; }
        public int weight { get; set; }
    }
}