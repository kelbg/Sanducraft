using UnityEngine;

public class OrderGenerator : MonoBehaviour
{
    public GameObject OrderDisplay;
    public Sandwich[] Menu;

    public Sandwich CurrentOrder { get; private set; }

    public Sandwich NewRandomOrder()
    {
        var index = Random.Range(0, Menu.Length);
        CurrentOrder = Menu[index];
        OrderDisplay.GetComponent<Order>().SetNewOrder(CurrentOrder);
        return CurrentOrder;
    }
}
