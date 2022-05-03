using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayToModel : MonoBehaviour
{
    public char[,] array = new char[10,10];
    public GameObject cube;
    public float space = 0;
    // Start is called before the first frame update
    void Start()
    {
        float _spaceRow = 0;
        float _spaceCol = 0;
        for (float row = 0; row < array.GetLength(0); row++)
        {
            _spaceCol = 0;
            _spaceRow += space;
            for (float col = 0; col < array.GetLength(1); col++)
            {
                Instantiate(cube, new Vector3((row+_spaceRow)*cube.transform.localScale.x, 1, (col+ _spaceCol )* cube.transform.localScale.z), Quaternion.identity);
                _spaceCol+=space;            
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
