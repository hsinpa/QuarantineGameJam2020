using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DiseaseSO : ScriptableObject
{
    public string _id;
    public string _name;
    public string _description;

    public float infectRate;
    public float deathRate;
    public float panicRate;

    public float errorRange;
}
