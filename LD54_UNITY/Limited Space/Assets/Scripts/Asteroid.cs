using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Hazard
{
    protected override DamageSource damageSource => DamageSource.Asteroid;
}
