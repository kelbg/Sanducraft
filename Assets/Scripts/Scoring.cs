using System.Linq;
using TMPro;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    public GameObject ScoreDisplay;
    public int PointsPerSandwich = 100;
    public int CurrentScore
    {
        get => _currentScore;
        // Updates score display whenever a new value is set
        private set
        {
            _currentScore = value;
            UpdateScoreDisplay(value);
        }
    }

    private TextMeshProUGUI scoreDisplay;
    private int _currentScore;

    void Start()
    {
        scoreDisplay = ScoreDisplay.GetComponent<TextMeshProUGUI>();
        CurrentScore = 0;
    }

    public int CalculateScore(Sandwich sandwich, Sandwich order)
    {
        var diff = order.Contents.Except(sandwich.Contents).ToList();

        // Linq Except excludes duplicates, meaning if there is one slice of bread 
        // in the sandwich but two in the order it won't be counted as a difference
        if (sandwich.BreadSlices != order.BreadSlices)
            diff.Add(order.Contents.Find(x => x.FoodType == FoodItem.Type.Bread));

        if (diff.Any())
        {
            Debug.Log($"-{PointsPerSandwich} Ingredientes faltando/sobressalentes: {string.Join(", ", diff)}");
            CurrentScore -= PointsPerSandwich;
            return -PointsPerSandwich;
        }

        Debug.Log($"+{PointsPerSandwich} Sanduíche perfeito!");
        CurrentScore += PointsPerSandwich;

        return PointsPerSandwich;
    }

    public void UpdateScoreDisplay(int newScore)
    {
        scoreDisplay.text = $"Pontos: {newScore}";
    }
}
