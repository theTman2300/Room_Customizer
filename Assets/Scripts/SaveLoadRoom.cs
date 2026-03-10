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

        if (!Directory.Exists(Application.persistentDataPath + "/Slots"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Slots");
        }

        string json = JsonConvert.SerializeObject(furnitureObjects, Formatting.Indented);
        print(json);
        File.WriteAllText(slot.Path, json);
        print("Room saved");
    }

    public void LoadRoom(Slot slot)
    {
        isLoading = true;
        foreach (GameObject existingFurniture in GameObject.FindGameObjectsWithTag("Furniture"))
        {
            Destroy(existingFurniture);
        }

        FurnitureObject[] furnitureObjects = JsonConvert.DeserializeObject<FurnitureObject[]>(File.ReadAllText(slot.Path));

        //create a dictionary to easily look up prefab
        //done this way because it is not possible to serializeField a dictionary
        Dictionary<FurnitureType.type, GameObject> furnitureDictionary = new();
        for (int i = 0; i < prefabs.Length; i++)
        {
            furnitureDictionary.Add(prefabs[i].type, prefabs[i].prefab);
        }

        furniture.Clear();
        foreach (FurnitureObject furnitureObject in furnitureObjects)
        {
            Instantiate(furnitureDictionary[furnitureObject.furnitureType], 
                new Vector3(furnitureObject.FurniturePosition[0], furnitureObject.FurniturePosition[1], furnitureObject.FurniturePosition[2]), 
                Quaternion.identity);
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
