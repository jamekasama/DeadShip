using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnim : MonoBehaviour
{

    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CameraPlay()
    {
        anim.SetBool("CameraPlay", true);
    }
}
