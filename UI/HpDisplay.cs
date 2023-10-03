using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using TMPro;

public class HpDisplay : MonoBehaviour
{
    Health _playerHealth;
    TextMeshProUGUI _text;

    private void OnEnable()
    {
        if (_playerHealth != null) _playerHealth.OnWasDamaged += UpdateDisplay;
    }

    private void OnDisable()
    {
        _playerHealth.OnWasDamaged -= UpdateDisplay;
    }

    void Start() 
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _playerHealth = PlayerManager.Instance.Health;

        _playerHealth.OnWasDamaged += UpdateDisplay;

        UpdateDisplay();
    }

    void UpdateDisplay() 
    {
        _text.text = $"HP: {_playerHealth.CurHealth} / {_playerHealth.MaxHealth}";
    }
}
