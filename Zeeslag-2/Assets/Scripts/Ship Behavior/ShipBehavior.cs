using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBehavior : MonoBehaviour
{
    [SerializeField] private float sinkSpeed = 1f;
    [SerializeField] private float sinkRotateSpeed = 10f;
    [SerializeField] private float bobSpeed = 0.5f;
    [SerializeField] private float bobHeight = 1;

    public bool standingOn = false;
    public List<Cannon> cannons;
    public Orientation orientation;
    public Vector2 startCoordinates;
    public List<Vector2> coordinates;
    public List<Transform> parts = new List<Transform>();
    public int Health;

    private float startPosition;
    private float offset;
    private float direction = 1;
    private bool sinking = false;
    private bool canLoseHealth = true;

    public void Start()
    {
        RandomizePosition();
        SetupParts();         
    }

    void Update()
    {
        Bob();
        if (sinking)
        {
            Sink();
        }
    }

    public void Bob()
    {
        if (!standingOn)
        {
            transform.position = new Vector3(transform.position.x, (startPosition + Mathf.Sin(Time.time * bobSpeed) * offset * direction) * bobHeight, transform.position.z);
        }
    }

    public void Sink()
    {
        sinking = true;
        if (transform.eulerAngles.z > 250)
        {
            transform.Rotate(new Vector3(0, 0, -Time.deltaTime * sinkRotateSpeed));
        }
        else
        {
            sinking = false;
        }
    }

    public void Damage()
    {
        if (canLoseHealth)
        {
            canLoseHealth = false;

            if (transform.parent.name == "Player Warships")
            {
                OVRInput.SetControllerVibration(0.1f, 0.8f, OVRInput.Controller.Active);
            }
            //OVRInput.SetControllerVibration(0.8f, 0.8f, OVRInput.Controller.RTouch);

            Health--;
            Debug.Log($"{this}'s Health = {Health}");
            if (Health <= 0)
            {
                Sink();
            }
            StartCoroutine(DamageCooldown());
        }
    }

    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(0.02f);
        canLoseHealth = true;
    }

    private void SetupParts()
    {
        if (gameObject.transform.childCount > 2)
        {
            for (int i = 0; i < gameObject.transform.GetChild(2).transform.childCount; i++)
            {
                parts.Add(gameObject.transform.GetChild(2).transform.GetChild(i).transform);
            }
            Health = parts.Count;
        }        
    }

    public void RandomizePosition()
    {
        if (!standingOn)
        {
            transform.position = new Vector3(transform.position.x, Random.Range(0f, 1f), transform.position.z);
            offset = Random.Range(0.5f, 1f);
            startPosition = transform.position.y;

            if (Random.Range(0f, 1f) > 0.5F)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }
        }        
    }
}
