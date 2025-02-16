using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovingState : CharacterState
{
    [System.Serializable]
    public struct Descriptor
    {
        public float walkSpeed;
    }

    Descriptor m_desc;
    private Vector3 m_moveDirection;

    public CharacterMovingState(Character character, CharacterStateMachine characterStateMachine, Descriptor desc) : base(character, characterStateMachine)
    {
        m_desc = desc;
    }

    public override void EnterState()
    {
        base.EnterState();

        character.Input.OnSprint += OnSprint;
        character.Input.OnAttack += OnAttack;
        character.Input.OnSkill += OnSkill;
        character.Input.OnUlt += OnUlt;
        character.Input.OnDash += OnDash;
        character.Input.OnInteract += OnInteract;

        character.LastDesiredSpeed = character.DesiredSpeed;
        character.DesiredSpeed = m_desc.walkSpeed;
    }

    public override void ExitState()
    {
        base.ExitState();

        character.Input.OnSprint -= OnSprint;
        character.Input.OnAttack -= OnAttack;
        character.Input.OnSkill -= OnSkill;
        character.Input.OnUlt -= OnUlt;
        character.Input.OnDash -= OnDash;
        character.Input.OnInteract -= OnInteract;
    }

    public override void ChangeStateChecks()
    {
        base.ChangeStateChecks();

        if (character.IsMoving() == false)
        {
            character.ChangeCharacterState(character.IdleState);
        }
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        m_moveDirection = character.GetMoveDirection();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        WalkingMove();
    }

    private void WalkingMove()
    {
        character.RB.AddForce(m_moveDirection * character.Speed * 10f, ForceMode.Force);
    }

    private void OnSprint()
    {
        character.ChangeCharacterState(character.SprintingState);

        character.IsSprinting = !character.IsSprinting;
    }

    private void OnAttack()
    {
        character.ChangeCharacterState(character.AttackingState);
    }

    private void OnSkill()
    {
        character.ChangeCharacterState(character.SkillState);
    }

    private void OnUlt()
    {
        character.ChangeCharacterState(character.UltState);
    }

    private void OnDash()
    {
        character.ChangeCharacterState(character.DashState);
    }

    private void OnInteract()
    {
        character.ChangeCharacterState(character.InteractState);
    }
}
