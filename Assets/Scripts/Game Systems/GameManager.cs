using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]private SceneController sceneController;
    [SerializeField]private UIManager uiManager;
    [SerializeField] private GameObject board;
    public bool isLevelScene = false;

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

    public void EndLevel(bool WIN)
    {
        board.SetActive(false);
        uiManager.EndGamePanel(WIN);

    }
}
