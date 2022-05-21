using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public void RotateTowardsTarget(float yAngle, float xAngle)
    {

    }

    public void Shoot(Vector3 target)
    {
        Debug.Log($"Firing at {target}");
    }
}
