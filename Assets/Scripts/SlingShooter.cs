using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShooter : MonoBehaviour
{
    public CircleCollider2D Collider;
    public LineRenderer Trajectory;

    private Vector2 _startPos; // menyimpan titik awal sebelum karet ketapel ditarik

    [SerializeField]
    private float _radius = 0.75f; // radius/panjang maksimal dari tali ditarik

    [SerializeField]
    private float _throwSpeed = 30f; // kecepatan awal yang diberikan ketapel pada saat melontarkan burung nantinya

    private Bird _bird;

    void Start()
    {
        _startPos = transform.position;
    }

    void OnMouseUp() // burung akan dilemparkan dengan arah (velocity) dan panjangan tarikan ketapel beserta dengan kecepatan awal, kemudian mengembalikan posisi tali pelontar ke posisi awal
    {
        Collider.enabled = false;
        Vector2 velocity = _startPos - (Vector2)transform.position;
        float distance = Vector2.Distance(_startPos, transform.position);

        _bird.Shoot(velocity, distance, _throwSpeed);

        //Kembalikan ketapel ke posisi awal
        gameObject.transform.position = _startPos;
        Trajectory.enabled = false;

    }

    void OnMouseDrag() // mengubah posisi dari game object ShooterArea agar mengikuti gerakan mouse, tetapi gerakan tersebut akan terbatas pada radius yang ditetapkan
    {
        // Mengubah posisi mouse ke world position
        Vector2 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Hitung supaya 'karet' ketapel berada dalam radius yang ditentukan
        Vector2 dir = p - _startPos;

        if (dir.sqrMagnitude > _radius)
            dir = dir.normalized * _radius;

        transform.position = _startPos + dir;

        float distance = Vector2.Distance(_startPos, transform.position);

        if (!Trajectory.enabled)
        {
            Trajectory.enabled = true;
        }

        DisplayTrajectory(distance);
    }

    // memprediksikan posisi burung dengan rumus di atas, kemudian menggambarkannya dengan menggunakan LineRenderer
    void DisplayTrajectory(float distance)
    {
        if (_bird == null)
        {
            return;
        }

        Vector2 velocity = _startPos - (Vector2)transform.position;
        // total point/titik yang akan digambarkan ke dalam trajectory
        int segmentCount = 5;
        Vector2[] segments = new Vector2[segmentCount];

        // Posisi awal trajectoy merupakan posisi mouse dari player saat ini
        segments[0] = transform.position;

        // Velocity awal
        Vector2 segVelocity = velocity * _throwSpeed * distance;

        for (int i = 1; i < segmentCount; i++)
        {
            float elapsedTime = i * Time.fixedDeltaTime * 5;
            segments[i] = segments[0] + segVelocity * elapsedTime + 0.5f * Physics2D.gravity * Mathf.Pow(elapsedTime, 2);
        }

        Trajectory.positionCount = segmentCount;
        for (int i = 0; i < segmentCount; i++)
        {
            Trajectory.SetPosition(i, segments[i]);
        }
    }
    
    public void InitiateBird(Bird bird)
    {
        _bird = bird;
        _bird.MoveTo(gameObject.transform.position, gameObject);
        Collider.enabled = true;
    }

}