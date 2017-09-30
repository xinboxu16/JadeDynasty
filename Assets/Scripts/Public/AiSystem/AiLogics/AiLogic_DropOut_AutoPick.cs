using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class DropOutInfo
    {
        public DropOutType DropType;
        public string Model;
        public string Particle;
        public int Value;
    }
    public enum DropOutType
    {
        GOLD = 0,
        HP = 1,
        MP = 2,
    }

    public class AiLogic_DropOut_AutoPick
    {
    }
}
