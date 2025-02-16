using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Money : MonoBehaviour
{
    [SerializeField] UnityEvent _onPick;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Character>(out Character characterComponent))
        {
            _onPick.Invoke();
            Destroy(gameObject, 3f);
        }
    }
}
