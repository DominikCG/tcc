using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void ChangeScene(int index)
    {
        if(SceneManager.GetActiveScene().buildIndex != index)
        {
            SceneManager.LoadScene(index);
        }
    }

    public void NextScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    public void PreviousScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

    public void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
