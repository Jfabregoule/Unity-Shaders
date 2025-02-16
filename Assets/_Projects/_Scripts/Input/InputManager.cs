using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Unity.VisualScripting;

[DefaultExecutionOrder(-9)]
public class InputManager : Singleton<InputManager>
{
    private PlayerControls _controls;
    private CinemachineFreeLook _freeLookCamera;

    public bool IsController;

    private bool _characterEnabled;
    private bool _combatEnabled;
    private bool _cameraRotationEnabled;

    private float longAttackTimer;
    private bool isLongAttack = false;
    private bool isFirstTimeInfunction = true;

    #region Events

    public delegate void ChangeDeviceEvent();
    public event ChangeDeviceEvent OnChangedDevice;

    public delegate void CrouchEvent();
    public event CrouchEvent OnCrouch;

    public delegate void SprintEvent();
    public event SprintEvent OnSprint;

    public delegate void AttackEvent();
    public event AttackEvent OnAttack;

    public delegate void SkillEvent();
    public event SkillEvent OnSkill;

    public delegate void UltEvent();
    public event UltEvent OnUlt;

    public delegate void DashEvent();
    public event DashEvent OnDash;

    public delegate void InteractEvent();
    public event InteractEvent OnInteract;


    #endregion

    protected override void Awake()
    {
        base.Awake();
        _controls = new PlayerControls();

        _characterEnabled = true;
        _combatEnabled = true;
        _cameraRotationEnabled = true;
        _freeLookCamera = FindObjectOfType<CinemachineFreeLook>();
    }

