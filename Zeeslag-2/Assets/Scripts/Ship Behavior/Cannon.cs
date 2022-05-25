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
    private bool isPlayer = false;
    private ShipBehavior ship;


    private void Start()
    {
        enabled = false;
        shootSounds = GameObject.Find("AudioHelper").GetComponent<AudioHelper>().ShootSounds;
        cannonHelper = GameObject.Find("CannonHelper").GetComponent<CannonHelper>();
        bullet = cannonHelper.bullet;
        turret = transform.GetChild(0);
        ShotPoint = transform.GetChild(1);
        ship = transform.GetComponentInParent<ShipBehavior>();
        isPlayer = ship.transform.parent.name == "Player Warships";
    }


    private void Update()
    {
        if (rotatedown)
        {
            RotateDown(this.target);
        }
    }

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

    public GameObject InstantiateProjectile()
    {
        if (isPlayer)
        {
            return Instantiate(bullet, ShotPoint.position, Quaternion.Euler(90, 0, 0));
        }
        else
        {
            return Instantiate(bullet, ShotPoint.position, Quaternion.Euler(90, 0, 180));
        }
    }

    public void FireBullet(Vector3 target, bool missed)
    {
        //Instantiate the projectile
        GameObject projectileObj = InstantiateProjectile();
        Projectile projectile = projectileObj.GetComponent<Projectile>();

        //Assign variables and launch
        projectile.target = target;
        projectile.isPlayer = isPlayer;
        projectile.Launch();

        //Assign correct tag to bullet
        projectileObj.tag = missed ? "MissBullet" : "HitBullet";

        //Play shoot audio
        AudioPlayer audioPlayer = Instantiate(GameObject.Find("AudioPlayer"), transform.position, Quaternion.identity).GetComponent<AudioPlayer>();
        audioPlayer.Play(shootSounds[Random.Range(0, shootSounds.Count)]);

        //Instantiate particles if player is shooting
        if (isPlayer)
        {
            Instantiate(cannonHelper.shootParticles, ShotPoint.position, Quaternion.identity);
            RotateDown(target);
        }
    }

    public void Shoot(Vector3 target, bool missed = false)
    {
        this.target = target;
        this.missed = missed;
        enabled = true;

        FireBullet(target, missed);
    }
}
