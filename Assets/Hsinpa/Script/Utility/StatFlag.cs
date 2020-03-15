using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatFlag
{
    public enum Facility {
         University , Hospital, None
    }

    public class BaseModifier {
        public const int baseInfectNumPerPerson = 4;
    }

    public class Other {
        public const string VillageHealthSprite = "DeathMeter";
        public const string VillageHealthSpriteDead = "DeathMeterEnd";

    }
}
