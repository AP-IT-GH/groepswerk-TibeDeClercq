using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject splash;
    //water sound
    //explosion sound
    [SerializeField] private float explosionYOffset = -1f;
    [SerializeField] private float splashYOffset = -1f;

    private void OnTriggerEnter(Collider other)
    {
        //Instantiate explosion when hitting a ship
        if (other.tag == "Ship" && tag == "HitBullet")
        {
            Instantiate(explosion, transform.position + new Vector3(0,explosionYOffset,0), Quaternion.identity, other.transform);
            //play explosion sound
            Destroy(gameObject);
        }

        //Instantiate splash when hitting water
        if (other.tag == "Water" && tag == "MissBullet")
        {
            Instantiate(splash, transform.position + new Vector3(0, splashYOffset, 0), Quaternion.identity, other.transform);
            //play splash sound
            Destroy(gameObject);
        }
    }
}
