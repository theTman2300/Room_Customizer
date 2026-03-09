using NaughtyAttributes;
using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class SlotsManager : MonoBehaviour
{
    [SerializeField] Slot[] slots;

    string path = "";

    void Start()
    {
        path = Application.persistentDataPath + "/Slots.JSON";
        print(path);
    }

    [Button]
    void LoadSlots()
    {
        slots = JsonConvert.DeserializeObject<Slot[]>(File.ReadAllText(path));
    }

    [Button]
    void SaveSlots()
    {
        string json = JsonConvert.SerializeObject(slots, Formatting.Indented);
        print(json);
        File.WriteAllText(path, json);
    }
}

