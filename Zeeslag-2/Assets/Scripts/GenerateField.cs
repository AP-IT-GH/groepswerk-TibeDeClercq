using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateField : MonoBehaviour
{
    public Field field;
    public Transform startPosition;
    public float shipHeight = 1;
    public float spacing = 1;
    public float offsetZ = 0;

    public GameObject battleShip, destroyer, frigate, cruiser, corvette;

    void Start()
    {
        GenerateShips();
        AssignRandomShip();
    }

    public void ResetField()
    {
        foreach(Transform ship in startPosition)
        {
            Destroy(ship.gameObject);
        }
        startPosition.position = Vector3.zero;
        Start();
    }

    private void AssignRandomShip()
    {
        //property pls
        //GameObject.Find("Warships").transform.GetChild(Random.Range(0, field.Ships.Count)).GetComponent<ShipBehavior>().standingOn = true;
    }

    private void GenerateShips()
    {
        foreach (Ship ship in field.Ships)
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
        startPosition.position = new Vector3(-(field.Size / 2) * spacing, startPosition.position.y, (-(field.Size / 2) * spacing) + offsetZ);
        Debug.Log($"setting transform position to {startPosition.position}");        
    }

    private void InstantiateShip(GameObject shipPrefab, Ship ship)
    {
        GameObject newShip = Instantiate(shipPrefab, new Vector3(ship.PositionStart.x * spacing, shipHeight, ship.PositionStart.y * spacing), Quaternion.identity, startPosition);
        ShipBehavior behavior = newShip.GetComponent<ShipBehavior>();
        behavior.startCoordinates = ship.PositionStart;
        behavior.coordinates = ship.ShipCoords;        
        behavior.orientation = ship.Orientation;

        if (ship.Orientation == Orientation.Horizontal)
        {
            newShip.transform.localRotation = Quaternion.Euler(0, 90, 0);            
        }

        newShip.transform.Rotate(new Vector3(0,0,-0.1f));

        if (startPosition.name == "Enemy Warships")
        {
            MeshRenderer[] renderers = newShip.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers)
            {
                renderer.enabled = false;
            }
        }
    }
}


