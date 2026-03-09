using UnityEngine;

[CreateAssetMenu(fileName = "FurnitureObject", menuName = "Scriptable Objects/FurnitureObject")]
public class FurnitureObject : ScriptableObject
{
    public Vector3 FurniturePosition;
    public FurnitureType.type furnitureType;
}
