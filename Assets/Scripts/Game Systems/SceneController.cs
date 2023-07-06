using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            // Ação a ser executada quando a tecla F1 for pressionada
            Debug.Log("Tecla F1 pressionada!");
            NextScene();
            // Insira aqui o código que você deseja executar quando a tecla F1 for pressionada
        }
    }

    public void ChangeScene(int index)
    {
        if (SceneManager.GetActiveScene().buildIndex != index)
        {
            SceneManager.LoadScene(index);
        }
    }

    public void NextScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    public void PreviousScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

    public void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
