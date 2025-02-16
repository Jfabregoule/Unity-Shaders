using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{

    private bool _isInitialized = false;

    [Header("Debbuging")]
    [Space(10f)]

    [SerializeField] private bool _showState = false;
    [SerializeField] private bool _showSpeed = false;
    [SerializeField] private bool _showDesiredSpeed = false;
    [SerializeField] private bool _showLastSpeed = false;

    public Rigidbody RB { get; private set; }
    public CapsuleCollider CapsuleCollider { get; private set; }
    public InputManager Input { get; private set; }
    public Transform MainCameraTransform { get; private set; }

    [Header("Movement")]
    [Space(10f)]

    public float m_playerHeight;
    [SerializeField] private LayerMask m_whatIsGround;
    [SerializeField] private float m_groundDrag;
    [SerializeField] private float _maxSlopeAngle;

    [Header("Attack")]
    [Space(10f)]

    [SerializeField] GameObject _tornado;
    [SerializeField] GameObject _groundSlash;


    [Header("States Descriptors")]
    [Space(10f)]

    [SerializeField] private CharacterMovingState.Descriptor _movingStateDescriptor;
    [SerializeField] private CharacterSprintingState.Descriptor _sprintingStateDescriptor;
    [SerializeField] private CharacterDashState.Descriptor _dashStateDescriptor;
    [SerializeField] private CharacterInteractState.Descriptor _interactStateDescriptor;
    [SerializeField] private CharacterAttackingState.Descriptor _attackingStateDescriptor;
    [SerializeField] private CharacterLongAttackingState.Descriptor _longAttackingStateDescriptor;
    [SerializeField] private CharacterSkillState.Descriptor _skillStateDescriptor;
    [SerializeField] private CharacterUltState.Descriptor _ultStateDescriptor;

    public bool IsSprinting { get; set; }
    public float Speed { get; set; }
    public float MaxSpeed { get; set; }
    public float DesiredSpeed { get; set; }
    public float LastDesiredSpeed { get; set; }
    public Animator PlayerAnimator { get; private set; }

    public class AnimatorParameters
    {
        public const string SPEED_PARAM = "Speed";
        public const string DIRX_PARAM = "DirX";
        public const string DIRY_PARAM = "DirY";
    }

    #region State Machine Variables

    public CharacterStateMachine StateMachine { get; set; }
    public CharacterIdleState IdleState { get; set; }
    public CharacterMovingState MovingState { get; set; }
    public CharacterSprintingState SprintingState { get; set; }
    public CharacterDashState DashState { get; set; }
    public CharacterInteractState InteractState { get; set; }
    public CharacterAttackingState AttackingState { get; set; }
    public CharacterLongAttackingState LongAttackingState { get; set; }
    public CharacterSkillState SkillState { get; set; }
    public CharacterUltState UltState { get; set; }

    #endregion

    public void Init()
    {
        Input = InputManager.Instance;
        MainCameraTransform = Helpers.Camera.transform;
        RB = GetComponent<Rigidbody>();
        CapsuleCollider = GetComponent<CapsuleCollider>();

        PlayerAnimator = GetComponent<Animator>();
        RB.drag = m_groundDrag;

        StateMachine = new CharacterStateMachine();
        IdleState = new CharacterIdleState(this, StateMachine);
        MovingState = new CharacterMovingState(this, StateMachine, _movingStateDescriptor);
        SprintingState = new CharacterSprintingState(this, StateMachine, _sprintingStateDescriptor);
        DashState = new CharacterDashState(this, StateMachine, _dashStateDescriptor);
        InteractState = new CharacterInteractState(this, StateMachine, _interactStateDescriptor);
        AttackingState = new CharacterAttackingState(this, StateMachine, _attackingStateDescriptor);
        LongAttackingState = new CharacterLongAttackingState(this, StateMachine, _longAttackingStateDescriptor);
        SkillState = new CharacterSkillState(this, StateMachine, _skillStateDescriptor);
        UltState = new CharacterUltState(this, StateMachine, _ultStateDescriptor);

        IsSprinting = false;

        StateMachine.Initialize(IdleState);

        _isInitialized = true;
    }

    private void Update()
    {
        if (!_isInitialized) return;

        SpeedControl();
        UpdateAnimatorValues();

        StateMachine.CurrentCharacterState.ChangeStateChecks();
        StateMachine.CurrentCharacterState.FrameUpdate();

        if (_showState)
            Debug.Log(StateMachine.CurrentCharacterState);
        if (_showSpeed)
            Debug.Log(Speed);
        if (_showLastSpeed)
            Debug.Log(LastDesiredSpeed);
        if (_showDesiredSpeed)
            Debug.Log(DesiredSpeed);
    }

    private void FixedUpdate()
    {
        if (!_isInitialized) return;

        StateMachine.CurrentCharacterState.PhysicsUpdate();
        FacingControl();
    }

    public void ChangeCharacterState(CharacterState state)
    {
        StateMachine.ChangeState(state);
        UpdateMoveSpeed();
    }

    private void UpdateAnimatorValues()
    {
        Vector3 cameraForward = MainCameraTransform.forward;
        Vector3 cameraRight = MainCameraTransform.right;

        Vector2 moveDirection = Input.GetMoveDirection();

        Vector3 relativeDirection = (cameraForward * moveDirection.y + cameraRight * moveDirection.x).normalized;

        if (IsSprinting == false)
            moveDirection = moveDirection * 0.5f;

        Vector3 speedValue = RB.velocity;
        speedValue.y = 0;
        PlayerAnimator.SetFloat(AnimatorParameters.SPEED_PARAM, speedValue.magnitude);
        PlayerAnimator.SetFloat(AnimatorParameters.DIRX_PARAM, moveDirection.x);
        PlayerAnimator.SetFloat(AnimatorParameters.DIRY_PARAM, moveDirection.y);
    }

    public Vector3 GetMoveDirection()
    {
        Vector3 cameraForward = Vector3.ProjectOnPlane(MainCameraTransform.forward, Vector3.up).normalized;
        Vector3 cameraRight = Vector3.ProjectOnPlane(MainCameraTransform.right, Vector3.up).normalized;

        Vector2 inputDirection = Input.GetMoveDirection().normalized;

        Vector3 movedir = (inputDirection.y * cameraForward) + (inputDirection.x * cameraRight);

        return movedir;
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(RB.velocity.x, 0f, RB.velocity.z);
        float currentSpeed = flatVel.magnitude;

        if (flatVel.magnitude > Speed)
        {
            Vector3 limitedVel = flatVel.normalized * Speed;
            RB.velocity = new Vector3(limitedVel.x, RB.velocity.y, limitedVel.z);
        }
    }

    private void FacingControl()
    {
        Vector2 moveDirection = Input.GetMoveDirection();

        if (moveDirection != Vector2.zero)
        {
            Vector3 cameraForward = MainCameraTransform.forward;
            Vector3 cameraRight = MainCameraTransform.right;

            cameraForward.y = 0;
            cameraRight.y = 0;

            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 relativeDirection = (cameraForward * moveDirection.y + cameraRight * moveDirection.x).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(relativeDirection, Vector3.up);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5);
        }
    }

    private IEnumerator SmoothlyLerpMoveSpeed(float lerpSpeed)
    {
        float time = 0;
        float difference = Mathf.Abs(DesiredSpeed - Speed);
        float startValue = Speed;
        while (time < difference)
        {

            Speed = Mathf.Lerp(startValue, DesiredSpeed, time / difference);

            time += Time.deltaTime * lerpSpeed;

            yield return null;
        }
        Speed = DesiredSpeed;
    }

    private void UpdateMoveSpeed()
    {
        if (DesiredSpeed == 0f)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed(10f));
        }
        else if (Mathf.Abs(DesiredSpeed - LastDesiredSpeed) > 2f && Speed != 0f)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed(5f));
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed(10f));
        }
    }

    public bool IsMoving()
    {
        return (Input.GetMoveDirection() != Vector2.zero);
    }

    public void CreateSpell()
    {
        GameObject tornado = Instantiate(_tornado, transform.position, transform.rotation, null);
        Destroy(tornado, 5f);
    }

    public void CreateGroundSlash()
    {
        GameObject slash = Instantiate(_groundSlash, transform.position, transform.rotation, null);
        Destroy(slash, 2f);
    }
}
