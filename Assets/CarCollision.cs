using UnityEngine;
using UnityEngine.SceneManagement;

public class CarCollision : MonoBehaviour
{
    public GameObject gameOverText;
    public GameObject restartButton;
    public ScoreManager scoreManager;

    bool gameOver = false;

    void Start()
    {
        gameOverText.SetActive(false);
        restartButton.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("TrafficCar") && !gameOver)
        {
            gameOver = true;

            gameOverText.SetActive(true);
            restartButton.SetActive(true);

            //scoreManager.gameOver = true;
            Time.timeScale = 0f;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
