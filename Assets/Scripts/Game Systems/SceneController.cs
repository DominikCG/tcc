using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private void Awake(){
        if(SceneManager.GetActiveScene().buildIndex == 0){
            GameManager.Instance.isLevelScene = false;
        }else{
            GameManager.Instance.isLevelScene = true;
        }

    }
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
