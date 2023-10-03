using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using UnityEngine.SceneManagement;

public class GameState
{
    public virtual void EnterState() { }

    public virtual void UpdateState() { }

    public virtual void ExitState() { }
}

public class GS_InPlay : GameState
{
    public override void EnterState() 
    {
        if (PlayerManager.PControls != null) 
        {
            PlayerManager.PControls.Gameplay.Enable();
            PlayerManager.PControls.Menu.Pause.started += GameManager.Instance.PauseGame;

            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public override void UpdateState() { }

    public override void ExitState() 
    {
        PlayerManager.PControls?.Gameplay.Disable();
        PlayerManager.PControls.Menu.Pause.started -= GameManager.Instance.PauseGame;

        Cursor.lockState = CursorLockMode.None;
    }
}

public class GS_Paused : GameState
{
    public override void EnterState() 
    {
        GameManager.Instance.PauseMenu.SetActive(true);
        PlayerManager.PControls.Menu.Pause.started += GameManager.Instance.PauseGame;

        Time.timeScale = 0;
    }

    public override void UpdateState() { }

    public override void ExitState() 
    {
        GameManager.Instance.PauseMenu?.SetActive(false);
        PlayerManager.PControls.Menu.Pause.started -= GameManager.Instance.PauseGame;

        Time.timeScale = 1;
    }
}

public class GS_Finished : GameState
{
    //early reset implementation
    float _timerToReset = 5;

    bool _resetting;

    public override void EnterState() 
    {
        GameManager.Instance.AwardAccolades();

        PlayerManager.Instance.Health.enabled = false;
    }

    public override void UpdateState() 
    {
        //Basic Implementation
        if (_timerToReset > 0)
        {
            _timerToReset -= Time.deltaTime;
        }
        else if (_timerToReset <= 0 && !_resetting) 
        {
            _resetting = true;
            GameManager.Instance.ChangeState(new GS_InPlay());
        }
    }

    public override void ExitState() 
    {
        Scene curScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(curScene.name);
    }
}