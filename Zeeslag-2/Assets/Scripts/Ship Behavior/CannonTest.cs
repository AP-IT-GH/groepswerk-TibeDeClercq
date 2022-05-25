using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTest : MonoBehaviour
{

    private List<AudioClip> shootSounds;
    private Transform turret;
    public Transform target;
    private Transform ShotPoint;
    private GameObject bullet;
    private float BlastPower;
    // Start is called before the first frame update
    void Start()
    {
        // shootSounds = GameObject.Find("AudioHelper").GetComponent<AudioHelper>().ShootSounds;
        bullet = GameObject.Find("Bullet");
        turret = transform.GetChild(0);
        ShotPoint = transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        Shoot(target.position);
    }

    public void RotateTowardsTarget(Vector3 target)
    {
        var lookPos = target - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
        StartCoroutine(RotateCannon(target));
    }

    private IEnumerator RotateCannon(Vector3 target)
    {
        yield return new WaitForSeconds(2f);
        var lookPos = target - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        rotation *= Quaternion.Euler(10, 0, 0);
        turret.rotation = Quaternion.Slerp(turret.rotation, rotation, Time.deltaTime);
        yield return new WaitForSeconds(2f);
        rotation = Quaternion.LookRotation(lookPos);
        rotation *= Quaternion.Euler(-20, 0, 0);
        turret.rotation = Quaternion.Slerp(turret.rotation, rotation, Time.deltaTime);
        yield return new WaitForSeconds(2f);
        FireBullet(target);

    }
    public void FireBullet(Vector3 target)
    {
        double distancex = target.x - transform.position.x;
        double distancez = target.z - transform.position.z;

        double sqh = distancex * distancex + distancez * distancez;

        var distance = System.Math.Sqrt(sqh);



        BlastPower = (float)System.Math.Sqrt((distance * 9.81));


        GameObject CreatedCannonball = Instantiate(bullet, ShotPoint.position, ShotPoint.rotation * Quaternion.Euler(45, 0, 0));
        CreatedCannonball.GetComponent<Rigidbody>().velocity = ShotPoint.transform.up * BlastPower;

    }


    public void Shoot(Vector3 target, bool missed = false)
    {
        RotateTowardsTarget(target);
        AudioPlayer audioPlayer = Instantiate(GameObject.Find("AudioPlayer"), transform.position, Quaternion.identity).GetComponent<AudioPlayer>();
        audioPlayer.Play(shootSounds[Random.Range(0, shootSounds.Count)]);

        //if (missed) projectile.gameObject.tag = "MissBullet"
        //if (!missed) projectile.gameObject.tag = "HitBullet"
    }
}
