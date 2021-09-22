using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bird : MonoBehaviour
{
    public enum BirdState { Idle, Thrown, HitSomething }
    // public GameObject Parent;
    public Rigidbody2D RigidBody;
    public CircleCollider2D Collider;

    public UnityAction OnBirdDestroyed = delegate { };
    public UnityAction<Bird> OnBirdShot = delegate { };

    public BirdState State { get { return _state; } }

    private BirdState _state;
    private float _minVelocity = 0.05f;
    private bool _flagDestroy = false;

    void Start() // mematikan fungsi physics dan collider dari burung
    {
        RigidBody.bodyType = RigidbodyType2D.Kinematic;
        Collider.enabled = false;
        _state = BirdState.Idle;
    }

    private IEnumerator DestroyAfter(float second)
    {
        yield return new WaitForSeconds(second);
        Destroy(gameObject);
    }

    void OnDestroy() // memberitahu jika object Bird akan dihancurkan
    {
        if (_state == BirdState.Thrown || _state == BirdState.HitSomething)
            OnBirdDestroyed();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        _state = BirdState.HitSomething;
    }

    void FixedUpdate()
    {
        if (_state == BirdState.Idle &&
            RigidBody.velocity.sqrMagnitude >= _minVelocity)
        {
            // mengubah state dari burung menjadi Thrown
            _state = BirdState.Thrown;
        }

        if ((_state == BirdState.Thrown || _state == BirdState.HitSomething) &&
            RigidBody.velocity.sqrMagnitude < _minVelocity &&
            !_flagDestroy)
        {
            // menghancurkan game object  burung tersebut setelah 2 detik
            // jika kecepatannya sudah kurang dari batas minimum
            _flagDestroy = true;
            StartCoroutine(DestroyAfter(2));
        }

    }

    public void MoveTo(Vector2 target, GameObject parent) // menginisiasi posisi dan mengubah parent dari game object burung
    {
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.position = target;
    }

    public void Shoot(Vector2 velocity, float distance, float speed) // melemparkan burung dengan arah, jarak tali yang ditarik, dan kecepatan awal
    {
        Collider.enabled = true;
        RigidBody.bodyType = RigidbodyType2D.Dynamic;
        RigidBody.velocity = velocity * speed * distance;
        OnBirdShot(this); // memberikan tanda bahwa trail sudah dapat di-spawn
    }


    // berfungsi pada burung berwarna kuning
    public virtual void OnTap()
    {
        //Do nothing
    }
}
