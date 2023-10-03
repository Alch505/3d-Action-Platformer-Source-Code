using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Player;

public class PauseMenu : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.PauseMenu = this.gameObject;
        this.gameObject.SetActive(false);
    }

    public void ResumeGame() 
    {
        GameManager.Instance.ChangeState(new GS_InPlay());
    }

    public void RestartGame() 
    {
        GameManager.Instance.ChangeState(new GS_InPlay());

        Scene curScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(curScene.name);
    }

    public void QuitGame() 
    {
        Application.Quit();
    }
}
