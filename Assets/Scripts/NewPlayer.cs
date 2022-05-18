using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class NewPlayer : MonoBehaviour
{
    public CharacterController controller;

 

    [Header("CHR")]
    public float moveSpeed;
    public float runSpeed;
    public float fireRate;
    private float nextFire;
    public float fireRateGun;
    private float nextFireGun;
  
    private float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    [Header("DelayShotGun")]
    public float canFireShot;
    public float increaseShot;
    public float decreaseShot;

    [Header("GunType")]
    public bool rifle;
    public bool shotGun;
    public Image AmmoBar;
    




    [Header("Mag")]
    public int maxMag;
    public int maxMagShot;

    [Header("RifleAmmo")]
    public float maxAmmo;
    public float currentAmmo;
    public float decreaseAmmo;
    public float increaseAmmo;
    public bool reloadRifle;

    [Header("ShotGunAmmo")]
    public float maxAmmoShot;
    public float currentAmmoShot;
    public float decreaseAmmoShot;
    public float increaseAmmoShot;
    public bool reloadShot;

 


    [Header("Reload")]
    public float fireRateReload;
    private float nextReload;
    public float fireRateChange;
    private float nextChange;

    public Text ammoText;
    public Text ammoShotText;
    public bool notReloadRifle;
    public bool notReloadShot;





    [Header("SlowPlayer")]
    public float maxSlow;
    public float curretSlow;
    public float increaseSlow;
    public float decreaseSlow;




    [Header("SwingSword")]
    private bool CanRun;

    [Header("Bullet")]
    public Transform spawnPoint;
    public GameObject bullet;
    public float speed = 5f;

    [Header("BulletShotGun")]
    public Transform spawnShot1;
    public Transform spawnShot2;
    public Transform spawnShot3;
    public GameObject bulletShot;
    public float speedShot = 5f;


    [Header("WeaponState")]
    private bool swordState;
    private bool GunState;

    [Header("AnimReload")]
    public bool reload;

   
    public Vector3 moveDir;




    [Header("Sound")]
    public AudioSource M4;
    public AudioSource LS;
    public AudioSource ReloadSound;









    [Header("DelayShoot")]
    private float delayButton = 0;
    public float delayincrease;

    public LayerMask _aimLayerMask;

    private Animator anim;









    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();


        rifle = true;
        CanRun = true;
        GunState = true;
        notReloadRifle = true;
        notReloadShot = true;

        delayButton = 0f;
       

        


    }

    void FixedUpdate()
    {

      



    }


    void Update() // Update is called once per frame
    {
        Mouse1();

        Mouse0();

        ChangeGun();

        Reload();


        if (canFireShot >= 1f)
        {
            canFireShot = 1f;
        }

        canFireShot += increaseShot * Time.deltaTime; 

        if (curretSlow >= maxSlow)
        {
            curretSlow = maxSlow;
           
        }

        curretSlow += increaseSlow * Time.deltaTime; 
   

        if(delayButton >= 1f)
        {
            delayButton = 1f;
        }


        

        if (Time.time > nextFire)
        {
            CanRun = true;
        }

      


       // if (Time.time > nextReload)
      //  {
           // notReload = true;
      //  }








        } // Update is called once per frame
    void ChangeGun()
    {
        
            if (reloadRifle)
            {
                currentAmmo += increaseAmmo * Time.deltaTime;
            


             }



            if (reloadShot)
            {
                currentAmmoShot += increaseAmmoShot * Time.deltaTime;
          

             }
        
        

        if (currentAmmo >= maxAmmo)
        {
            
            currentAmmo = maxAmmo;
            reloadRifle = false;
            notReloadRifle = true;
        }

        if (currentAmmoShot >= maxAmmoShot)
        {
            currentAmmoShot = maxAmmoShot;
            reloadShot = false;
            notReloadShot = true;
        }



        if (rifle)
        {
            // ammoText.text = currentAmmo.ToString(("F0"));
            if (AmmoBar != null)
                AmmoBar.fillAmount = currentAmmo / 100f;
        }

        if (shotGun)
        {
            // ammoShotText.text = currentAmmoShot.ToString(("F0"));
            if (AmmoBar != null)
                AmmoBar.fillAmount = currentAmmoShot / 100f;
        }

        if (notReloadRifle == true || notReloadShot == true)
        {


            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                rifle = true;
                shotGun = false;
                notReloadRifle = true;

            }
        }




        if (notReloadRifle == true || notReloadShot == true)
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                rifle = false;
                shotGun = true;
                notReloadShot = true;

            }
        }
        
       

    }

    void Mouse1()
    {
        if (CanRun == true)
        {
            RunWithSword();

            if (curretSlow >= maxSlow)
            {
                runSpeed = 4f;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            swordState = true;
            GunState = false;

            if (swordState == true)
            {
                if (Time.time > nextFire)
                {

                    nextFire = Time.time + fireRate;
                    CanRun = false;
                    anim.SetTrigger("Sword1");

                    LS.Play();

                    GetComponent<DashPlayer>().AnimSword();


                }
            }

        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            swordState = false;
            GunState = true;
        }
    }

    void Mouse0()
    {

       
            if (Input.GetKey(KeyCode.Mouse0))
            {
                GunState = true;


                delayButton += delayincrease * Time.deltaTime;


                if (GunState == true)
                {
                    AimTowardMouse();



                    CanRun = false;

                    RunWithGun();

                    anim.SetBool("IsShoot", true);
                    runSpeed = 0.5f;


                if (notReloadRifle)
                {
                    if (rifle)
                    {
                        if (Time.time > nextFireGun)
                        {
                            nextFireGun = Time.time + fireRateGun;

                            if (delayButton >= 1f)
                            {
                                if (currentAmmo >= 0f)
                                {

                                    ShootBullet();
                                    currentAmmo -= decreaseAmmo * Time.deltaTime;
                                    M4.Play();

                                }
                            }
                        }

                    }
                }


                if (notReloadShot)
                {
                    if (shotGun)
                    {
                        //delay
                        if(canFireShot >= 1f)    //if (Time.time > nextFireShot)
                        {
//                              nextFireShot = Time.time + fireRateShot;

                            if (delayButton >= 1f)
                            {
                                if (currentAmmoShot >= 0f)
                                {
                                        canFireShot -= decreaseShot * Time.deltaTime;
                                    ShootShotgun();
                                        currentAmmoShot -= decreaseAmmoShot * Time.deltaTime;
                                        M4.Play();
                                    
                                }
                            }
                        }
                    }
                }





                }

            
        }

    
    

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
           


            CanRun = true;
            anim.SetBool("IsShoot", false);
            if (curretSlow >= maxSlow)
            {
                runSpeed = 4f;
            }

            GunState = false;
            swordState = true;

            delayButton = 0f;

            //notReload = true;

          

        }

    

    }

    void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            notReloadRifle = false;
            notReloadShot = false;

            if (Time.time > nextReload)
            {
                nextReload = Time.time + fireRateReload;


                ReloadSound.Play();


                maxMag -= 1;
                maxMagShot -= 1;

                if (rifle)
                {
                    if (currentAmmo <= maxAmmo)
                    {
                        //currentAmmo += maxAmmo;
                        reloadRifle = true;
                        currentAmmo = 0f;
                      
                        

                    }
                }

                if (shotGun)
                {
                    if (currentAmmoShot <= maxAmmoShot)
                    {
                        //currentAmmoShot += maxAmmoShot;

                        reloadShot = true;
                        currentAmmoShot = 0f;
                       

                    }
                }
            }
        }
    }

    

    void AttackSword()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                CanRun = false;
                anim.SetTrigger("Sword1");


            }

        }



    }


    public void Hurt()
    {
       

        if (curretSlow <= 99f)
        {
            runSpeed = 2f;
        }
        curretSlow -= decreaseSlow *Time.deltaTime;

      

    }



    public void RunWithSword()   //NORMAL MOVEMENT 
    {
       
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(hor, 0, ver).normalized;


        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(direction * runSpeed * Time.deltaTime);

         

            anim.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);


        }

        if (direction.magnitude == 0f)
        {
            anim.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);

        }


        
    } //NORMAL MOVEMENT 


   
    void AimTowardMouse()
    {


       

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, _aimLayerMask))
            {

                var _direction = hitInfo.point - transform.position;
                //var _direction = hitInfo.point - spawnPoint.position;
                _direction.y = 0f;

                _direction.Normalize();
                transform.forward = _direction;
                Debug.DrawLine(transform.position, hitInfo.point);


             }
       
    }

    private void ShootShotgun()
    {
        GameObject sB1 = Instantiate(bulletShot, spawnShot1.position, spawnShot1.rotation);
        GameObject sB2 = Instantiate(bulletShot, spawnShot2.position, spawnShot2.rotation);
        GameObject sB3 = Instantiate(bulletShot, spawnShot3.position, spawnShot3.rotation);
    }

    private void ShootBullet()
    {

        GameObject cB = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);

        cB.transform.Rotate(Vector3.left * 90);

        Rigidbody rig = cB.GetComponent<Rigidbody>();

        rig.AddForce(spawnPoint.forward * speed, ForceMode.Impulse);




    }


    void RunWithGun()
    {



        //float hor = moveJoystick.Horizontal;
        // float ver = moveJoystick.Vertical;
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");
        Vector2 convertedXY = ConvertWithCamera(Camera.main.transform.position, hor, ver);
        Vector3 direction = new Vector3(hor, 0, ver).normalized;

        //new animation

        float velocityZ = Vector3.Dot(direction.normalized, transform.forward);
        float velocityX = Vector3.Dot(direction.normalized, transform.right);



        if (direction.magnitude >= 0.1f)
        {
            direction.Normalize();
            direction *= moveSpeed * Time.deltaTime;
            transform.Translate(direction * moveSpeed, Space.World);
            

        }

        if (direction != Vector3.zero)
        {
            //WalkSound.Play();
        }




        //new animation


        anim.SetFloat("VelocityZ", velocityZ, 0.2f, Time.deltaTime);
        anim.SetFloat("VelocityX", velocityX, 0.2f, Time.deltaTime);





    }












    private Vector2 ConvertWithCamera(Vector3 cameraPos, float hor, float ver)
    {
        Vector2 joyDirection = new Vector2(hor, ver).normalized;
        Vector2 Camera2DPos = new Vector2(cameraPos.x, cameraPos.z);
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 cameraToPlayerDirection = (Vector2.zero - Camera2DPos).normalized;
        float angle = Vector2.SignedAngle(cameraToPlayerDirection, new Vector2(0, 1));
        Vector2 finalDirection = RotateVector(joyDirection, -angle);
        return finalDirection;
    }

    public Vector2 RotateVector(Vector2 v, float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        float _x = v.x * Mathf.Cos(radian) - v.y * Mathf.Sin(radian);
        float _y = v.x * Mathf.Sin(radian) - v.y * Mathf.Cos(radian);
        return new Vector2(_x, _y);
    }
  
}
