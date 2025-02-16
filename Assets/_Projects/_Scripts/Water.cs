using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private AudioLowPassFilter _lowPassFilter;

    [SerializeField] private float _cuttOffFrequencyAboveWater = 22000f;
    [SerializeField] private float _cuttOffFrequencyUnderWater = 500f;

    private void Start()
    {
        _lowPassFilter = GameManager.Instance.MainCamera.GetComponent<AudioLowPassFilter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerEars"))
            _lowPassFilter.cutoffFrequency = _cuttOffFrequencyUnderWater;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerEars"))
            _lowPassFilter.cutoffFrequency = _cuttOffFrequencyAboveWater;
    }
}
