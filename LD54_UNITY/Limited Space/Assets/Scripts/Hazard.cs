using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hazard : MonoBehaviour
{

    protected virtual DamageSource damageSource => DamageSource.None;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable other = collision.gameObject.GetComponent<IDamageable>();
        if (other != null)
        {
            other.Damage(collision.relativeVelocity.magnitude, damageSource);
        }
    }
}
public enum DamageSource
{
    None,
    Asteroid,
    Pirate,
    Tumbleweed,
    Container,
    Player
}
