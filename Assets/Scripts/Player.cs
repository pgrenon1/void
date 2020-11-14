using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxDistanceToDissolve = 100f;

    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, maxDistanceToDissolve);

        for (int i = 0; i < colliders.Length; i++)
        {
            var dissolvable = colliders[i].GetComponent<Dissolvable>();
            if (dissolvable && !dissolvable.IsDissolvedCompletly)
            {
                var delta = dissolvable.transform.position - transform.position;
                var distanceSqr = delta.sqrMagnitude;

                dissolvable.Dissolve(distanceSqr);
            }
        }
    }
}
