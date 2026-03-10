using System;
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
}
