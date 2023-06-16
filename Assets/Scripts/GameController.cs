using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Singleton pattern
    private static GameController _instance;
    public static GameController Instance { get => _instance; }

    public List<GameObject> Sandwich { get; private set; }
    public const int MaxIngredients = 3;
    public const float IngredientOffsetZ = 0.05f; // To stack ingredients on top of each other

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    void Start()
    {
        Sandwich = new List<GameObject>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit);

            switch (hit.collider.tag)
            {
                case "BreadContainer":
                case "IngredientContainer":
                    var newIngredient = Instantiate(hit.collider.gameObject);
                    newIngredient.tag = newIngredient.tag.Replace("Container", "");
                    DragItem(newIngredient);
                    break;

                case "Bread":
                case "Ingredient":
                    DragItem(hit.collider.gameObject);
                    break;
            }
        }
    }

    private void DragItem(GameObject item)
    {
        var dragAndDrop = item.AddComponent<DragAndDrop>();
        dragAndDrop.DragEnd += OnItemDropped;
        dragAndDrop.DragEnd += (_) => UpdateIngredientStackOffset();
    }

    private void OnItemDropped(GameObject item)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hits = Physics.RaycastAll(ray);

        // Items dropped outside of the plate are removed
        if (!hits.Any(hit => hit.collider.tag == "Plate"))
        {
            if (Sandwich.Contains(item))
                Sandwich.Remove(item);

            Destroy(item);
            return;
        }

        // Moves the item to the top of the stack
        if (Sandwich.Contains(item))
        {
            Sandwich.Remove(item);
            Sandwich.Add(item);
            return;
        }

        if ((item.tag == "Ingredient" && Sandwich.Count(x => x.tag == "Ingredient") >= MaxIngredients)
            || (item.tag == "Bread" && Sandwich.Count(x => x.tag == "Bread") >= 2))
        {
            Destroy(item);
            return;
        }

        Sandwich.Add(item);
    }

    private void SetZOffset(GameObject item, float offset)
    {
        Vector3 newPos = new Vector3(
            item.transform.position.x,
            item.transform.position.y,
            -Camera.main.nearClipPlane - offset);

        item.transform.position = newPos;
    }

    private void UpdateIngredientStackOffset()
    {
        for (int i = 0; i < Sandwich.Count; i++)
            SetZOffset(Sandwich[i], IngredientOffsetZ * i);
    }
}
