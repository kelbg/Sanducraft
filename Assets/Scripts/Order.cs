using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Order : MonoBehaviour
{
    public TextMeshProUGUI SandwichName;
    public TextMeshProUGUI SandwichIngredients;
    public Image SandwichIcon;
    public Sandwich Sandwich { get; private set; }

    void Start()
    {
        
    }

    public void SetNewOrder(Sandwich sandwich)
    {
        Sandwich = sandwich;
        SandwichName.text = sandwich.Name;
        SandwichIcon.sprite = sandwich.Icon;
        SandwichIngredients.text = sandwich.Recipe;
    }
}
