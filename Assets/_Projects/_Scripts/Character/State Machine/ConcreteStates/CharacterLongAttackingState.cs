using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static Character;

public class CharacterLongAttackingState : CharacterState
{
    [System.Serializable]
    public struct Descriptor
    {
        public float longAttackStrength;
        public float longAttackDuration;
        public float longAttackCooldown;
        public UnityEvent OnLongAttack;
    }

    Descriptor m_desc;
    private float m_timer;
    private float m_lastHitTime;
    private Vector3 m_moveDirection;

    public CharacterLongAttackingState(Character character, CharacterStateMachine characterStateMachine, Descriptor desc) : base(character, characterStateMachine)
    {
        m_desc = desc;
        m_lastHitTime = -Mathf.Infinity;
    }

    public override void EnterState()
    {
        base.EnterState();

        if (Time.time - m_lastHitTime < m_desc.longAttackCooldown)
        {
            character.ChangeCharacterState(character.IdleState);
            return;
        }

        m_desc.OnLongAttack?.Invoke();

        character.PlayerAnimator.SetTrigger("Kick");

        character.LastDesiredSpeed = character.DesiredSpeed;
    }

    public override void ExitState()
    {
        base.ExitState();

        if (Time.time - m_lastHitTime >= m_desc.longAttackCooldown)
        {
            Collider[] colliders = Physics.OverlapSphere(character.gameObject.transform.position, 2f);

            foreach (Collider collider in colliders)
            {
                GameObject obj = collider.gameObject;

                Enemy enemy = obj.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage((int)m_desc.longAttackStrength);
                    m_desc.OnLongAttack?.Invoke();
                }
            }
            m_lastHitTime = Time.time;
        }
    }

    public override void ChangeStateChecks()
    {
        base.ChangeStateChecks();

        if (m_timer >= m_desc.longAttackDuration)
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
