using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechTree
{
    public List<Tech> allTechs;
    public const int numOfTechsBeforeCure = 5;
    public TechTree()
    {
        allTechs = new List<Tech>();
    }

    public void addTech(Tech t)
    {
        if (!allTechs.Contains(t))
            allTechs.Add(t);
    }

    public Tech getById(int id)
    {
        return allTechs.Find(t => t.id == id);
    }

    public bool isTechAvailable(Tech t)
    {
        if (t.isCure)
            return isCureAvailable();
        else
        {
            if (t.dependancy == null || t.dependancy.Count < 1)
                return true;

            foreach (var id in t.dependancy)
            {
                var tech = getById(id);
                if (tech == null || !tech.isComplete)
                    return false;
            }

        }
        return false;
    }

    public bool isCureAvailable()
    {
        int count = 0;
        foreach (var t in allTechs)
        {
            if (t.isComplete)
                count++;
            if (count >= numOfTechsBeforeCure)
                return true;
        }
        return false;
    }
}
