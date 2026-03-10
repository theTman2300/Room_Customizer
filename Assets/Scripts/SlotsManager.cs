using NaughtyAttributes;
using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class SlotsManager : MonoBehaviour
{
    [SerializeField] Slot[] Slots;

    string path = "";

    void Start()
    {
        path = Application.persistentDataPath + "/Slots.JSON";
        print(path);
        LoadSlots();
    }

    [Button]
    void LoadSlots()
    {
        Slots = JsonConvert.DeserializeObject<Slot[]>(File.ReadAllText(path));
        print("slots loaded");
    }

    [Button]
    void SaveSlots()
    {
        string json = JsonConvert.SerializeObject(Slots, Formatting.Indented);
        print("Slots saved");
        print(json);
        File.WriteAllText(path, json);
    }

    public Slot[] GetSlots()
    {
        return Slots;
    }
}

