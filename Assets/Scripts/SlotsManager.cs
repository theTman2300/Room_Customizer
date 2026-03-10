using NaughtyAttributes;
using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

public class SlotsManager : MonoBehaviour
{
    [SerializeField] List<Slot> Slots;

    string path = "";

    void Start()
    {
        path = Application.persistentDataPath + "/Slots.JSON";
        print(path);

        if (!File.Exists(path)) SaveSlots();

        LoadSlots();
    }

    public void LoadSlots()
    {
        Slots = JsonConvert.DeserializeObject<Slot[]>(File.ReadAllText(path)).ToList();
        print("slots loaded");
    }

    public void SaveSlots()
    {
        string json = JsonConvert.SerializeObject(Slots.ToArray(), Formatting.Indented);
        print(json);
        File.WriteAllText(path, json);
        print("Slots saved");
    }

    public void AddSlot(Slot slot)
    {
        Slots.Add(slot);
    }

    public void RemoveSlot(Slot slot)
    {
        Slots.Remove(slot);
    }

    public Slot[] GetSlots()
    {
        return Slots.ToArray();
    }
}

