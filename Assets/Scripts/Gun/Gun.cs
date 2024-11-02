using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private float coolDownTime = 1f;
    private float lastShotTime;
    public float shootForce;
    public Transform attackPoint;
    public GameObject bullet;
    public ParticleSystem muzzle;

    private AudioSource src;

    public Camera fpsCam;

    [SerializeField]
    private TrailRenderer bulletTrail;

    void Start()
    {
        muzzle.Stop();
        src = GetComponent<AudioSource>();
    }


    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        //RaycastHit hit;
        if(lastShotTime + coolDownTime < Time.time)
        {
            muzzle.Play();
            src.Play();
            Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit))
            {
                targetPoint = hit.point;
            }else{
                targetPoint = ray.GetPoint(75);  
            }

            Vector3 direction = targetPoint - attackPoint.position; 

            GameObject currentBullet = Instantiate(bullet, attackPoint.position, attackPoint.transform.rotation);
            TrailRenderer trail = Instantiate(bulletTrail, attackPoint.position, attackPoint.transform.rotation);
            StartCoroutine(SpawnTrail(trail, hit));
            currentBullet.transform.forward = direction.normalized;

            currentBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * shootForce, ForceMode.Impulse);
            lastShotTime = Time.time;
        }
    }

    IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while(time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }
        trail.transform.position = hit.point;
        Destroy(trail.gameObject, trail.time);
    }

}
