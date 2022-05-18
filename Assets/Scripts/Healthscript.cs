using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Healthscript : MonoBehaviour
{
    public float health = 100f;

    public Image HealthBar;

    public bool Immortal;


    public void ApplyDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {

            print("characterDie");
            //Destroy(gameObject);
            SceneManager.LoadScene("Lose");


        }
    }



    // Start is called before the first frame update
    void Start()
    {
        Immortal = false;
    }

    // Update is called once per frame
    void Update()
    {

        if(HealthBar != null) 
        HealthBar.fillAmount = health / 100f;
        

        if (Input.GetKeyDown(KeyCode.B))
        {
            Immortal = true;
        }

        if (Immortal)
        {
            health = 100f;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            Immortal = false;
        }



    }
}
