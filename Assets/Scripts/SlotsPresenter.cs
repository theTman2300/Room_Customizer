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
    [Space]
    [SerializeField] TextMeshProUGUI modeText;

    bool menuShown = false;
    SlotsManager slotsManager;
    SaveLoadRoom saveLoadRoom;

    public enum mode { loading, saving, deleting}
    mode interactionMode = mode.loading;

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
        for (int i = 0; i < SlotUIPosition.transform.childCount; i++)
        {
            Destroy(SlotUIPosition.transform.GetChild(i).gameObject);
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
        switch (interactionMode) 
        {
            case mode.loading:
                saveLoadRoom.LoadRoom(slot);
                break;
            case mode.saving:
                saveLoadRoom.SaveRoom(slot);
                break;
            case mode.deleting:
                DeleteSlot(slot);
                break;
        }
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

    void DeleteSlot(Slot slot)
    {
        slotsManager.RemoveSlot(slot);

        //save new slot
        slotsManager.SaveSlots();
        slotsManager.LoadSlots(); //not sure if the loading is necessary, but it can't hurt

        //refresh menu
        HideMenu();
        ShowMenu();
    }

    public void OnLoadModePressed()
    {
        interactionMode = mode.loading;
        modeText.text = "Mode: Loading"; 
    }

    public void OnSaveModePressed()
    {
        interactionMode = mode.saving;
        modeText.text = "Mode: Saving";
    }

    public void OnDeleteModePressed()
    {
        interactionMode = mode.deleting;
        modeText.text = "Mode: Deleting";
    }
}
