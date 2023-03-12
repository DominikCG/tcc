using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private SceneController sceneController;
    private UIManager uiManager;

    public bool isLevelScene;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        sceneController = FindObjectOfType<SceneController>();
        uiManager = FindObjectOfType<UIManager>();

        InitGame();
    }

    private void InitGame()
    {

    }

    #region Scene Functions
    public void LoadScene(int sceneIndex) => sceneController.ChangeScene(sceneIndex);

    public void LoadNextScene() => sceneController.NextScene();

    public void LoadPreviousScene() => sceneController.PreviousScene();

    public void ReloadGame() => sceneController.ReloadScene();
    #endregion

    public void QuitGame() => Application.Quit();
}
