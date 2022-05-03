using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public int teller = 0;
    public GameObject cube;
    
    public void MakeButton(float row, float col, float spaceRow, float spaceCol)
    {
        Instantiate(cube, new Vector3((row + spaceRow) * cube.transform.localScale.x, 1, (col + spaceCol) * cube.transform.localScale.z), Quaternion.identity);
    }
}
