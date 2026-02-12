using UnityEngine;
using System;

[CreateAssetMenu(fileName = "IntentData", menuName = "Scriptable Objects/IntentData")]
public class StateIntentData : ScriptableObject
{
    public override string ToString()
    {
        return name;
    }
}