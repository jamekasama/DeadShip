using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    private Animator anim;

    public bool gunPlay;

    [Header("Sound")]
    public AudioSource BG;

    public float increase;
    public float startChange;
    public bool startIncrease;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        BG.Play();

        startChange = 0f;
        startIncrease = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (gunPlay)
        {
            anim.SetBool("gunPlay", true);
        }

        if(gunPlay == false)
        {
            anim.SetBool("gunPlay", false);
        }

        if (startIncrease)
        {
            startChange += increase * Time.deltaTime;
            
        }

        if (startChange >= 1f)
        {
            SceneManager.LoadScene("Level1");
        }
    }

    public void StartGame()
    {
        gunPlay = true;
        startIncrease = true;

       
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
