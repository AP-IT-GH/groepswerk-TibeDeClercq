using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    private List<AudioClip> shootSounds;
    private void Start()
    {
        shootSounds = GameObject.Find("AudioHelper").GetComponent<AudioHelper>().ShootSounds;
    }

    public void RotateTowardsTarget(float yAngle, float xAngle)
    {

    }

    public void Shoot(Vector3 target, bool missed = false)
    {
        AudioPlayer audioPlayer = Instantiate(GameObject.Find("AudioPlayer"), transform.position, Quaternion.identity).GetComponent<AudioPlayer>();
        audioPlayer.Play(shootSounds[Random.Range(0, shootSounds.Count)]);

        //if (missed) projectile.gameObject.tag = "MissBullet"
        //if (!missed) projectile.gameObject.tag = "HitBullet"
    }
}