    private void OnEnable()
    {
        _controls.Enable();

        if (_characterEnabled)
            BindCharacterEvents();
        if (_combatEnabled)
            BindCombatEvents();
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        _controls.Disable();

        if (_characterEnabled)
            UnbindCharacterEvents();
        if (_combatEnabled)
            UnbindCombatEvents();
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    #region Device Change

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
            case InputDeviceChange.Enabled:
                break;

            case InputDeviceChange.Removed:
            case InputDeviceChange.Disabled:
                break;
        }
    }

    private void UpdateControlMethod(InputControl control)
    {
        if (control == null) return;

        bool lastChange = IsController;

        IsController = control.device is Gamepad || control.device is Joystick;

        if (lastChange == IsController) return;

        OnChangedDevice?.Invoke();
    }


    #endregion


    public void DisableAllControls()
    {
        DisableCharacterControls();
        DisableCombatControls();
        DisableCameraRotation();
    }

    public void EnableAllControls()
    {
        EnableCharacterControls();
        EnableCombatControls();
        EnableCameraRotation();
    }
    public void EnableCharacterControls()
    {
        if (_characterEnabled) return;
        _characterEnabled = true;
        BindCharacterEvents();

        _controls.Character.Enable();
    }

    public void DisableCharacterControls()
    {
        if (!_characterEnabled) return;
        _characterEnabled = false;
        UnbindCharacterEvents();

        _controls.Character.Disable();
    }

    public void EnableCombatControls()
    {
        if (_combatEnabled) return;
        _combatEnabled = true;
        BindCombatEvents();

        _controls.Combat.Enable();
    }

    public void DisableCombatControls()
    {
        if (!_combatEnabled) return;
        _combatEnabled = false;
        UnbindCombatEvents();

        _controls.Combat.Disable();
    }
    public void EnableCameraRotation()
    {
        if (_cameraRotationEnabled) return;
        _cameraRotationEnabled = true;
        BindCameraRotationEvents();

        if (_freeLookCamera != null)
        {
            _freeLookCamera.m_XAxis.m_MaxSpeed = 300f;
            _freeLookCamera.m_YAxis.m_MaxSpeed = 2f;
        }
    }

    public void DisableCameraRotation()
    {
        if (!_cameraRotationEnabled) return;
        _cameraRotationEnabled = false;
        UnbindCameraRotationEvents();

        if (_freeLookCamera != null)
        {
            _freeLookCamera.m_XAxis.m_MaxSpeed = 0f;
            _freeLookCamera.m_YAxis.m_MaxSpeed = 0f;
        }
    }

    #region Public Readers

    public Vector2 GetMoveDirection()
    {
        var moveDirection = _controls.Character.Move.ReadValue<Vector2>();
        if (moveDirection != Vector2.zero)
        {
            var activeControl = GetActiveControl(_controls.Character.Move);
            UpdateControlMethod(activeControl);
        }
        return moveDirection;
    }

    private InputControl GetActiveControl(InputAction action)
    {
        InputControl activeControl = null;
        float maxValue = 0f;

        foreach (var control in action.controls)
        {
            if (control is AxisControl axisControl)
            {
                float controlValue = axisControl.ReadValue();
                if (Mathf.Abs(controlValue) > maxValue)
                {
                    maxValue = Mathf.Abs(controlValue);
                    activeControl = control;
                }
            }
        }

        return activeControl;
    }

    public Vector2 GetLookDirection() =>
        _controls.Character.Look.ReadValue<Vector2>();

    #endregion

    #region Bind / Unbind Categorized Methods

    private void BindCharacterEvents()
    {
        _controls.Character.Sprint.performed += ctx => { UpdateControlMethod(ctx.control); SprintPerformed(); };
        _controls.Character.Crouch.performed += ctx => { UpdateControlMethod(ctx.control); CrouchPerformed(); };
        _controls.Character.Dash.performed += ctx => { UpdateControlMethod(ctx.control); DashPerformed(); };
        _controls.Character.Interact.performed += ctx => { UpdateControlMethod(ctx.control); InteractPerformed(); };
        if (_cameraRotationEnabled)
            BindCameraRotationEvents();
    }

    private void UnbindCharacterEvents()
    {
        _controls.Character.Sprint.performed -= ctx => { UpdateControlMethod(ctx.control); SprintPerformed(); };
        _controls.Character.Crouch.performed -= ctx => { UpdateControlMethod(ctx.control); CrouchPerformed(); };
        _controls.Character.Dash.performed -= ctx => { UpdateControlMethod(ctx.control); DashPerformed(); };
        _controls.Character.Interact.performed -= ctx => { UpdateControlMethod(ctx.control); InteractPerformed(); };

        if (_cameraRotationEnabled)
            UnbindCameraRotationEvents();
    }

    private void BindCombatEvents()
    {
        _controls.Combat.Attack.performed += ctx => { UpdateControlMethod(ctx.control); AttackPerformed(); };
        _controls.Combat.Skill.performed += ctx => { UpdateControlMethod(ctx.control); SkillPerformed(); };
        _controls.Combat.Ult.performed += ctx => { UpdateControlMethod(ctx.control); UltPerformed(); };
    }

    private void UnbindCombatEvents()
    {
        _controls.Combat.Attack.performed -= ctx => { UpdateControlMethod(ctx.control); AttackPerformed(); };
        _controls.Combat.Skill.performed -= ctx => { UpdateControlMethod(ctx.control); SkillPerformed(); };
        _controls.Combat.Ult.performed -= ctx => { UpdateControlMethod(ctx.control); UltPerformed(); };
    }

    private void BindCameraRotationEvents()
    {
        _controls.Character.Look.performed += ctx => { };
    }

    private void UnbindCameraRotationEvents()
    {
        _controls.Character.Look.performed -= ctx => { };
    }

    #endregion

    #region Event Callers

    private void SprintPerformed() => OnSprint?.Invoke();

    private void CrouchPerformed() => OnCrouch?.Invoke();

    private void DashPerformed() => OnDash?.Invoke();

    private void InteractPerformed() => OnInteract?.Invoke();

    private void LookPerformed() { }

    private void AttackPerformed() => OnAttack?.Invoke();

    private void SkillPerformed() => OnSkill?.Invoke();

    private void UltPerformed() => OnUlt?.Invoke();


    #endregion
}