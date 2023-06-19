using System;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    public float GameCountdown = 5;
    public TextMeshProUGUI CountdownDisplay;

    public event Action GameStart;
    public event Action GameEnd;

    void Start()
    {
        GameStart?.Invoke();
    }

    void Update()
    {
        if (GameCountdown > 0)
        {
            GameCountdown -= Time.deltaTime;
        }
        else
        {
            GameCountdown = 0;
            GameEnd?.Invoke();
            GameEnd = null; // Fires only once
        }

        CountdownDisplay.text = $"Tempo restante: {TimeSpan.FromSeconds(GameCountdown):mm\\:ss}";
    }
}
