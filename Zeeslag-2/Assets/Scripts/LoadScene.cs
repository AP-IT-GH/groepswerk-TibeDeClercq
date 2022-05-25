using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public GameObject pauseCanvas;
    public GameObject mainMenuCanvas;
    public GameObject victoryCanvas;
    public GameObject gameOverCanvas;
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
    }
    public void Pause()
    {
        Vector3 vHeadPos = Camera.main.transform.position;
        Vector3 vGazeDir = Camera.main.transform.forward;
        pauseCanvas.transform.position = (vHeadPos + vGazeDir * 3.0f) + new Vector3(0.0f, -.40f, 0.0f);
        Vector3 vRot = Camera.main.transform.eulerAngles;
        vRot.z = 0;
        pauseCanvas.transform.eulerAngles = vRot;
        pauseCanvas.SetActive(true);
        victoryCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
        mainMenuCanvas.SetActive(false);
    }
    public void mainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        mainMenuCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        victoryCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
    }
    private void Update()
    {
        if(mainMenuCanvas.activeInHierarchy == true || pauseCanvas.activeInHierarchy == true)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}   
