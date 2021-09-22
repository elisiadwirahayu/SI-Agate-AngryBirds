using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBird : Bird
{
    [SerializeField]
    public float _boostForce = 100;
    public bool _hasBoost = false;

    // memberikan efek boost  ketika burung sedang terbang
    public void Boost()
    {
        if (State == BirdState.Thrown && !_hasBoost)
        {
            RigidBody.AddForce(RigidBody.velocity * _boostForce);
            _hasBoost = true;
        }
    }

    // ketika burung kuning dilemparkan, kemudian player melakukan tap, maka fungsi Boost akan terpanggil
    public override void OnTap()
    {
        Boost();
    }
}
