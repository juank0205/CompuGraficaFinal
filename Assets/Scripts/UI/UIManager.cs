using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
    [SerializeField] private GameObject hudCanvas;
    [SerializeField] private GameObject deathCanvas;
    [SerializeField] private GameObject endCanvas;


    private void Start() {
        SetActiveHud(HUDStates.hud);
    }

    public void SetActiveHud(HUDStates state) {
        switch (state) {
            case HUDStates.hud:
                hudCanvas.SetActive(true);
                deathCanvas.SetActive(false);
                endCanvas.SetActive(false);
                break;
            case HUDStates.dead:
                hudCanvas.SetActive(false);
                deathCanvas.SetActive(true);
                endCanvas.SetActive(false);
                break;
            case HUDStates.end:
                hudCanvas.SetActive(false);
                deathCanvas.SetActive(false);
                endCanvas.SetActive(true);
                break;
        }
    }

    public void Quit() {
        Application.Quit();
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MainMenu() {
        SceneManager.LoadScene(0);
    }
}

public enum HUDStates { hud, dead, end }
