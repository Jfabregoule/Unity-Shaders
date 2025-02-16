using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Dummy : MonoBehaviour, IDamageable
{
    private Animator _animator;

    [SerializeField] private float _maxHealth;

    [SerializeField] private UnityEvent onDamage;
    [SerializeField] private UnityEvent onDeath;

    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    public float Health { get; set; }
    public float MaxHealth { get; private set; }

    public event System.Action OnDamage;
    public event System.Action OnDeath;

    const string HEALTH_PARAM = "Health";
    const string HIT_PARAM = "Hit";

    private void Awake()
    {
        OnDamage += () => onDamage?.Invoke();
        OnDeath += () => onDeath?.Invoke();
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        MaxHealth = _maxHealth;
        Health = MaxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Weapon>(out Weapon weapon))
        {
            TakeDamage(weapon.Damage);
        }
        StartCoroutine(nameof(DamageColor));
    }

    public void TakeDamage(float damage)
    {
        if (Health - damage < 0)
        {
            Health = 0;
            OnDeath.Invoke();
        }
        else
            Health -= damage;

        OnDamage.Invoke();

        _animator.SetTrigger(HIT_PARAM);
        _animator.SetFloat(HEALTH_PARAM, Health);
    }

    public void Heal(float amount)
    {
        Health += amount;
        if (Health > MaxHealth)
            Health = MaxHealth;
    }

    public bool IsDead() => Health <= 0;

    IEnumerator DamageColor()
    {
        meshRenderer.material.SetFloat("_Effect", 1f);
        yield return new WaitForSeconds(3);
        meshRenderer.material.SetFloat("_Effect", 0f);
    }
}
