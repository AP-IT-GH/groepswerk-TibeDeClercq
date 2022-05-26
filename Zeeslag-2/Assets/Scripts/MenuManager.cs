using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Zeeslag game;

    public GameObject pauseCanvas;
    public GameObject mainMenuCanvas;
    public GameObject victoryCanvas;
    public GameObject gameOverCanvas;

    private bool canPause = true;
    private bool isPaused = false;
    private bool firststart = true;

    public void doExitGame()
    {
        Application.Quit();
    }
    public void StartGame()
    {
        mainMenuCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        victoryCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);

        if (firststart)
        {
            game.Play();
            firststart = false;
        }
        else
        {
            game.Restart();
        }
    }

    public void Resume()
    {
        mainMenuCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        victoryCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
        game.Play();
    }

    public void TogglePause()
    {
        if (canPause && !gameOverCanvas.activeInHierarchy && !victoryCanvas.activeInHierarchy &&!mainMenuCanvas.activeInHierarchy)
        {
            if (isPaused)
            {
                pauseCanvas.SetActive(false);
                game.Play();
                isPaused = false;
            }
            else
            {
                pauseCanvas.SetActive(true);
                victoryCanvas.SetActive(false);
                gameOverCanvas.SetActive(false);
                mainMenuCanvas.SetActive(false);

                game.Pause();
                isPaused = true;
            }
            canPause = false;
        }        
    }

    public void ReleasePauseToggle()
    {
        canPause = true;
    }

    public void mainMenu()
    {
        mainMenuCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        victoryCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);

        game.Pause();
    }
}   
