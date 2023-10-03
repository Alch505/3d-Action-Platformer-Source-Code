using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Player;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State { get; private set; }

    public bool StartWithDebugOn;

    public GameObject PauseMenu;

    //Accolades
    int _enemiesKilled;
    int _totalEnemies;

    bool _wasDamaged;

    bool _noDmgFighter;
    bool _fighter;
    bool _noDmgNormal;
    bool _normal;
    bool _noDmgPacifist;
    bool _pacifist;

    void Awake() 
    {
        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else 
        {
            Destroy(this.gameObject);
            return;
        }

        SceneManager.sceneLoaded += SceneRestarted;
        ChangeState(new GS_InPlay());
    }

    void Update() 
    {
        State?.UpdateState();
    }

    public void ChangeState(GameState newState) 
    {
        State?.ExitState();

        State = newState;

        State.EnterState();
    }

    public void PauseGame(InputAction.CallbackContext ctx) 
    {
        if (State is GS_InPlay) ChangeState(new GS_Paused());
        else if (State is GS_Paused) ChangeState(new GS_InPlay());
    }

    public void EnterDebug() { }

    public void SceneRestarted(Scene scene, LoadSceneMode mode) 
    {
        ChangeState(new GS_InPlay());

        _enemiesKilled = 0;
        _totalEnemies = 0;
        _wasDamaged = false;
    }

    public void AwardAccolades() 
    {
        if (_enemiesKilled == 0)
        {
            _pacifist = true;
            if (!_wasDamaged) _noDmgPacifist = true;
        }
        else if (_enemiesKilled >= _totalEnemies)
        {
            _fighter = true;
            if (!_wasDamaged) _noDmgFighter = true;
        }
        else 
        {
            _normal = true;
            if (!_wasDamaged) _noDmgNormal = true;
        }
    }

    public void AddEnemyToCount() { _totalEnemies += 1; }

    public void AddToEnemiesKilled() { _enemiesKilled += 1; }

    public void PlayerWasDamaged() { _wasDamaged = true; }
}
