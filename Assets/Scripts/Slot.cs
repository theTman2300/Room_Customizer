using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Slot", menuName = "Scriptable Objects/Slot")]
[Serializable]
public class Slot : ScriptableObject
{
    public int ID;
    public string Path;
}
