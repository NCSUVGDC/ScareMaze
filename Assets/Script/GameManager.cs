using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    bool hasGameEnded = false;
    public float restartDelay = 7f;
    public int playersNum = 3;
    public int playersAlive = 3;
    public bool paused = false;
    public Canvas pauseCanvas;
    public Canvas gameOverCanvas;
    public Text gameOverText;
 
    
    // Update is called once per frame
    void Update()
    {
        // Track player's health if 0, game over sequence
        // Track number of AI/players
        // Track players alive, if 0, win sequence
        if (playersAlive == 0)
        {
            WinGame();
            Invoke("Restart", restartDelay);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void PlayerDeath()
    {
        playersAlive = playersAlive - 1;
    }

    public void WinGame()
    {
        GameEnd(true);
    }
    
    public void LoseGame()
    {
        // UI - Ghost Loses
        // Play Again and Quit
        GameEnd(false);
    }

    public void Restart()
    {
        gameOverCanvas.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Pause()
    {
        paused = true;
        Time.timeScale = 0;
        pauseCanvas.gameObject.SetActive(true);

    }

    public void Resume()
    {
        paused = false;
        Time.timeScale = 1;
        pauseCanvas.gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public bool IsRunning()
    {
        return (paused || hasGameEnded);
    }
    //win is a bool saying whether the game was won or lost. win is true, lose is false
    void GameEnd(bool win)
    {
        pauseCanvas.gameObject.SetActive(false);
        string message;
        if (win)
        {
            message = "You Win!";
        }
        else
        {
            message = "You Lose.";
        }
        gameOverCanvas.gameObject.SetActive(true);
        gameOverText.text = message;
    }
}
