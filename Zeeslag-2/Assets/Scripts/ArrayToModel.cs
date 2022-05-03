using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayToModel : MonoBehaviour
{
    public char[,] array = new char[10,10];
    public GameObject cube;
    // Start is called before the first frame update
    void Start()
    {
        for (int row = 0; row < array.GetLength(0); row++)
        {
            for (int col = 0; col < array.GetLength(1); col++)
            {
                Instantiate(cube, new Vector3(row*cube.transform.localScale.x, 1, col*cube.transform.localScale.z), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
