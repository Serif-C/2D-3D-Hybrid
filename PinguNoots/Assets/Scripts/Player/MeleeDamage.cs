using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    private SphereCollider sphereCollider;

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();

        sphereCollider.radius = 0.5f;

        StartCoroutine(DestroyAfterSeconds(0.01f));
    }

    public void OnTriggerEnter(Collider other)
    {
        EnemyAbs enemy = other.gameObject.GetComponent<EnemyAbs>();

        if (other != null && other.gameObject.GetComponent<EnemyAbs>())
        {
            enemy.TakeDamage(15f);
            Destroy(gameObject);
        }
    }

    IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        //Debug.Log("Waited " + seconds + " seconds before being destroyed");

        Destroy(gameObject);
    }
}
