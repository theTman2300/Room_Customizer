using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotsPresenter : MonoBehaviour
{
    [SerializeField] KeyCode menuKey = KeyCode.Escape;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] float padding = 50;
    [SerializeField] GameObject SlotUI;
    [SerializeField] Transform SlotUIPosition;

    bool menuShown = false;
    SlotsManager slotsManager;
    SaveLoadRoom saveLoadRoom;

    private void Start()
    {
        slotsManager = GameObject.FindWithTag("SlotsManager").GetComponent<SlotsManager>();
        saveLoadRoom = GameObject.FindWithTag("SaveLoadRoom").GetComponent<SaveLoadRoom>();
        HideMenu();
    }

    private void Update()
    {
        if (!Input.GetKeyUp(menuKey)) return;

        menuShown = !menuShown;
        if (menuShown) ShowMenu();
        else HideMenu();
    }

    void HideMenu()
    {
        SlotUI.gameObject.SetActive(false);
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    void ShowMenu()
    {
        SlotUI.gameObject.SetActive(true);
        Slot[] slots = slotsManager.GetSlots();
        for (int i = 0; i < slots.Length; i++)
        {
            int index = i;
            GameObject slotObject = Instantiate(slotPrefab, SlotUIPosition);
            slotObject.GetComponent<RectTransform>().anchoredPosition = 
                new Vector2(SlotUI.GetComponent<RectTransform>().anchoredPosition.x,
                SlotUI.GetComponent<RectTransform>().anchoredPosition.y - (padding * i));

            slotObject.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = slots[index].name;
            slotObject.GetComponent<Button>().onClick.AddListener(() => SlotClicked(slots[index]));
        }
    }

    public void SlotClicked(Slot slot)
    {
        saveLoadRoom.LoadRoom(slot);
    }

    public void SaveNew(TMP_InputField name)
    {
        print(name.text);
        if (File.Exists(Application.persistentDataPath + "/Slots/" + name.text + ".JSON"))
        {
            print("File already exists, canceling save");
            return;
        }

        Slot newSlot = new Slot();
        newSlot.name = name.text;
        newSlot.Path = Application.persistentDataPath + "/Slots/" + name.text + ".JSON";
        saveLoadRoom.SaveRoom(newSlot);
        slotsManager.AddSlot(newSlot);

        //save new slot
        slotsManager.SaveSlots();
        slotsManager.LoadSlots(); //not sure if the loading is necessary, but it can't hurt

        //refresh menu
        HideMenu();
        ShowMenu();
    }
}
