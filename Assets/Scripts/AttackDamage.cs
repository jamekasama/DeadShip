using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamage : MonoBehaviour
{
    public LayerMask layer;
    public float radius = 1f;
    public float damage = 1f;

    public bool AlienBullet;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layer);

        if(hits.Length > 0)
        {
            hits[0].GetComponent<Healthscript>().ApplyDamage(damage);
            hits[0].GetComponent<NewPlayer>().Hurt();

            if (AlienBullet)
            {
                Destroy(gameObject);
            }
        }

     
    }
}
