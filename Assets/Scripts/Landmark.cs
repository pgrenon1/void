using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmark : MonoBehaviour
{
    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= 3f)
        {
            Destroy(this.GetComponent<Rigidbody>());
            Destroy(this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Dissolvable>())
        {
            Destroy(collision.gameObject);
        }
    }
}
