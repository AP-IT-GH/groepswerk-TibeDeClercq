using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public Zeeslag game;
    public GameObject otherPlayerField;
    public bool isPlayer;
    [HideInInspector] public List<ShipBehavior> ships;
    [HideInInspector] public List<ShipBehavior> otherPlayerShips;

    private GenerateField enemyField;

    void Start()
    {
        StartCoroutine(LateStart());
        enemyField = otherPlayerField.transform.parent.GetComponent<GenerateField>();
    }

    private IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.7f);

        for (int i = 0; i < transform.childCount; i++)
        {
            ships.Add(transform.GetChild(i).GetComponent<ShipBehavior>());
        }

        for (int i = 0; i < otherPlayerField.transform.childCount; i++)
        {
            otherPlayerShips.Add(otherPlayerField.transform.GetChild(i).GetComponent<ShipBehavior>());
        }
    }

    public void Shoot(Vector2 coords, int shotCount = 1)
    {
        //Option to shoot from multiple ships, default is 1
        for (int i = 0; i < shotCount; i++)
        {
            ShipBehavior shootingShip = ships[Random.Range(0, ships.Count)];
            List<Cannon> validCannons = new List<Cannon>();
            ShipBehavior targetShip = null;

            //Find if coordinates match enemy ship
            foreach (ShipBehavior ship in otherPlayerShips)
            {
                if (ship.coordinates.Contains(coords))
                {
                    targetShip = ship;
                    break;
                }
            }


            //Add all valid cannons if the shooting ship to list
            if (isPlayer)
            {
                if (shootingShip.orientation == Orientation.Vertical)
                {
                    foreach (Cannon cannon in shootingShip.cannons)
                    {
                        if (cannon is FrontCannon)
                        {
                            validCannons.Add(cannon);
                        }
                    }
                }
                else
                {
                    foreach (Cannon cannon in shootingShip.cannons)
                    {
                        if (cannon is FrontCannon || cannon is BackCannon || cannon is LeftCannon)
                        {
                            validCannons.Add(cannon);
                        }
                    }
                }
            }
            else
            {
                if (shootingShip.orientation == Orientation.Vertical)
                {
                    foreach (Cannon cannon in shootingShip.cannons)
                    {
                        if (cannon is BackCannon)
                        {
                            validCannons.Add(cannon);
                        }
                    }
                }
                else
                {
                    foreach (Cannon cannon in shootingShip.cannons)
                    {
                        if (cannon is FrontCannon || cannon is BackCannon || cannon is RightCannon)
                        {
                            validCannons.Add(cannon);
                        }
                    }
                }
            }

            //Pick a random cannon to shoot
            Cannon shootingCannon = validCannons[Random.Range(0, validCannons.Count)];

            //Fire
            if (targetShip != null) 
            {
                shootingCannon.Shoot(targetShip.parts[targetShip.coordinates.IndexOf(coords)].position);
            }
            else
            {
                Vector3 offset = new Vector3(-(enemyField.field.Size / 2) * enemyField.spacing, enemyField.startPosition.position.y, (-(enemyField.field.Size / 2) * enemyField.spacing) + enemyField.offsetZ);
                shootingCannon.Shoot(new Vector3(coords.x * enemyField.spacing, 0, coords.y * enemyField.spacing) + offset);
            }
        }        
    }
}
