using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static Character;

public class CharacterUltState : CharacterState
{
    [System.Serializable]
    public struct Descriptor
    {
        public float ultStrength;
        public float ultDuration;
        public UnityEvent OnUlt;
    }

    Descriptor m_desc;
    private float m_timer;
    private Vector3 m_moveDirection;

    public CharacterUltState(Character character, CharacterStateMachine characterStateMachine, Descriptor desc) : base(character, characterStateMachine)
    {
        m_desc = desc;
    }

    public override void EnterState()
    {
        base.EnterState();

        character.PlayerAnimator.SetTrigger("Ult");

        m_desc.OnUlt?.Invoke();

        character.CreateSpell();


        character.LastDesiredSpeed = character.DesiredSpeed;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void ChangeStateChecks()
    {
        base.ChangeStateChecks();

        if (m_timer >= m_desc.ultDuration)
        {
            m_timer = 0f;
            character.ChangeCharacterState(character.IdleState);
        }
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        m_timer += Time.deltaTime;
    }
}
