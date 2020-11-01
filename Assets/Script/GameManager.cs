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
    }

    public void PlayerDeath()
    {
        playersAlive = playersAlive - 1;
    }

    public void WinGame()
    {
        // UI - Ghost Wins
        // Play Again and Quit
    }
    
    public void LoseGame()
    {
        // UI - Ghost Loses
        // Play Again and Quit
        Invoke("Restart", restartDelay);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
