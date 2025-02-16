using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static Character;

public class CharacterSkillState : CharacterState
{
    [System.Serializable]
    public struct Descriptor
    {
        public float skillStrength;
        public float skillDuration;
        public UnityEvent OnSkill;
    }

    Descriptor m_desc;
    private float m_timer;
    private Vector3 m_moveDirection;

    public CharacterSkillState(Character character, CharacterStateMachine characterStateMachine, Descriptor desc) : base(character, characterStateMachine)
    {
        m_desc = desc;
    }

    public override void EnterState()
    {
        base.EnterState();

        m_desc.OnSkill?.Invoke();

        character.PlayerAnimator.SetTrigger("Skill");
        character.CreateGroundSlash();
        character.LastDesiredSpeed = character.DesiredSpeed;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void ChangeStateChecks()
    {
        base.ChangeStateChecks();

        if (m_timer >= m_desc.skillDuration)
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
