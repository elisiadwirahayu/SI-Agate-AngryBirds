using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public float Health = 50f;

    public UnityAction<GameObject> OnEnemyDestroyed = delegate { };

    private bool _isHit = false;

    void OnDestroy()
    {
        if (_isHit)
        {
            OnEnemyDestroyed(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<Rigidbody2D>() == null) return;

        // Jika game object asing memiliki tag Bird, maka Enemy akan segara mati/hancur
        if (col.gameObject.tag == "Bird")
        {
            _isHit = true;
            Destroy(gameObject);
        }
        else if (col.gameObject.tag == "Obstacle")
        {
            // Hitung damage yang diperoleh
            float damage = col.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10;

            // mengurangi health dari enemy
            Health -= damage;

            // jika health enemy kurang dari 0
            if (Health <= 0)
            {
                _isHit = true;
                Destroy(gameObject); // enemy akan mati
            }
        }
    }
}
