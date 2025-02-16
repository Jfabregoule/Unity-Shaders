using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterDashState : CharacterState
{
    [System.Serializable]
    public struct Descriptor
    {
        public float dashSpeed;
        public float dashDuration;
        public UnityEvent OnDash;
    }

    Descriptor m_desc;
    private Vector3 m_dashDirection;
    private float m_dashTimer;

    public CharacterDashState(Character character, CharacterStateMachine characterStateMachine, Descriptor desc) : base(character, characterStateMachine)
    {
        m_desc = desc;
    }

    public override void EnterState()
    {
        base.EnterState();

        m_dashDirection = -character.transform.forward.normalized;
        character.DesiredSpeed = m_desc.dashSpeed;
        m_dashTimer = m_desc.dashDuration;
        character.PlayerAnimator.SetTrigger("Dash");

    }

    public override void ExitState()
    {
        base.ExitState();

        character.DesiredSpeed = character.LastDesiredSpeed;
    }

    public override void ChangeStateChecks()
    {
        base.ChangeStateChecks();

        if (m_dashTimer <= 0)
        {
            character.ChangeCharacterState(character.IdleState);
        }
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        m_dashTimer -= Time.deltaTime;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        DashMove();
    }

    private void DashMove()
    {
        character.RB.AddForce(m_dashDirection * m_desc.dashSpeed * 10f, ForceMode.Impulse);
    }
}
