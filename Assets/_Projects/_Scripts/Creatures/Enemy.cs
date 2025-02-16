using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class Enemy : MonoBehaviour
{
    private Animator _animator;
    private float _timeAttack;
    private int _life;
    private bool _isActivate = false;
    private Transform _playerTransform;
    [SerializeField] UnityEvent _isSleeping;
    [SerializeField] UnityEvent _isWakeUp;
    [SerializeField] UnityEvent _isHit;
    [SerializeField] UnityEvent _isAttacking;
    [SerializeField] UnityEvent _isBaseAttacking;
    [SerializeField] GameObject _prefabOrb;
    [SerializeField] GameObject _shootVFX;
    [SerializeField] GameObject _hitVFX;
    // Start is called before the first frame update
    void Start()
    {
        _timeAttack = 0;
        _life = 100;
        _animator = GetComponent<Animator>();
        _isSleeping.Invoke();
        _playerTransform = GameManager.Instance.Player.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        _timeAttack += Time.deltaTime;
        if (_timeAttack > 10f) {
            _timeAttack = 0;
            if (_isActivate && _life > 0)
            {
                _animator.SetTrigger("IsAttacking");
                _isBaseAttacking.Invoke();
                GameObject orb = Instantiate(_prefabOrb, _shootVFX.transform.position, _shootVFX.transform.rotation, null);
            }
        }
        if (_life == 0) {
            _animator.SetTrigger("IsDead");
        }
        if (_isActivate && _life > 0) {
            RotateEnemy();
        }
    }

   

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" || other.tag == "Collide")
        {
            _isWakeUp.Invoke();
            _animator.SetTrigger("IsWakeUp");
            _isActivate = true;
        }
        if(other.tag == "Collide")
        {
            _isHit.Invoke();
            _animator.SetTrigger("IsHit");
            SoundManager.Instance.PlayAllSoundFXClipsByKey("Enemy Hit", transform.position, 0.6f);
            Vector3 spawnPosition = other.transform.position;

            
            GameObject hitVFX = Instantiate(_hitVFX, spawnPosition + Vector3.up * 1.5f, Quaternion.identity);
            Destroy(hitVFX,2f);
            _life -= 5;
        }
    }

     
    public void RotateEnemy()
    {
        
        Vector3 directionToPlayer = (_playerTransform.position - gameObject.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookRotation, 2.0f * Time.deltaTime);
        
    }

    public void TakeDamage(int damage)
    {
        SoundManager.Instance.PlayAllSoundFXClipsByKey("Enemy Hit", transform.position, 0.6f);
        _animator.SetTrigger("IsWakeUp");
        _isHit.Invoke();
        _animator.SetTrigger("IsHit");
        _life -= damage;
        if( _life <= 0 )
        {
            SoundManager.Instance.PlayAllSoundFXClipsByKey("Enemy Dead", transform.position, 0.6f);
            _life = 0;
        }
    }
}
