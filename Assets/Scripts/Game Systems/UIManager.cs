using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject endGamePanel = default;
    [SerializeField] private TMP_Text title= default;
    [SerializeField] private TMP_Text score= default;
    [SerializeField] private TMP_Text finalScore= default;
    [SerializeField] private Button nextbutton= default;
    [SerializeField] private Button retrybutton= default;

    public void Awake()
    {
        if (GameManager.Instance.isLevelScene)
        {

            nextbutton.gameObject.SetActive(false);
            retrybutton.gameObject.SetActive(false);
        }
    }
    public void PlayButton()
    {
        GameManager.Instance.LoadNextScene();
    }

    public void EndGamePanel(bool STATUS)
    {
        endGamePanel.SetActive(true);
        if (STATUS)
        {
            title.text = "Congratulations!";
            finalScore.text = score.text;
            nextbutton.gameObject.SetActive(true);
        }
        else
        {
            title.text = "Game Over!";
            finalScore.text = score.text;
            retrybutton.gameObject.SetActive(true);
        }

    }
}
