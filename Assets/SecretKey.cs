using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretKey : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _door;

    public void Interact()
    {
        _door.SetActive(false);
        Destroy(gameObject);
    }
}
