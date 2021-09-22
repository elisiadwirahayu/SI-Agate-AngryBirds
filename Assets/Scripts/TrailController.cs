using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    public GameObject Trail;
    public Bird TargetBird; // burung yang akan diberi trails
    
    private List<GameObject> _trails; // trail yang ditampilkan dalam game

    void Start()
    {
        // inisiasi atribut _trails
        _trails = new List<GameObject>();
    }

    // menambahkan burung yang akan dijadikan target
    public void SetBird(Bird bird)
    {
        TargetBird = bird;

        for (int i = 0; i < _trails.Count; i++)
        {
            // me-reset ulang trail yang ada
            Destroy(_trails[i].gameObject);
        }

        _trails.Clear();
    }

    // membuat game object trail setiap 100ms
    public IEnumerator SpawnTrail()
    {
        _trails.Add(Instantiate(Trail, TargetBird.transform.position, Quaternion.identity));

        // memberikan delay
        yield return new WaitForSeconds(0.1f);

        if (TargetBird != null && TargetBird.State != Bird.BirdState.HitSomething)
        {
            StartCoroutine(SpawnTrail());
        }
    }
}
