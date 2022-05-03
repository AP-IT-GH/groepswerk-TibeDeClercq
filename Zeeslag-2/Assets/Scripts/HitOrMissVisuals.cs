using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitOrMissVisuals : MonoBehaviour
{
    public Field field;
    public ArrayToModel array;
    // Start is called before the first frame update
    void Start()
    {
        array.array = field.Values;
    }
    public void Activate()
    {
        if(this.array.teller == 'H')
        {
            this.gameObject.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
        }
        else
        {
            this.gameObject.GetComponent<Renderer>().material.color = new Color(255, 0, 0);
        }
        
    }
}
