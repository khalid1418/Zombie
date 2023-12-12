using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour {

    [SerializeField] private Transform vfxBlood;
    //[SerializeField] private Transform vfxHitRed;
    private float speed = 100f;
    private Transform hitTransform;


    private Rigidbody bulletRigidbody;

    private void Awake() {
        bulletRigidbody = GetComponent<Rigidbody>();
        

    }

    private void Start() {

        bulletRigidbody.velocity = transform.forward * speed;
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {

        }
        else
        {
            if (other.GetComponent<BulletTarget>() != null)
            {
                // Hit target
                Instantiate(vfxBlood, transform.position, Quaternion.identity);
                other.gameObject.TryGetComponent<Damagable>(out Damagable damagable);
                hitTransform = other.transform;
                if (other.gameObject.name == "Boss")
                {
                    damagable.Damage(30);
                }
                else
                    damagable.Damage(50);


            }

           


            
            else
            {
                // Hit something else
                //Instantiate(vfxHitRed, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

}