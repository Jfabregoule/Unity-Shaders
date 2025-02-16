using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    [SerializeField] MeshRenderer _material;

    public void StartTimer()
    {
        _material.sharedMaterial.SetFloat("_StartTime", Time.time);
    }
}
