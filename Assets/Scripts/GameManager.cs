using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject startCanvas; // UI screen shown at the beginning
    public GameObject loseCanvas;  // UI screen shown when player loses
    public GameObject winCanvas;   // UI screen shown when player wins

    public bool gameStarted = false; // Tracks if the game has started

    void Start()
    {
        // Show the start screen
        if (startCanvas != null)
            startCanvas.SetActive(true);

        // Hide the lose and win screens
        if (loseCanvas != null)
            loseCanvas.SetActive(false);

        if (winCanvas != null)
            winCanvas.SetActive(false);

        // Pause the game until the player presses Play
        Time.timeScale = 0f;
    }

    // Called when the player presses the Play button
    public void StartGame()
    {
        gameStarted = true; // Mark the game as started

        // Hide all UI screens
        if (startCanvas != null)
            startCanvas.SetActive(false);

        if (loseCanvas != null)
            loseCanvas.SetActive(false);

        if (winCanvas != null)
            winCanvas.SetActive(false);

        // Resume the game
        Time.timeScale = 1f;
    }

    // Called when the player loses
    public void ShowLoseScreen()
    {
        if (loseCanvas != null)
            loseCanvas.SetActive(true); // Show the lose screen

        Time.timeScale = 0f; // Pause the game
    }

    // Called when the player wins
    public void ShowWinScreen()
    {
        if (winCanvas != null)
            winCanvas.SetActive(true); // Show the win screen

        Time.timeScale = 0f; // Pause the game
    }

    // Called when the player clicks Retry
    public void RestartGame()
    {
        // Reload the current scene to restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
