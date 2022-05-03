using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateField : MonoBehaviour
{
    public Zeeslag game;
    public Transform startPosition;
    public float shipHeight = 1;
    public float spacing = 1;

    public GameObject battleShip, destroyer, frigate, cruiser, corvette;

    void Start()
    {
        foreach(Ship ship in game.FieldPlayer1.Ships)
        {
            switch (ship.Size)
            {
                case 2:
                    InstantiateShip(corvette, ship);
                    break;
                case 3:
                    InstantiateShip(frigate, ship);
                    break;
                case 4:
                    InstantiateShip(destroyer, ship);
                    break;
                case 5:
                    InstantiateShip(cruiser, ship);
                    break;
                case 6:
                    InstantiateShip(battleShip, ship);
                    break;
            }
        }
        startPosition.position = new Vector3(-(game.FieldPlayer1.Size/2) * spacing, startPosition.position.y, -(game.FieldPlayer1.Size / 2) * spacing);
    }
    private void InstantiateShip(GameObject shipPrefab, Ship ship)
    {
        if (ship.Orientation == Orientation.Horizontal)
        {
            Instantiate(shipPrefab, new Vector3(ship.PositionStart.x * spacing, shipHeight, ship.PositionStart.y * spacing), Quaternion.Euler(0, 90, 0), startPosition);
        }
        else
        {
            Instantiate(shipPrefab, new Vector3(ship.PositionStart.x * spacing, shipHeight, ship.PositionStart.y * spacing), Quaternion.identity, startPosition);
        }
    }
}


