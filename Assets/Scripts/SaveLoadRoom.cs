using NaughtyAttributes;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveLoadRoom : MonoBehaviour
{
    [SerializeField] FurniturePrefabs[] prefabs;
    [SerializeField, ReadOnly] List<Furniture> furniture;

    bool isLoading = false;

    public void RemoveFurniture(Furniture oldFurniture)
    {
        if (isLoading) return;
        furniture.Remove(oldFurniture);
    }

    public void AddFurniture(Furniture newFurniture)
    {
        furniture.Add(newFurniture);
    }

    public void SaveRoom(Slot slot)
    {
        FurnitureObject[] furnitureObjects = new FurnitureObject[furniture.Count];
        for (int i = 0; i < furniture.Count; i++)
        {
            furnitureObjects[i] = furniture[i].GetFurnitureObject();
        }

        string json = JsonConvert.SerializeObject(furnitureObjects, Formatting.Indented);
        print("Room saved");
        print(json);
        File.WriteAllText(slot.Path, json);
    }

    public void LoadRoom(Slot slot)
    {
        isLoading = true;
        FurnitureObject[] furnitureObjects = JsonConvert.DeserializeObject<FurnitureObject[]>(File.ReadAllText(slot.Path));

        //create a dictionary to easily look up prefab
        //done this way because it is not possible to serializeField a dictionary
        Dictionary<FurnitureType.type, GameObject> furnitureDictionary = new();
        for (int i = 0; i < prefabs.Length; i++)
        {
            furnitureDictionary.Add(prefabs[i].type, prefabs[i].prefab);
        }

        foreach (FurnitureObject furnitureObject in furnitureObjects)
        {
            Instantiate(furnitureDictionary[FurnitureType.type.chair], furnitureObject.FurniturePosition, Quaternion.identity);
        }
        isLoading = false;
    }
}

[Serializable]
public class FurniturePrefabs
{
    public FurnitureType.type type;
    public GameObject prefab;
}
