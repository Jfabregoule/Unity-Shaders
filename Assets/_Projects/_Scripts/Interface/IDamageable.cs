using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IDamageable
{
    public event System.Action OnDamage;
    public event System.Action OnDeath;

    float Health { get; set; }
    float MaxHealth { get; }

    void TakeDamage(float amount);

    void Heal(float amount);

    bool IsDead();
}
