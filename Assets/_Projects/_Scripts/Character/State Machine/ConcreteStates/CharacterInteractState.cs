using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static Character;

public class CharacterInteractState : CharacterState
{
    [System.Serializable]
    public struct Descriptor
    {
        public float interactRange;
        public Transform interactTarget;
        public LayerMask interactDetectionLayer;
        public float interactDuration;
        public UnityEvent OnInteract;
    }

    Descriptor m_desc;
    private float m_timer;
    private Vector3 m_moveDirection;

    public CharacterInteractState(Character character, CharacterStateMachine characterStateMachine, Descriptor desc) : base(character, characterStateMachine)
    {
        m_desc = desc;
    }

    public override void EnterState()
    {
        base.EnterState();

        m_desc.OnInteract?.Invoke();

        Collider[] colliders = Physics.OverlapSphere(m_desc.interactTarget.position, m_desc.interactRange, m_desc.interactDetectionLayer);

        foreach (Collider collider in colliders)
        {
            GameObject obj = collider.gameObject;

            IInteractable interactable = obj.GetComponent<IInteractable>();
            if (interactable != null)
                interactable.Interact();
        }

        //character.PlayerAnimator.SetTrigger("Interact");

        character.LastDesiredSpeed = character.DesiredSpeed;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void ChangeStateChecks()
    {
        base.ChangeStateChecks();

        if (m_timer >= m_desc.interactDuration)
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
