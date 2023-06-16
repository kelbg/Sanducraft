using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sandwich", menuName = "Sandwich")]
public class Sandwich : ScriptableObject
{
    public Sprite Icon;
    public string Name;
    public FoodItem Bread;
    public FoodItem Ingredient1;
    public FoodItem Ingredient2;
    public FoodItem Ingredient3;

    public List<FoodItem.Type> FullRecipe => new()
    {
        Bread.FoodItemType,
        Ingredient1.FoodItemType,
        Ingredient2.FoodItemType,
        Ingredient3.FoodItemType,
        Bread.FoodItemType
    };

    public string Ingredients => $"- {Ingredient1.Name}\n- {Ingredient2.Name}\n- {Ingredient3.Name}";
}
