using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hazard : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable other = collision.gameObject.GetComponent<IDamageable>();
        if (other != null)
        {
            other.Damage(collision.relativeVelocity.magnitude);
        }
    }
}
