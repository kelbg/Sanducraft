using UnityEngine;

[CreateAssetMenu(fileName = "New Food Item", menuName = "Food Item")]
public class FoodItem : ScriptableObject
{
    public string Name;
    public Sprite FoodSprite;
    public Type FoodType;

    public enum Type
    {
        Bread,
        Turkey,
        Chicken,
        Cheese,
        Lettuce,
        Tomato
    }
}
