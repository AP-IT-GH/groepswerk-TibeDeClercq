using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMathClamp : MonoBehaviour
{
    public bool restart = false;

    void Start()
    {
        Debug.Log("Hello!");
    }

    private void Update()
    {
        if (restart)
        {
            this.gameObject.SetActive(false);
            this.gameObject.SetActive(true);
        }
    }
}
