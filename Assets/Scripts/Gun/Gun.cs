using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Gun : MonoBehaviour
{
    private float coolDownTime = 1f;
    [SerializeField]
    private float range;
    private float lastShotTime;
    public float shootForce;
    public Transform attackPoint;
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
        if (lastShotTime + coolDownTime < Time.time)
        {
            muzzle.Play();
            src.Play();

            RaycastHit hit;
            Vector3 targetPoint;

            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                GameObject enemy = hit.transform.gameObject;
                if (enemy.CompareTag("Enemy"))
                {
                    enemy.GetComponent<EnemyHealth>().TakeDamage();
                }
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = fpsCam.transform.position + fpsCam.transform.forward * range;
            }

            TrailRenderer trail = Instantiate(bulletTrail, attackPoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, targetPoint));
            
            lastShotTime = Time.time;
            //muertesContador += currentBullet.GetComponent<BulletCollisionCustom>().muertesContador;
            //muertestext.text=muertesContador.ToString();
        }
    }

    IEnumerator SpawnTrail(TrailRenderer trail, Vector3 targetPoint)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, targetPoint, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }
        
        trail.transform.position = targetPoint;
        
        Destroy(trail.gameObject, trail.time);
    }

}
