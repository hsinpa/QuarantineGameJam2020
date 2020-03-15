using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SkillJsonBase
{
    [SerializeField]
    public TownSkillJson[] town_skills;

    public TownSkillJson FindSkillByName(string p_name) {

        if (town_skills != null) {
            foreach (TownSkillJson s in town_skills) {
                if (s.name == p_name)
                    return s;
            }
        }

        return default(TownSkillJson);
    }
}
