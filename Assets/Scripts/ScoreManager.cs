using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;
    public static ScoreManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ScoreManager>();
            }

            return _instance;
        }
    }
    private int _currentScore = 0;
    private int _lastScore = 0;

    public TextMeshProUGUI ScoreLabel;
    public TextMeshProUGUI LastScoreLabel;
    public TextMeshProUGUI GameOverLabel;

    public void SumToScore(int points)
    {
        _currentScore += points;
    }

    private void Start() {
        _lastScore = PlayerPrefs.GetInt("lastScore");
    }

    private void Update() {
        ScoreLabel.text = "Score: " + _currentScore;
        LastScoreLabel.text = "Score to beat: "  + _lastScore;
    }

    public void GameOver()
    {
        PlayerPrefs.SetInt("lastScore", _currentScore);
        GameOverLabel.gameObject.SetActive(true);
    }


}
