using UnityEngine;

public class CharacterIdleState : CharacterState
{
    public CharacterIdleState(Character character, CharacterStateMachine characterStateMachine) : base(character, characterStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        character.Input.OnAttack += OnAttack;
        character.Input.OnSkill += OnSkill;
        character.Input.OnUlt += OnUlt;
        character.Input.OnDash += OnDash;
        character.Input.OnInteract += OnInteract;


        character.LastDesiredSpeed = character.DesiredSpeed;
        character.DesiredSpeed = 0f;
    }

    public override void ExitState()
    {
        base.ExitState();

        character.Input.OnAttack -= OnAttack;
        character.Input.OnSkill -= OnSkill;
        character.Input.OnUlt -= OnUlt;
        character.Input.OnDash -= OnDash;
        character.Input.OnInteract -= OnInteract;
    }

    public override void ChangeStateChecks()
    {
        base.ChangeStateChecks();

        if (character.IsMoving() && character.IsSprinting)
        {
            character.ChangeCharacterState(character.SprintingState);
        }

        if (character.IsMoving())
        {
            character.ChangeCharacterState(character.MovingState);
        }
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        IdleMove();
    }
    private void IdleMove()
    {
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
