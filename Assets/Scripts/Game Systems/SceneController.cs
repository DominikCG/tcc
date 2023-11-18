using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 4)
        {
            AudioManager.Instance.PlayAudio(AudioClipID.LevelBGM2);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            AudioManager.Instance.PlayAudio(AudioClipID.LevelBGM3);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 6)
        {
            AudioManager.Instance.PlayAudio(AudioClipID.LevelBGM4);
        }
        else
        {
            AudioManager.Instance.PlayAudio(AudioClipID.LevelBGM);
        }
    }
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F1))
       // {
            // Ação a ser executada quando a tecla F1 for pressionada
       //     Debug.Log("Tecla F1 pressionada!");
        //    NextScene();
            // Insira aqui o código que você deseja executar quando a tecla F1 for pressionada
       // }
    }

    public void ChangeScene(int index)
    {
        if (SceneManager.GetActiveScene().buildIndex != index)
        {
            SceneManager.LoadScene(index);
        }
    }

    public void NextScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    public void MainMenu() => SceneManager.LoadScene(0);

    public void PreviousScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

    public void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
