using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sandwich", menuName = "Sandwich")]
public class Sandwich : ScriptableObject
{
    public Sprite Icon;
    public string Name;
    public List<FoodItem> Contents = new();
    public List<FoodItem> Ingredients =>
        Contents.Where(x => x.FoodType != FoodItem.Type.Bread).ToList();
    public int BreadSlices => Contents.Count(x => x.FoodType == FoodItem.Type.Bread);

    public string Recipe
    {
        get
        {
            string recipe = "";
            Ingredients.ForEach(x => recipe += $"- {x.Name}\n");
            return recipe;
        }
    }

    public bool HasItem(FoodItem food) => Contents.Any(x => x.FoodType == food.FoodType);
}
