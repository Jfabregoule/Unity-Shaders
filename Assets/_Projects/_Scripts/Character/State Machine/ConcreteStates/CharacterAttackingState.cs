using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.VFX;
using static Character;

public class CharacterAttackingState : CharacterState
{
    [System.Serializable]
    public class Descriptor
    {
        public float attackStrength;
        public float attackCooldown;
        public GameObject _distortion;

        public float attackDuration;
        public UnityEvent OnAttack;
    }

    Descriptor m_desc;
    private float m_timer;
    private float m_lastHitTime;
    private Vector3 m_moveDirection;

    bool m_heavyAttack;

    public CharacterAttackingState(Character character, CharacterStateMachine characterStateMachine, Descriptor desc) : base(character, characterStateMachine)
    {
        m_desc = desc;
        m_lastHitTime = -Mathf.Infinity;
    }

    public override void EnterState()
    {
        base.EnterState();

        if (Time.time - m_lastHitTime < m_desc.attackCooldown)
        { 
            character.ChangeCharacterState(character.IdleState);
            return;
        }

        m_heavyAttack = false;

        character.Input.OnAttack += OnAttack;

        character.LastDesiredSpeed = character.DesiredSpeed;

        character.PlayerAnimator.SetTrigger("Attack");

        m_timer = 0f;
    }

    public override void ExitState()
    {
        base.ExitState();

        character.Input.OnAttack -= OnAttack;


        if (Time.time - m_lastHitTime >= m_desc.attackCooldown)
        {
            Collider[] colliders = Physics.OverlapSphere(character.gameObject.transform.position, 2f);

            foreach (Collider collider in colliders)
            {
                GameObject obj = collider.gameObject;

                Enemy enemy = obj.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage((int)m_desc.attackStrength);
                    m_desc.OnAttack?.Invoke();
                }
            }
            m_lastHitTime = Time.time;
        }
    }

    public override void ChangeStateChecks()
    {
        base.ChangeStateChecks();

        if (m_timer >= m_desc.attackDuration)
        {
            m_timer = 0f;
            if (m_heavyAttack)
                character.ChangeCharacterState(character.LongAttackingState);
            else
                character.ChangeCharacterState(character.IdleState);
        }
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        m_timer += Time.deltaTime;
    }

    private void OnAttack()
    {
        m_heavyAttack = true;
    }
}
