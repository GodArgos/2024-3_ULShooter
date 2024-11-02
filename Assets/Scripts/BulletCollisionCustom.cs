using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollisionCustom : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject bullet;
    private float maxDistance = 200f;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float distanceTraveled = Vector3.Distance(startPosition, transform.position);
        if (distanceTraveled >= maxDistance)
        {
            Destroy(bullet);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth health = other.gameObject.GetComponent<EnemyHealth>();
            if(health != null){
                health.TakeDamage();
                Destroy(bullet);
            }
        }
    }
}
