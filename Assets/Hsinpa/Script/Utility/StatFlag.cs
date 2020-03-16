using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatFlag
{
    public enum Facility {
         University , Hospital, None
    }

    public class BaseModifier {
        //Default AP
        public const int baseAP = 3;

        //Default Investigate power
        public const int baseInvestigatePower = 1;

        public const int baseInfectNumPerPerson = 4;

        public const float baseCureRate = 0.25f;

        public const float baseErrorRate = 0.05f;
    }

    public class ActionStat {
        public const string Quarantine = "Quarantine";
        public const string Lab = "Improve lab quality";
        public const string Cure = "Cure";
        public const string Investigate = "Develop Tech";
    }

    public class Other {
        public const string VillageHealthSprite = "DeathMeter";
        public const string VillageHealthSpriteDead = "DeathMeterEnd";
    }

    public class Audio {
        public const string ButtonSound = "ButtonSound";
        public const string PanelCloseOpenSound = "PanelCloseOpenSound";
        public const string Tech_Done = "Tech_Done";
    }
}
