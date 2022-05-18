using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerJoyStick : MonoBehaviour
{
    public CharacterController controller;

    [Header("Joystick")]
    public FixedJoystick moveJoystick;
    public FixedJoystick lookJoystick;
    public FixedJoystick FiringJoystick;


    private Animator anim;

    [Header("Joystick")]
    public float moveSpeed;
    public float runSpeed;
    public float fireRate;
    public float nextFire;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;


    [Header("Bullet")]
    public Transform spawnPoint;
    public GameObject bullet;
    public float speed = 5f;

    [Header("Sound")]
    public AudioSource M4;
    public AudioSource WalkSound;

    public LayerMask _aimLayerMask;


    private bool runState;
    private bool shootState;




    [Header("TestRotate")]
    public bool OriginalRotate;
    public bool NewRotate;

    [Header("BulletCharge")]
    public float maxCharge;
    public float curretCharge;
    public bool noCharge;
    public Image barCharge;


    [Header("FightState")]
    public bool combat;
    public bool range;





    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        //  anim.SetFloat("Speed", 0f);


        runState = true;
        shootState = false;



        curretCharge = maxCharge;
        noCharge = true;

        combat = true;
        range = false;








    }

    // Update is called once per frame
    void Update()
    {




        SwordAttack();









        if (curretCharge >= 100f)
        {
            curretCharge = 100f;
        }

        barCharge.fillAmount = curretCharge / 100f;


        curretCharge += 8f * Time.deltaTime;

        //UpdateLookJoystick();

        if (runState == true)
        {
            RunMode();
        }


        if (shootState == true)
        {
            UpdateMoveJoystick();
            UpdateFiringJoystick();


            if (range)
            {
                AimTowardMouse();
            }
        }

        if (Input.GetKey(KeyCode.E))
        {
            combat = false;
            range = true;
        }



        if (Input.GetKey(KeyCode.Q))
        {
            range = false;
            combat = true;

            runState = true;
            shootState = false;

            anim.SetBool("IsShoot", false);

        }



        if (range == true)
        {


            if (noCharge == true)
            {
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    runState = false;
                    shootState = true;

                    curretCharge -= 20f * Time.deltaTime;


                    anim.SetBool("IsShoot", true);





                }
            }




            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                runState = true;
                shootState = false;

                anim.SetBool("IsShoot", false);


            }

        }


    }




    void SwordAttack()
    {
        if (combat)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //Debug.Log("SwingSword");
                anim.SetTrigger("Sword1");
                runState = false;


            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {

                runState = true;


            }
        }


    }

    void RunMode()
    {




        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(hor, 0, ver).normalized;


        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            controller.Move(direction * runSpeed * Time.deltaTime);

            anim.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);


        }

        if (direction.magnitude == 0f)
        {
            anim.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);

        }



    }

    void AimTowardMouse()
    {


        if (OriginalRotate)
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

        if (NewRotate)
        {
            //Rotate object to face mouse.
            Vector3 MouseScreenToCameraSpace = new Vector3(Input.mousePosition.x, 0f, Input.mousePosition.y);
            // Vector3 PlayerScreenToCameraSpace = new Vector3(Camera.main.WorldToScreenPoint(transform.position).x, 0f, Camera.main.WorldToScreenPoint(transform.position).y);
            Vector3 PlayerScreenToCameraSpace = new Vector3(Camera.main.WorldToScreenPoint(spawnPoint.position).x, 0f, Camera.main.WorldToScreenPoint(spawnPoint.position).y);

            Vector3 PlayerToMouse = MouseScreenToCameraSpace - PlayerScreenToCameraSpace;
            //Debug.DrawLine(transform.position, PlayerToMouse);
            Debug.DrawLine(spawnPoint.position, PlayerToMouse);
            transform.LookAt(PlayerToMouse);
        }





        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                ShootBullet();
                M4.Play();



            }



        }



    }


    void UpdateFiringJoystick()
    {

        float hor = FiringJoystick.Horizontal;
        float ver = FiringJoystick.Vertical;

        Vector2 convertedXY = ConvertWithCamera(Camera.main.transform.position, hor, ver);
        Vector3 direction = new Vector3(hor, 0, ver).normalized;
        Vector3 lookAtPosition = transform.position + direction;
        transform.LookAt(lookAtPosition);

        if (FiringJoystick.Horizontal != 0f && FiringJoystick.Vertical != 0f)
        {

            anim.SetBool("FiringAnim", true);
            moveSpeed = 0.002f;

            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                ShootBullet();
                // M4.Play();
            }
        }
        else
        {
            anim.SetBool("FiringAnim", false);
            moveSpeed = 0.005f;
            // M4.Stop();

        }


    }

    private void ShootBullet()
    {

        GameObject cB = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);

        cB.transform.Rotate(Vector3.left * 90);

        Rigidbody rig = cB.GetComponent<Rigidbody>();

        rig.AddForce(spawnPoint.forward * speed, ForceMode.Impulse);




    }

    void UpdateMoveJoystick()
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

    void UpdateLookJoystick()
    {
        float hor = lookJoystick.Horizontal;
        float ver = lookJoystick.Vertical;

        Vector2 convertedXY = ConvertWithCamera(Camera.main.transform.position, hor, ver);
        Vector3 direction = new Vector3(hor, 0, ver).normalized;
        Vector3 lookAtPosition = transform.position + direction;
        transform.LookAt(lookAtPosition);
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
