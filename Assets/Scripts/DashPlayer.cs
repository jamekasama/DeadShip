using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPlayer : MonoBehaviour
{

    NewPlayer moveScript;

    public float dashSpeed;
    public float dashTime;
    public float dashCD;

    private Animator anim;

    public bool canDash;

    public GameObject spawnLightSaber;



    // Start is called before the first frame update
    void Start()
    {
        moveScript = GetComponent<NewPlayer>();
        canDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        anim = GetComponentInChildren<Animator>();

        dashCD -= Time.deltaTime;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            canDash = false;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            canDash = true;
        }


       




        if (canDash)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (dashCD <= 0)
                {
                    anim.SetTrigger("Dash");
                    StartCoroutine(Dash());
                }
            }
        }

    }

    IEnumerator Dash()
    {
        float startTime = Time.time;

        while(Time.time < startTime + dashTime)
        {
            moveScript.controller.Move(moveScript.moveDir * dashSpeed * Time.deltaTime);
         

            dashCD = 0.4f;


            yield return null;
        }
    }

    public void AnimSword()
    {
        Debug.Log("playSwingSwordAnim");
    }


    void Active_AttackPoint()
    {
        spawnLightSaber.SetActive(true);
    }

    void Deactive_AttackPoint()
    {
        if (spawnLightSaber.activeInHierarchy)
        {
            spawnLightSaber.SetActive(false);
        }
    }
}
