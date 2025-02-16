using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] ShockWave _shockWave;
    [SerializeField] GameObject _tornado;

    public void StartTimer()
    {
        _shockWave.StartTimer();
    }

    public void EnableTornado()
    {
        
    }

    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
