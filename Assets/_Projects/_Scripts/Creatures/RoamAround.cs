using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamAround : MonoBehaviour
{
    public Transform centerPoint;
    public float rotationSpeed = 10f;
    public float radius = 5f;

    private float currentAngle = 0f;

    void Start()
    {
        if (centerPoint == null)
        {
            Debug.LogError("CenterPoint n'est pas assigné !");
        }
    }

    void Update()
    {
        if (centerPoint != null)
        {
            currentAngle += rotationSpeed * Time.deltaTime;

            float x = Mathf.Cos(currentAngle) * radius;
            float z = Mathf.Sin(currentAngle) * radius;

            transform.LookAt(new Vector3(centerPoint.position.x + x, transform.position.y, centerPoint.position.z + z));

            transform.position = new Vector3(centerPoint.position.x + x, transform.position.y, centerPoint.position.z + z);
        }
    }
}
