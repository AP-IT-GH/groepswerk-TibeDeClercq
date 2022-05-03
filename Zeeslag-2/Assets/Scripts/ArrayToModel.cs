using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayToModel : MonoBehaviour
{
    public char[,] array = new char[10,10];
    public Button cube;
    public float space = 0;
    public int teller = 0;
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
                teller++;
                //Test teller
                //array[(int)row, (int)col] = (char)teller;
                //Instantiate(cube, new Vector3((row+_spaceRow)*cube.transform.localScale.x, 1, (col+ _spaceCol )* cube.transform.localScale.z), Quaternion.identity);
                cube.MakeButton(row,col,_spaceRow,_spaceCol);
                this.cube.teller = teller;
                _spaceCol+=space;
                //Teller
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
