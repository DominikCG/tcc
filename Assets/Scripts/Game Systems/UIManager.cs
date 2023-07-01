using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text finalScore;
    [SerializeField] private Button nextbutton;
    [SerializeField] private Button retrybutton;

    public void Awake(){
        nextbutton.gameObject.SetActive(false);
        retrybutton.gameObject.SetActive(false);
    }
    public void PlayButton()
    {
        GameManager.Instance.LoadNextScene();
    }

    public void EndGamePanel(bool STATUS)
    {
        endGamePanel.SetActive(true);
        if(STATUS){
            title.text = "Congratulations!";
            finalScore.text = score.text;
            nextbutton.gameObject.SetActive(true);
        }else{
            title.text = "Game Over!";
            finalScore.text = score.text;
            retrybutton.gameObject.SetActive(true);
        }

    }
}
