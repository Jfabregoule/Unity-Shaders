using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MoveOrb : MonoBehaviour
{
    [SerializeField] float _speed = 0;
    [SerializeField] LayerMask _collide;
    [SerializeField] GameObject _explosion;

    private Vector3 _direction;

    void Start()
    {
        Transform player = GameManager.Instance.Player.transform;

        Vector3 target = player.transform.position;

        target.y += 1;

        _direction = (target - transform.position).normalized;

        GetComponent<VisualEffect>().Play();
        Destroy(gameObject, 10f);
        StartCoroutine(WaitSpeed());
    }

    void Update()
    {
        transform.position += _direction * Time.deltaTime * _speed;
    }

    IEnumerator WaitSpeed()
    {
        yield return new WaitForSeconds(0.8f);

        _speed = 5;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Helpers.IsInLayerMask(other.gameObject, _collide))
        {
            GameObject explosion = Instantiate(_explosion,transform.position,Quaternion.identity,null);
            Destroy(explosion, 2f);
            Destroy(gameObject);
        }
    }
}
