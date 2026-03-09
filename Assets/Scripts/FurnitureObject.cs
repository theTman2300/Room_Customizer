using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FurnitureObject", menuName = "Scriptable Objects/FurnitureObject")]
[Serializable]
public class FurnitureObject : ScriptableObject
{
    public Vector3 FurniturePosition;
    public FurnitureType.type furnitureType;
}
