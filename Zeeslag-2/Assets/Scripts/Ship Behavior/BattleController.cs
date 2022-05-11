using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public Zeeslag game;
    public bool isPlayer;

    public List<ShipBehavior> ships;

    void Start()
    {
        StartCoroutine(LateStart());
    }

    private IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < transform.childCount; i++)
        {
            ships.Add(transform.GetChild(i).GetComponent<ShipBehavior>());
        }
    }
}
