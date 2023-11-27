using StarterAssests;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;


public class Player : MonoBehaviour , Damagable
{
    private Animator _animator;
    private StarterAssestsInputs _input;
    private Damagable damagable;
    [SerializeField] 
    private Transform transformSpawn;
    [SerializeField]
    private LayerMask zombieLayer;
    private float timer;
    [SerializeField]
    private float attackTimer = 2f;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Gradient gradient;
    [SerializeField]
    private Image fill;
    [SerializeField]
    private Transform aimBox;
    [SerializeField]
    private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField]
    private Animator gunAnimation;
    [SerializeField]
    private TwoBoneIKConstraint rightHandBone;
    [SerializeField]
    private TwoBoneIKConstraint leftHandBone;
    [SerializeField]
    private TwoBoneIKConstraint leftHandWithGun;
    [SerializeField]
    private MultiAimConstraint AimRig;
    [SerializeField]
    private MultiAimConstraint bodyRig;
    [SerializeField]
    private GameObject mechate;
    [SerializeField]
    private GameObject pistolGun;
    [SerializeField]
    private GameObject crossFireImage;
    [SerializeField]
    private Transform spawnBulletPosition;
    [SerializeField]
    private Transform bulletProjectile;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private GameObject textBox;
    public bool isPickUp = false;
    [SerializeField]
    private Transform vfx;
    [SerializeField]
    private AudioClip[] audioClips;
    private HealthSystem healthSystem;
    [SerializeField]
    private bool gunEquipment = false;
    [SerializeField]
    private int health;
    private float rigwight;
    private int bullets = 5;
    private AudioSource audioSource;




    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _animator = GetComponent<Animator>();
        _input = GetComponent<StarterAssestsInputs>();
        gunAnimation = GetComponent<Animator>();
        healthSystem = new HealthSystem(100);
        healthSystem.OnDead += HealthSystem_OnDead1;
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        healthSystem.OnHealthMaxChanged += HealthSystem_OnHealthMaxChanged;
        audioSource = GetComponent<AudioSource>();
        health = healthSystem.GetHealth();
        


    }

    private void switchWeapon()
    {
        if(UnityEngine.Input.GetKeyDown(KeyCode.Alpha1) && isPickUp)
        {
            gunEquipment = !gunEquipment;
            
        }
    }

    private void HealthSystem_OnHealthMaxChanged(object sender, EventArgs e)
    {
        slider.maxValue = healthSystem.GetHealthMax();
        fill.color = gradient.Evaluate(1f);

    }

    private void HealthSystem_OnHealthChanged(object sender, EventArgs e)
    {
        slider.value = healthSystem.GetHealth();
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    private void HealthSystem_OnDead1(object sender, EventArgs e)
    {
        SceneManager.LoadScene("DeathScene");
    }

    // Update is called once per fra
    void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            aimBox.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;

        }
        text.text = "0/" + bullets.ToString();
        switchWeapon();
        if (gunEquipment)
        {
            

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            //transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
            gunAnimation.SetLayerWeight(1, Mathf.Lerp(gunAnimation.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
            rightHandBone.weight = 0;
            leftHandBone.weight = 0;
            leftHandWithGun.weight = 1;
            AimRig.weight = 1;
            //bodyRig.weight = 1;
            mechate.SetActive(false);
            pistolGun.SetActive(true);
            crossFireImage.SetActive(true);
            textBox.SetActive(true);
            ;

            if ( _input.shoot && bullets >0)
            {
                Debug.Log(bullets);
                Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
                Instantiate(bulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
                Instantiate(vfx, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));

                _input.shoot = false;
               bullets -= 1;
                audioSource.PlayOneShot(audioClips[0]);

            }

        }
        else

        {
            timer += Time.deltaTime;
            if (_input.shoot && timer > attackTimer)
            {
                _animator.SetTrigger("meleeAttack");
                timer = 0;
                _input.shoot = false;

            }
            gunAnimation.SetLayerWeight(1, Mathf.Lerp(gunAnimation.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
            rightHandBone.weight = 1;
            leftHandBone.weight = 1;
            leftHandWithGun.weight = 0;
            AimRig.weight = 0f;
            bodyRig.weight = 0f;
            mechate.SetActive(true);
            pistolGun.SetActive(false);
            crossFireImage?.SetActive(false);
            textBox.SetActive(false);
        }



    }

    public void Fire()
    {
       
        


            RaycastHit hit;
            if (Physics.SphereCast(transformSpawn.position, 0.5f, transformSpawn.TransformDirection(Vector3.forward), out hit , 1 , zombieLayer))
                {
                hit.collider.gameObject.TryGetComponent<Damagable>(out Damagable damagable);
            if(hit.collider.gameObject.name == "Boss")
            {
                damagable.Damage(20);
            }else
                    damagable.Damage(30);
                
                
            }
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Zombie"))
        {
            
            other.GetComponent<AiZombie>().OnAware();
        }
    }

    public void Damage(int damage)
    {
        healthSystem.Damage(damage);
    }

    public void GetHeal()
    {

            healthSystem.Heal(100);


    }
    public bool IsHealthAtMax()
    {
        return healthSystem.GetHealth() >= healthSystem.GetHealthMax();
    }


}
