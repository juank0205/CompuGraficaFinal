using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
    [SerializeField] private List<Image> storyImages;
    [SerializeField] private Image menuButtons;
    [SerializeField] private Button nextButton;
    private int storyStep = 0;

    private void Start() {
        SetButtonsAndStoryState(true, false);
        nextButton.gameObject.SetActive(false);
    }

    public void Quit() {
        Application.Quit();
    }

    public void Play() {
        //StartCoroutine(ActivateScene());
        //SceneManager.LoadScene(1);
        MoveStory();
    }

    private void MoveStory() {
        storyStep++;
        if (storyStep == storyImages.Count + 1) {
            SetButtonsAndStoryState(false, false);
            SceneManager.LoadScene(1);
        } else {
            SetStoryActiveOnStoryStep();
        }
    }

    private void SetButtonsAndStoryState(bool buttonsState, bool storyState) {
        menuButtons.gameObject.SetActive(buttonsState);
        foreach (Image image in storyImages)
            image.gameObject.SetActive(storyState);
    }

    private void SetStoryActiveOnStoryStep() {
        menuButtons.gameObject.SetActive(false);
        if (storyStep == 1) nextButton.gameObject.SetActive(true);
        for (int i = 0; i < storyImages.Count; i++) {
            if (i == storyStep-1)
                storyImages[i].gameObject.SetActive(true);
            else
                storyImages[i].gameObject.SetActive(false);
        }
    }

    IEnumerator ActivateScene() {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("TestLevelScene"));
    }
}
