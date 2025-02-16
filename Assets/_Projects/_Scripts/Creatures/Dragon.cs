using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dragon : MonoBehaviour
{
    [SerializeField] private UnityEvent OnFireBreath;

    private Animator _animator;

    private const string FIRE_BREATH_TRIGGER = "Fire Breath";

    private void Start()
    {
        _animator = GetComponent<Animator>();

        StartCoroutine(FireBreathRoutine());
    }

    private IEnumerator FireBreathRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);

            _animator.SetTrigger(FIRE_BREATH_TRIGGER);

            yield return new WaitForSeconds(1.9f);

            OnFireBreath?.Invoke();
        }
    }
}
