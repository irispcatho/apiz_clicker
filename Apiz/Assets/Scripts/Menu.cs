using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class Menu : MonoBehaviour
{
    public Image Fade;

    private void Start()
    {
        Fade.DOFade(0, 1f);
    }

    public void OnClickPlay()
    {
        Fade.DOFade(1, 1f).OnComplete(FadeCompletePlay);
    }

    private void FadeCompletePlay()
    {
        SceneManager.LoadScene("Scene_Play");
    }

    public void OnClickCredits()
    {
        Fade.DOFade(1, 1f).OnComplete(FadeCompleteCredits);
    }

    private void FadeCompleteCredits()
    {
        SceneManager.LoadScene("Scene_Credits");
    }

    public void OnClickMenu()
    {
        Fade.DOFade(1, 1f).OnComplete(FadeCompleteMenu);
    }

    private void FadeCompleteMenu()
    {
        SceneManager.LoadScene("Scene_Menu");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Fade.DOFade(1, 1f).OnComplete(FadeCompleteMenu);
            Debug.Log("escape");
        }
    }


    public void OnClickQuit()
    {
        Application.Quit();
    }
}