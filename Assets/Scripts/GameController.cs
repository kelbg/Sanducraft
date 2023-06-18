using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Singleton pattern
    private static GameController _instance;
    public static GameController Instance { get => _instance; }

    public Sandwich Sandwich;
    public Transform PlatedItems;
    public const int MaxIngredients = 3;
    public const float StackOffsetZ = 0.05f; // To stack items on top of each other
    private OrderGenerator orderGenerator;
    private Scoring scoring;

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
        Sandwich = ScriptableObject.CreateInstance<Sandwich>();
        orderGenerator = GetComponent<OrderGenerator>();
        scoring = GetComponent<Scoring>();

        orderGenerator.NewRandomOrder();
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
                case "Bell":
                    // SandwichScore();
                    scoring.CalculateScore(Sandwich, orderGenerator.CurrentOrder);
                    orderGenerator.NewRandomOrder();
                    ClearPlate();
                    break;
            }
        }
    }

    private void DragItem(GameObject item)
    {
        var dragAndDrop = item.AddComponent<DragAndDrop>();
        dragAndDrop.DragEnd += OnItemDropped;
        dragAndDrop.DragEnd += (_) => UpdatePlateStackOffset();
    }

    private void OnItemDropped(GameObject item)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hits = Physics.RaycastAll(ray);

        if (!hits.Any(hit => hit.collider.tag == "Plate"))
        {
            RemoveFromPlate(item);
            return;
        }

        // Move item to the top of the stack if it's on the plate
        if (item.transform.IsChildOf(PlatedItems))
        {
            item.transform.SetSiblingIndex(PlatedItems.childCount - 1);
            return;
        }

        AddToPlate(item);
    }

    private void SetZOffset(Transform item, float offset)
    {
        Vector3 newPos = new(
            item.position.x,
            item.position.y,
            -Camera.main.nearClipPlane - offset);

        item.position = newPos;
    }

    private void UpdatePlateStackOffset()
    {
        for (int i = 0; i < PlatedItems.childCount; i++)
            SetZOffset(PlatedItems.GetChild(i), StackOffsetZ * i);
    }

    private void AddToPlate(GameObject item)
    {
        item.TryGetComponent<ItemContainer>(out var food);

        Sandwich.Contents.Add(food.Item);
        item.transform.SetParent(PlatedItems);
    }

    private void RemoveFromPlate(GameObject item)
    {
        item.TryGetComponent<ItemContainer>(out var food);

        Sandwich.Contents.Remove(food.Item);
        Destroy(item);
    }

    private void ClearPlate()
    {
        foreach (Transform child in PlatedItems)
            RemoveFromPlate(child.gameObject);
    }

    // private int SandwichScore()
    // {
    //     var order = orderGenerator.CurrentOrder.Contents;
    //     var sandwich = Sandwich.Contents;
    //     var diff = order.Except(sandwich).ToList();

    //     // Linq Except excludes duplicates, meaning if there is one slice of bread 
    //     // in the sandwich but two in the order it won't be counted as a difference
    //     if (Sandwich.BreadSlices != orderGenerator.CurrentOrder.BreadSlices)
    //         diff.Add(order.Find(x => x.FoodType == FoodItem.Type.Bread));

    //     if (diff.Any())
    //     {
    //         Debug.Log($"-{PointsPerSandwich} Extra or missing ingredients: {string.Join(", ", diff)}");
    //         return -PointsPerSandwich;
    //     }

    //     Debug.Log($"+{PointsPerSandwich} Perfect sandwich! ");
    //     return PointsPerSandwich;
    // }
}
