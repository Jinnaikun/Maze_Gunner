using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {


    public Text gameOverText; //Contains the game over text


    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ResetLevel();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            CloseGame();
        }

    }

    //Resets level on button press
    void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Exits game on button press
    void CloseGame()
    {
        Application.Quit();
    }

    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
    }
}
