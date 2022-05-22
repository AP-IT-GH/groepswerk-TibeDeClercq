using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject splash;
    [SerializeField] private List<AudioClip> explosionSounds;
    [SerializeField] private List<AudioClip> splashSounds;
    [SerializeField] private AudioClip flySound;
    [SerializeField] private float explosionYOffset = -1f;
    [SerializeField] private float splashYOffset = -1f;

    private void Start()
    {
        if (name == "Bullet(Clone)")
        {
            audioSource.PlayOneShot(flySound);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Instantiate explosion when hitting a ship
        if (other.tag == "Ship" && tag == "HitBullet")
        {
            Instantiate(explosion, transform.position + new Vector3(0,explosionYOffset,0), Quaternion.identity, other.transform);
            AudioPlayer audioPlayer = Instantiate(GameObject.Find("AudioPlayer"), transform.position, Quaternion.identity).GetComponent<AudioPlayer>();
            audioPlayer.Play(explosionSounds[Random.Range(0, explosionSounds.Count)]);

            Destroy(gameObject);
        }

        //Instantiate splash when hitting water
        if (other.tag == "Water" && tag == "MissBullet")
        {
            Instantiate(splash, transform.position + new Vector3(0, splashYOffset, 0), Quaternion.identity, other.transform);
            AudioPlayer audioPlayer = Instantiate(GameObject.Find("AudioPlayer"), transform.position, Quaternion.identity).GetComponent<AudioPlayer>();
            audioPlayer.Play(splashSounds[Random.Range(0, splashSounds.Count)]);

            Destroy(gameObject);
        }
    }
}
