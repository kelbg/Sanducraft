using UnityEngine;

public class OrderGenerator : MonoBehaviour
{
    public GameObject OrderDisplay;
    public Sandwich[] SandwichRecipes;

    public void NewRandomOrder()
    {
        var index = Random.Range(0, SandwichRecipes.Length);
        OrderDisplay.GetComponent<Order>().SetNewOrder(SandwichRecipes[index]);
    }
}
