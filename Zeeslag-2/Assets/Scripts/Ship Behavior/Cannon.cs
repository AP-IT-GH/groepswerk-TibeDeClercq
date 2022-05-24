using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour
{
    private List<AudioClip> shootSounds;
    private Transform turret;
    private Transform ShotPoint;
    private GameObject bullet;

    private Vector3 target;
    private bool missed;
    private bool inPosition = false;
    private CannonHelper cannonHelper;
    private bool rotatedown = false;


    private void Start()
    {
        enabled = false;
        shootSounds = GameObject.Find("AudioHelper").GetComponent<AudioHelper>().ShootSounds;
        cannonHelper = GameObject.Find("CannonHelper").GetComponent<CannonHelper>();
        bullet = cannonHelper.bullet;
        turret = transform.GetChild(0);
        ShotPoint = transform.GetChild(1);
    }


    private void Update()
    {
        if (rotatedown)
        {
            RotateDown(this.target);
        }

        //RotateTowardsTarget(this.target);

        //if (inPosition)
        //{
        //    FireBullet(this.target, this.missed);
        //    inPosition = false;
        //}
    }

    //public void RotateTowardsTarget(Vector3 target)
    //{
    //    var lookPos = target - transform.position;
    //    lookPos.y = 0;
    //    var rotation = Quaternion.LookRotation(lookPos);

    //    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);

    //    if (transform.rotation == rotation)
    //    {
    //        //Debug.Log("Cannon reached rotation");
    //        //inPosition = true;
    //        enabled = false;
    //    }
    //    inPosition = true;
    //}

    public void HoverRotate(Vector3 target)
    {
        var lookPos = target - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);
                
        rotation *= Quaternion.Euler(10, 0, 0);
        turret.rotation = Quaternion.Slerp(turret.rotation, rotation, Time.deltaTime);
    }

    private IEnumerator RotateDown(Vector3 target)
    {
        rotatedown = true;

        var lookPos = target - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);

        rotation = Quaternion.LookRotation(lookPos);
        rotation *= Quaternion.Euler(-20, 0, 0);
        turret.rotation = Quaternion.Slerp(turret.rotation, rotation, Time.deltaTime);

        if (turret.rotation.x >= rotation.x)
        {
            rotatedown = false;
        }
        yield return new WaitForSeconds(1.5f);
    }

    public void CorrectCannon()
    {
        var lookPos = new Vector3(0,0,-700) - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);

        rotation *= Quaternion.Euler(10, 0, 0);
        turret.rotation = rotation;
        Debug.Log(turret.rotation);
    }

    public void FireBullet(Vector3 target, bool missed)
    {
        //enabled = false;
 
        //double distancex = target.x - transform.position.x;
        //double distancez = target.z - transform.position.z;
        //double sqh = distancex * distancex + distancez * distancez;
        //var distance = System.Math.Sqrt(sqh);
        //BlastPower = (float)System.Math.Sqrt(distance * Physics.gravity.magnitude);
        GameObject projectile = Instantiate(bullet, ShotPoint.position, ShotPoint.rotation * Quaternion.Euler(45, 0, 0));
        projectile.GetComponent<Projectile>().target = target;
        projectile.GetComponent<Projectile>().isPlayer = transform.GetComponentInParent<ShipBehavior>().transform.parent.name == "Player Warships";
        projectile.GetComponent<Projectile>().Launch();
        //projectile.GetComponent<Rigidbody>().velocity = ShotPoint.transform.up * BlastPower;

        //Assign correct tag to bullet
        projectile.tag = missed ? "MissBullet" : "HitBullet";

        //Play shoot audio
        AudioPlayer audioPlayer = Instantiate(GameObject.Find("AudioPlayer"), transform.position, Quaternion.identity).GetComponent<AudioPlayer>();
        audioPlayer.Play(shootSounds[Random.Range(0, shootSounds.Count)]);

        //Instantiate particles
        Instantiate(cannonHelper.shootParticles, ShotPoint.position, Quaternion.identity);

        //rotate cannons down 

    }

    public void Shoot(Vector3 target, bool missed = false)
    {
        this.target = target;
        this.missed = missed;
        enabled = true;

        FireBullet(target, missed);
    }
}
