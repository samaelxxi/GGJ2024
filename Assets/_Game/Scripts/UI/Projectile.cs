using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Projectile : MonoBehaviour
{
    public float Speed = 1f;
    Vector3 _target;

    public void Init(Vector3 target)
    {
        _target = target;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = _target - transform.position;
        direction.Normalize();
        transform.Translate(direction * Speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, _target) < 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
