using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class AiZombie : MonoBehaviour , Damagable
{
    private enum WanderType { Random , Waypoint};
    [SerializeField] 
    private WanderType type;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float fov = 120f;
    [SerializeField]
    private float viewDistance = 10f;
    [SerializeField]
    private float wanderRadius = 5f;

    [SerializeField]
    private float walkSpeed = 2f;
    [SerializeField]
    private float RunSpeed = 6f;
    [SerializeField]
    private float losePlayer = 4f;
    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private Transform spawnSphere;
    [SerializeField]
    private Transform spawnBloodVfx;
    [SerializeField]
    private int _damage = 20;

    private bool isAware = false;
    private bool isDetecting = false;
    private NavMeshAgent agent;
    private Vector3 wanderPoint;
    private Animator animator;
    private HealthSystem healthSystem;
    private Collider[] ragdollColliders;
    private Rigidbody[] ragdollBodies;
    private float _animblind;
    private float stopSpeed = 0f;
    private float timeLastAttack;
    private bool hasStopped;
    private float idleDuration = 3f; // Adjust this as needed for the idle duration
    private float idleTimer = 0f;
    private bool isIdling = false;
    private bool isFound = true;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip normalClip;
    [SerializeField]
    private float normalPitch = 0.817f;
    [SerializeField]
    private float attackPitch = 1;
    [SerializeField]
    private float deadPitch = 1.15f;

    [SerializeField]
    private AudioClip[] sounds;
    [SerializeField]
    private Transform[] wayPoint;



    private int wayPointindex = 0;
    private float loseTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        wanderPoint = RandomWanderPoint();
        animator = GetComponent<Animator>();
        healthSystem = new HealthSystem(100);
        healthSystem.OnDead += HealthSystem_OnDead1;
        healthSystem.OnDamaged += HandleDamage;
        ragdollColliders = GetComponentsInChildren<Collider>();
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        foreach (Collider collider in ragdollColliders) 
        {
            if (!collider.CompareTag("Zombie"))
            {
                collider.enabled = false;
            }
        }
        foreach(Rigidbody rigidbody in ragdollBodies)
        {
            if (!rigidbody.CompareTag("Zombie"))
            {
                rigidbody.isKinematic = true;
            }
        }
    }





    // Update is called once per frame
    void Update()
    {

        if (healthSystem.IsDead())
        {
            Invoke("DestroyObject", 10f);
        }
        else
        {


            
            if (isAware)
            {

                agent.SetDestination(player.transform.position);
                agent.speed = RunSpeed;
                if (isFound)
                {
                    audioSource.Stop();
                    audioSource.PlayOneShot(sounds[2]);
                    isFound = false;

                }
                if (!isDetecting)
                {
                    loseTimer += Time.deltaTime;
                    if (loseTimer >= losePlayer)
                    {
                        isAware = false;
                        loseTimer = 0;
                        isFound = true;

                    }
                }

            }
            else
            {
                Wander();


            }
            SearchForPlayer();

        }
    }
    private void PlayIdleSound()
    {
        if (normalClip != null)
        {
            audioSource.clip = normalClip;
            audioSource.Play();
        }
    }

    private void SearchForPlayer()
    {
        float distanceToTarget = Vector3.Distance(player.transform.position, transform.position);
        _animblind = Mathf.Lerp(_animblind , agent.speed , Time.deltaTime * 10f);
        animator.SetFloat("Speed" , _animblind);
        if (distanceToTarget <= agent.stoppingDistance)
        {
            if (Time.time >= timeLastAttack + 2.5)
            {

                timeLastAttack = Time.time;
                agent.speed = stopSpeed;
                animator.SetTrigger("Attack");
                audioSource.clip = sounds[Random.Range(3, sounds.Length)];
                audioSource.Stop();
                audioSource.PlayOneShot(audioSource.clip);
                audioSource.pitch = attackPitch;
            }


        }

        if (Vector3.Angle(Vector3.forward , transform.InverseTransformPoint(player.transform.position)) < fov / 2)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < viewDistance)
            {
                RaycastHit hit;
                if(Physics.Linecast(transform.position, player.transform.position , out hit))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        OnAware();
                    }else
                    {
                        isDetecting = false;
                    }

                }else
                {
                    isDetecting = false;
                }
            }else
            {
                isDetecting= false;
            }
        }else
        {
            isDetecting= false;
        }
    }
    public void OnAware()
    {
        isAware = true;
        isDetecting = true;
        loseTimer= 0;
    }

    private void Wander()
    {
        if (type == WanderType.Random)
        {
            if (Vector3.Distance(transform.position, wanderPoint) < 2f)
            {
                wanderPoint = RandomWanderPoint();
            }
            else
            {
                agent.SetDestination(wanderPoint);
            }
        }
        else
        {
            //WayPoint Wandering

            if (wayPoint.Length >= 2)
            {

                if (!isIdling)
                {

                    if (Vector3.Distance(wayPoint[wayPointindex].position, transform.position) < 2f)
                    {
                        agent.speed = 0;
                        isIdling = true;
                        idleTimer = 0f;
                        PlayIdleSound();
                        audioSource.pitch = normalPitch;

                    }
                    else
                    {
                        agent.speed = walkSpeed;
                        agent.SetDestination(wayPoint[wayPointindex].position);
                        
                    }
                }
                else {
                    idleTimer += Time.deltaTime;
                    if (idleTimer >= idleDuration)
                    {
                        isIdling = false;
                        if (wayPointindex == wayPoint.Length - 1)
                        {
                            wayPointindex = 0;
                        }
                        else
                        {
                            wayPointindex++;

                        }
                    }
                }


            }else
            {
                Debug.Log("miss waypoint need more then 1 " + gameObject.name);
            }
        }
    }

    public void ZombieDamage()
    {

        RaycastHit hit;
        if (Physics.SphereCast(spawnSphere.position, 0.5f, spawnSphere.TransformDirection(Vector3.forward), out hit, 1, playerLayer))
        {
            hit.collider.gameObject.TryGetComponent<Damagable>(out Damagable damagable);
            damagable.Damage(_damage);



        }
    }

    public void BossDamage()
    {

        RaycastHit hit;
        if (Physics.SphereCast(spawnSphere.position, 1f, spawnSphere.TransformDirection(Vector3.forward), out hit, 2.5f, playerLayer))
        {
            hit.collider.gameObject.TryGetComponent<Damagable>(out Damagable damagable);
            damagable.Damage(_damage);



        }
    }



    private Vector3 RandomWanderPoint()
    {
        Vector3 randomPoint = (Random.insideUnitSphere * wanderRadius) + transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomPoint, out navHit , wanderRadius , -1);
        return new Vector3(navHit.position.x , transform.position.y , navHit.position.z);   
    }
    public void Damage(int damage)
    {
        healthSystem.Damage(damage);
    }
    private void DestroyObject()
    {
        Destroy(gameObject);
    }

    private void HealthSystem_OnDead1(object sender, System.EventArgs e)
    {
        agent.speed = 0;
        animator.enabled = false;
        foreach (Collider collider in ragdollColliders)
        {
            if(collider == ragdollColliders[0])
            {
                collider.enabled = false;
            }else
            {
                collider.enabled = true;
            }

        }
        foreach (Rigidbody rigid in ragdollBodies)
        {
            rigid.isKinematic = false;
        }
        audioSource.Stop();
        audioSource.PlayOneShot(sounds[1]);
        audioSource.pitch = deadPitch;
        
    }
    private void HandleDamage(object sender, System.EventArgs e)
    {
        animator.SetTrigger("GetDamaged");
        Instantiate(spawnBloodVfx, spawnSphere.position, Quaternion.identity);
        if (healthSystem.GetHealth() > 1)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(sounds[0]);
        }
        OnAware();
    }


}
