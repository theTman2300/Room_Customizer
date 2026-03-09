using NaughtyAttributes;
using UnityEngine;

public class CreateFurnitureButton : MonoBehaviour
{
    [SerializeField] GameObject furniturePrefab;
    [SerializeField] Vector2 position = default;

    public void OnClick()
    {
        Instantiate(furniturePrefab, position, Quaternion.identity);
    }
}
