using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydre : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Animator _animator;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") !=0 )
        {
            Debug.Log("on test");
            _animator.SetTrigger("IsAttack");
        }
    }
}
