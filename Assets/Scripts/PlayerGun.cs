using UnityEngine;
using UnityEngine.InputSystem;

// Player class for the gun class
public class PlayerGun : Gun
{
    [Header("Player Settings")]
    public Camera fpsCam;

    private InputAction shootAction;
    private InputAction reloadAction;
    private bool shooting;

    protected override void Awake()
    {
        base.Awake();
        // press left mouse button to shoot
        shootAction = new InputAction("Shoot", binding: "<Mouse>/leftButton");
        // press r to reload
        reloadAction = new InputAction("Reload", binding: "<Keyboard>/r");

        shootAction.Enable();
        reloadAction.Enable();
    }

    private void OnDestroy()
    {
        shootAction.Disable();
        reloadAction.Disable();
    }

    protected override void Update()
    {
        base.Update();
        HandleInput();
    }

    private void HandleInput()
    {
        // Allow button hold for shooting
        if (allowButtonHold)
            shooting = shootAction.IsPressed();
        else
            shooting = shootAction.WasPressedThisFrame();
        if (reloadAction.WasPressedThisFrame() && bulletsLeft < magazineSize && !reloading)
            Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)
            Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            Shoot(fpsCam);
        }
    }
}