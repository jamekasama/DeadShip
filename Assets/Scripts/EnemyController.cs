using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    CHASE,
    ATTACK,
    DIE
}

public class EnemyController : MonoBehaviour
{

    [Header("EnemyShoot")]
    public bool combatEnemy;
    public bool rangeEnemy;
    public float fireRate;
    private float nextFire;
    public Transform spawnPoint;
    public GameObject bullet;
    public float speed = 5f;
   
    public Transform lookPoint;





    [Header("SlowPlayer")]
    public float maxSlow;
    public float curretSlow;
    public float increaseSlow;
    public float decreaseSlow;
    public float move_Slow;
    

    private NavMeshAgent navAgent;

    private Transform playerTarget;

    public float move_Speed = 3.5f;
    public float move_Attack = 3.5f;

    public float attack_Distance = 1f;
    public float attack_Real = 1f;

    public float chase_Player_After_Attack_Distance = 1f;

    public float wait_Before_Attack_Time = 3f;
    private float attack_Timer;

    private EnemyState enemy_State;
    public GameObject attackPoint;

    private GameObject[] multipleEnemys;
    public Transform cloestEnemy;

    private Animator anim;

    public LayerMask layer;
    public float radius = 1f;

    public bool canSeePlayer;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float angle = 90;

    


    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        navAgent = GetComponent<NavMeshAgent>();

        navAgent.isStopped = false;

    }

    void Start()
    {

       
        attack_Timer = wait_Before_Attack_Time;

        cloestEnemy = null;

        canSeePlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (curretSlow >= maxSlow)
        {
            curretSlow = maxSlow;
        }

        curretSlow += increaseSlow * Time.deltaTime;

       
        EnviromentView();

        cloestEnemy = getCloestEnemy();

        

        if (canSeePlayer == true)
        {
            if (enemy_State == EnemyState.CHASE)
            {
                ChasePlayer();
            }

            if (enemy_State == EnemyState.ATTACK)
            {
                AttackPlayer();
            }

          


        }



        if (enemy_State == EnemyState.DIE)
        {
            DieAnim();
        }

        
     

    }

  public void SlowEnemy()
    {
        if (curretSlow <= 99f)
        {
            navAgent.speed = move_Slow;
        }
        curretSlow -= decreaseSlow * Time.deltaTime;

        if (curretSlow >= maxSlow)
        {
            navAgent.speed = move_Speed;
        }
    }




    public Transform getCloestEnemy()
    {
        multipleEnemys = GameObject.FindGameObjectsWithTag("Player");
        float cloestDistance = Mathf.Infinity;
        Transform trans = null;

        foreach( GameObject go in multipleEnemys)
        {
            float currentDistance;
            currentDistance = Vector3.Distance(transform.position, go.transform.position);
            if(currentDistance < cloestDistance)
            {
                cloestDistance = currentDistance;
                trans = go.transform;
            }
        }

        return trans;
    }






    void ChasePlayer()
    {
       navAgent.SetDestination(cloestEnemy.transform.position);

        if (curretSlow >= maxSlow)
        {
            navAgent.speed = move_Speed;
        }

        if (curretSlow <= 99f)
        {
            navAgent.speed = move_Slow;
        }

        if (navAgent.velocity.sqrMagnitude == 0)
        {
           
            anim.SetBool("Run", false);
        }
        else
        {
           
            anim.SetBool("Run", true);
        }

        if(Vector3.Distance(transform.position, cloestEnemy.transform.position) <= attack_Distance)
        {
            enemy_State = EnemyState.ATTACK;
            
            
          

        }


        RotateEnemy();




    }
    void RotateEnemy()
    {
        Vector3 direction = cloestEnemy.transform.position - transform.position;
       // Vector3 direction = lookPoint.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = rotation;
        
    }
    void AttackPlayer()
    {
        //navAgent.speed = move_Attack;
         navAgent.velocity = Vector3.zero;
         navAgent.isStopped = true;
        navAgent.SetDestination(cloestEnemy.transform.position);

        if (rangeEnemy)
        {

            RotateEnemy();
        }


        anim.SetBool("Run", false);

        attack_Timer += Time.deltaTime;

        if(attack_Timer > wait_Before_Attack_Time)
        {
           
                if (Random.Range(0, 2) > 0)
                {
                    anim.SetTrigger("Attack");
                     //print("attack1");
            
                }
                else
                {
                    anim.SetTrigger("Attack");
                    //print("attack2");
                }

                attack_Timer = 0f;
            

        }

       // if(Vector3.Distance (transform.position, cloestEnemy.transform.position) > attack_Real + chase_Player_After_Attack_Distance)
        //{
          //  navAgent.isStopped = false;
          
       // }



        if (Vector3.Distance(transform.position, cloestEnemy.transform.position) > attack_Distance + chase_Player_After_Attack_Distance)
        {
            //transform.LookAt(cloestEnemy.transform.position);
            enemy_State = EnemyState.CHASE;
            navAgent.isStopped = false;
        }

    }

    void Active_AttackPoint()
    {
        attackPoint.SetActive(true);
    }

    void Deactive_AttackPoint()
    {
        if (attackPoint.activeInHierarchy)
       {
            attackPoint.SetActive(false);
        }
    }

    void FindCloestEnemy()
    {

        float distanceToClosestEnemy = Mathf.Infinity;
        Enemy cloestEnemy = null;
        Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();

        foreach ( Enemy currentEnemy in allEnemies)
        {
            float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
            if(distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToEnemy;
                cloestEnemy = currentEnemy;
            }
        }


    }

    void ShootPlayer()
    {
        if (rangeEnemy)
        {
            GameObject cB = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);

            cB.transform.Rotate(Vector3.left * 90);

            Rigidbody rig = cB.GetComponent<Rigidbody>();

            rig.AddForce(spawnPoint.forward * speed, ForceMode.Impulse);
        }
    }

    public void Death()
    {

        enemy_State = EnemyState.DIE;

    }

    void DieAnim()
    {
        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true;
        anim.SetBool("Death", true);
        Destroy(GetComponent<CapsuleCollider>());
        Destroy(GetComponent<NavMeshAgent>());
    }




    void EnviromentView()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, playerMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;

    }





}




