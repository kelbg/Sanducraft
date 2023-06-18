using UnityEngine;

[ExecuteInEditMode]
public class ItemContainer : MonoBehaviour
{
    public FoodItem Item;

    void Awake()
    {
        if (Item?.Mat != null)
            GetComponent<Renderer>().material = Item.Mat;
    }
}
