using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

using Player;

public class Crosshair : MonoBehaviour
{
    RawImage _image;

    [SerializeField] float _fadeTime;
    [SerializeField] float _fadeRef;

    private void OnEnable()
    {
        if (PlayerManager.PControls != null) PlayerManager.PControls.Gameplay.Aim.started += ShowCrosshair;
        if (PlayerManager.PControls != null) PlayerManager.PControls.Gameplay.Aim.canceled += HideCrosshair;
    }

    private void OnDisable()
    {
        if (PlayerManager.PControls != null) PlayerManager.PControls.Gameplay.Aim.started -= ShowCrosshair;
        if (PlayerManager.PControls != null) PlayerManager.PControls.Gameplay.Aim.canceled -= HideCrosshair;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerManager.PControls.Gameplay.Aim.started += ShowCrosshair;
        PlayerManager.PControls.Gameplay.Aim.canceled += HideCrosshair;

        _image = GetComponent<RawImage>();

        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowCrosshair(InputAction.CallbackContext ctx) 
    {
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 1);
    }
    void HideCrosshair(InputAction.CallbackContext ctx)
    {
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0);
    }
}
