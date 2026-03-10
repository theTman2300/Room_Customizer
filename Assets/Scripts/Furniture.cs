using UnityEngine;

public class Furniture : MonoBehaviour
{
    [SerializeField] public FurnitureType.type furnitureType;
    [SerializeField] float moveSpeed = 1;
    [SerializeField] bool isSelected;
    [Space]
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] float sprintSpeedMulitplier = 3;

    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y * 0.1f); //set z to y for vertical sprite sorting
        GameObject.FindWithTag("SaveLoadRoom").GetComponent<SaveLoadRoom>().AddFurniture(this);
    }

    public FurnitureObject GetFurnitureObject()
    {
        FurnitureObject furnitureObject = ScriptableObject.CreateInstance<FurnitureObject>();
        furnitureObject.furnitureType = furnitureType;
        furnitureObject.FurniturePosition = new float[3] { transform.position.x, transform.position.y, transform.position.z};
        return furnitureObject;
    }

    public void SetSelectedState(bool state)
    {
        isSelected = state;
        if (isSelected)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.8f);
        }
        else
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        }
    }

    private void Update()
    {
        if (!isSelected) return;

        if (Input.GetKeyUp(KeyCode.Delete) || Input.GetKeyUp(KeyCode.Backspace))
        {
            DeleteFurniture();
            return;
        }

        Vector2 input = default;
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input.Normalize();

        float speed = moveSpeed;
        if (Input.GetKey(sprintKey)) speed *= sprintSpeedMulitplier;

        transform.position = new Vector3(transform.position.x + input.x * speed * Time.deltaTime, 
            transform.position.y + input.y * speed * Time.deltaTime, 
            transform.position.y * 0.1f);


    }

    void DeleteFurniture()
    {
        GameObject.FindWithTag("SaveLoadRoom").GetComponent<SaveLoadRoom>().RemoveFurniture(this);
        Destroy(gameObject);
    }
}
