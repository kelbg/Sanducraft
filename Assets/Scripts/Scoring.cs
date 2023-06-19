using System.Linq;
using TMPro;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    public GameObject ScoreDisplay;
    public TextMeshProUGUI FloatingScore;
    public int PointsPerSandwich = 100;
    public int CurrentScore
    {
        get => _currentScore;

        // Updates score display whenever a new value is set
        private set
        {
            int difference = value - _currentScore;
            if (difference != 0)
                ShowFloatingScore(difference);

            _currentScore = value;
            UpdateScoreDisplay(value);
        }
    }

    private TextMeshProUGUI scoreDisplay;
    private int _currentScore;
    private Animator floatingScoreAnim;

    void Start()
    {
        scoreDisplay = ScoreDisplay.GetComponent<TextMeshProUGUI>();
        floatingScoreAnim = FloatingScore.GetComponent<Animator>();
        CurrentScore = 0;
    }

    public int CalculateScore(Sandwich sandwich, Sandwich order)
    {
        var diff = order.Contents.Except(sandwich.Contents).ToList();

        // Linq Except excludes duplicates, meaning if there is one slice of bread 
        // in the sandwich but two in the order it won't be included in diff
        if (sandwich.BreadSlices != order.BreadSlices)
            diff.Add(order.Contents.Find(x => x.FoodType == FoodItem.Type.Bread));

        if (sandwich.Contents.Count > order.Contents.Count)
        {
            Debug.Log($"-{PointsPerSandwich} Sanduíche tem mais ingredientes do que deveria!");
            CurrentScore -= PointsPerSandwich;
            return -PointsPerSandwich;
        }

        if (diff.Any())
        {
            Debug.Log($"-{PointsPerSandwich} Ingredientes faltando: {string.Join(", ", diff)}");
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

    public void ShowFloatingScore(int value)
    {
        FloatingScore.text = $"{(value > 0 ? "+" : "")}{value}";
        floatingScoreAnim.StopPlayback();
        floatingScoreAnim.Play("FloatingScore");
    }
}
