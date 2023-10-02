using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tumbleweed : Hazard
{
    protected override DamageSource damageSource => DamageSource.Tumbleweed;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable other = collision.gameObject.GetComponent<IDamageable>();
        if (other != null)
        {
            other.Damage(0, damageSource);
        }
    }
}
