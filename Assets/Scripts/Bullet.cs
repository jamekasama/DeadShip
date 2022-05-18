using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
  
    public float damage = 10f;

    public float speed = 70f;

    public LayerMask layerEnemy;
    public LayerMask layerWall;
    public float radius = 1f;

    public float lifetime = 2.0f;

    public Transform spawnPoint;
    public ParticleSystem impact;


    [Header("WeaponType")]
    public bool ifBullet;
    public bool lightSaber;
    public bool ifShotGun;

 

    public float fireRate;
    public float nextFire;


  
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {

        if (ifShotGun)
        {
            Rigidbody rig = GetComponent<Rigidbody>();

            rig.AddForce(transform.forward * speed, ForceMode.Impulse);


        }





        if (ifBullet)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerEnemy);



            if (hits.Length > 0)
            {
                hits[0].GetComponent<Target>().TakeDamage(damage);
                //hits[0].GetComponent<EnemyController>().SlowEnemy();
                Destroy(gameObject);

                Instantiate(impact, spawnPoint.position, spawnPoint.rotation);
                


            }

            Collider[] hitsWall = Physics.OverlapSphere(transform.position, radius, layerWall);


            if(hitsWall.Length > 0)
            {
                Destroy(gameObject);

                Instantiate(impact, spawnPoint.position, spawnPoint.rotation);
                Debug.Log("hitwall");


            }

            Destroy(gameObject, lifetime);

        }

        if (lightSaber)
        {
            Collider[] hits = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity, layerEnemy);

            if (hits.Length > 0)
            {
                hits[0].GetComponent<Target>().TakeDamage(damage);
                //Destroy(gameObject);

                if (Time.time > nextFire)
                {
                    nextFire = Time.time + fireRate;
                    Instantiate(impact, spawnPoint.position, spawnPoint.rotation);
                }

               // Debug.Log("lightsaberwork");
            }




        }




    }

   
       
    

    

}
