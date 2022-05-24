using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Range(20.0f, 75.0f)] public float LaunchAngle;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject splash;
    [SerializeField] private List<AudioClip> explosionSounds;
    [SerializeField] private List<AudioClip> splashSounds;
    [SerializeField] private AudioClip flySound;
    [SerializeField] private float explosionYOffset = -1f;
    [SerializeField] private float splashYOffset = -1f;

    public Vector3 target;
    public bool isPlayer = false;

    private float lifeSpan = 0f;
    private Rigidbody rigid;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Start()
    {
        if (name == "Bullet(Clone)")
        {
            audioSource.PlayOneShot(flySound);
        }        
    }

    private void Update()
    {
        lifeSpan += Time.deltaTime;

        if (lifeSpan > 20 && name == "Bullet(Clone)" || transform.position.y < -10)
        {
            Instantiate(explosion, target + new Vector3(0, explosionYOffset, 0), Quaternion.identity);
            AudioPlayer audioPlayer = Instantiate(GameObject.Find("AudioPlayer"), transform.position, Quaternion.identity).GetComponent<AudioPlayer>();
            audioPlayer.Play(explosionSounds[Random.Range(0, explosionSounds.Count)]);

            Destroy(gameObject);
        }

        transform.rotation = Quaternion.LookRotation(rigid.velocity) * initialRotation;
        if (!isPlayer)
        {
            transform.Rotate(new Vector3(180, 0, 0));
        }
    }

    public void Launch()
    {
        rigid = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        rigid.velocity = Vector3.zero;
        transform.SetPositionAndRotation(initialPosition, initialRotation);

        // think of it as top-down view of vectors: 
        //   we don't care about the y-component(height) of the initial and target position.
        Vector3 projectileXZPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
        Vector3 targetXZPos = new Vector3(target.x, 0.0f, target.z);

        // rotate the object to face the target
        transform.LookAt(targetXZPos);

        // shorthands for the formula
        float R = Vector3.Distance(projectileXZPos, targetXZPos);
        float G = Physics.gravity.y;
        float tanAlpha = Mathf.Tan(LaunchAngle * Mathf.Deg2Rad);
        float H = target.y - transform.position.y;

        // calculate the local space components of the velocity 
        // required to land the projectile on the target object 
        float Vz = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)));
        float Vy = tanAlpha * Vz;

        // create the velocity vector in local space and get it in global space
        Vector3 localVelocity = new Vector3(0f, Vy, Vz);
        Vector3 globalVelocity = transform.TransformDirection(localVelocity);

        // launch the object by setting its initial velocity and flipping its state
        rigid.velocity = globalVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        ShipBehavior target = other.gameObject.GetComponentInParent<ShipBehavior>();

        //Instantiate explosion when hitting a ship
        if (other.tag == "Ship" && tag == "HitBullet" && lifeSpan > 2f && target.Health > 0)
        {   
            if (target.transform.parent.name == "Player Warships")
            {
                Instantiate(explosion, transform.position + new Vector3(0, explosionYOffset, 0), Quaternion.identity, other.transform);
                AudioPlayer audioPlayer = Instantiate(GameObject.Find("AudioPlayer"), transform.position, Quaternion.identity).GetComponent<AudioPlayer>();
                audioPlayer.Play(explosionSounds[Random.Range(0, explosionSounds.Count)]);
            }            

            //Reduce ships health
            target.Damage();

            Destroy(gameObject);
        }

        //Instantiate splash when hitting water
        if (other.tag == "Water" && tag == "MissBullet" && lifeSpan > 2f)
        {
            Instantiate(splash, transform.position + new Vector3(0, splashYOffset, 0), Quaternion.identity, other.transform);
            AudioPlayer audioPlayer = Instantiate(GameObject.Find("AudioPlayer"), transform.position, Quaternion.identity).GetComponent<AudioPlayer>();
            audioPlayer.Play(splashSounds[Random.Range(0, splashSounds.Count)]);

            Destroy(gameObject);
        }
    }
}
