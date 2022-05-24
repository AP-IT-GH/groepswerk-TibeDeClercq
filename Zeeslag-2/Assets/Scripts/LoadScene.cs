using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public GameObject pauseCanvas;
    public void _LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void doExitGame()
    {
        Application.Quit();
    }
    public void Resume()
    {
        Time.timeScale = 1;
        pauseCanvas.SetActive(false);
    }
    public void Pause()
    {
        Time.timeScale = 0;
        Vector3 vHeadPos = Camera.main.transform.position;
        Vector3 vGazeDir = Camera.main.transform.forward;
        pauseCanvas.transform.position = (vHeadPos + vGazeDir * 3.0f) + new Vector3(0.0f, -.40f, 0.0f);
        Vector3 vRot = Camera.main.transform.eulerAngles; vRot.z = 0;
        pauseCanvas.transform.eulerAngles = vRot;
        pauseCanvas.SetActive(true);
    }

}
