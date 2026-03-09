using UnityEngine;

public class FurnitureSelector : MonoBehaviour
{
    bool canSelect = true;

    GameObject currentSelectedFurniture = null;

    private void Update()
    {
        if (!canSelect) return;
        if (!Input.GetMouseButtonDown(0)) return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Furniture"))
        {
            if (currentSelectedFurniture == hit.collider.gameObject) //deselect when selecting an already selected furniture
            {
                DeselectFurniture();
                return;
            }
             
            DeselectFurniture();
            hit.collider.gameObject.GetComponent<Furniture>().SetSelectedState(true);
            currentSelectedFurniture = hit.collider.gameObject;
        }
        else
        {
            DeselectFurniture();
            currentSelectedFurniture = null;
        }
    }

    public void SetCanSelectState(bool state)
    {
        canSelect = state;
    }

    public void DeselectFurniture()
    {
        if (currentSelectedFurniture == null) return;
        currentSelectedFurniture.GetComponent<Furniture>().SetSelectedState(false);
        currentSelectedFurniture = null;
    }
}
