﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    

    public float health = 50f;
   


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float amount)
    {
        Debug.Log("hitEnemy");
        health -= amount;
        if(health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        //Destroy(gameObject);
        GetComponent<EnemyController>().Death();
        
    }
}
