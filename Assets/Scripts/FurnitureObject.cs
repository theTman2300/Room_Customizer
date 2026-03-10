using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FurnitureObject", menuName = "Scriptable Objects/FurnitureObject")]
[Serializable]
public class FurnitureObject : ScriptableObject
{
    public float[] FurniturePosition;
    public FurnitureType.type furnitureType;
}
