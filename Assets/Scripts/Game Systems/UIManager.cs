using UnityEngine;


public class UIManager : MonoBehaviour
{
    public void PlayButton(){
        GameManager.Instance.LoadNextScene();
    }
    
}
