using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public float score;
    public bool gameOver = false;

    void Update()
    {
        if (!gameOver)
        {
            score += Time.deltaTime * 10f; // speed of scoring
            scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
        }
    }
}
