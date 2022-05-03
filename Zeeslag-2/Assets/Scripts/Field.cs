using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FieldValue{
    Water,
    Ship,
    Hit,
    Miss
}

public class Field : MonoBehaviour
{
    public int Size;
    public int SmallShipCount;
    public int BigShipCount;

    public char[,] Values;

    public List<Ship> Ships;

    private void Start()
    {
        Debug.Log("Starting a field");
        GenerateShips();
        CreateField();
        PrintField();
    }

    private void GenerateShips()
    {
        this.Ships = new List<Ship>();

        this.GenerateSmallShips();
        this.GenerateBigShips();
    }

    private void GenerateSmallShips()
    {
        for (int i = 0; i < this.SmallShipCount; i++)
        {
            Ship ship = new Ship(2, this.Size);
            bool inField = this.InField(ship);
            bool overlap = this.Overlap(ship);

            while (!inField || overlap)
            {
                ship = new Ship(2, this.Size);
                inField = this.InField(ship);
                overlap = this.Overlap(ship);
            }

            this.Ships.Add(ship);

            Debug.Log($"SmallShip: Start: {ship.PositionStart.x},{ship.PositionStart.y} End: {ship.PositionEnd.x},{ship.PositionEnd.y}");
        }
    }

    private void GenerateBigShips()
    {
        for (int i = 0; i < this.BigShipCount; i++)
        {
            Ship ship = new Ship(4, this.Size);
            bool inField = this.InField(ship);
            bool overlap = this.Overlap(ship);

            while (!inField || overlap)
            {
                ship = new Ship(4, this.Size);
                inField = this.InField(ship);
                overlap = this.Overlap(ship);
            }

            this.Ships.Add(ship);

            Debug.Log($"BigShip: Start: {ship.PositionStart.x},{ship.PositionStart.y} End: {ship.PositionEnd.x},{ship.PositionEnd.y}");
        }
    }

    private bool InField(Ship ship)
    {
        return (ship.PositionStart.x >= 0 && ship.PositionStart.y >= 0 && ship.PositionEnd.x < this.Size && ship.PositionEnd.y < this.Size);
    }

    private bool Overlap(Ship newShip)
    {
        //Debug.Log("Checking overlap");
        foreach (Vector2 ship1Coord in newShip.ShipCoords)
        {
            foreach (Ship ship2 in this.Ships)
            {
                foreach (Vector2 ship2Coord in ship2.ShipCoords)
                {
                    if(ship1Coord == ship2Coord)
                    {
                        Debug.Log("Overlap!!!");
                        return true;
                    }
                }
            }
        }
        //Debug.Log("No overlap!");
        return false;
    }

    private void CreateField()
    {
        this.Values = new char[this.Size, this.Size];

        for (int x = 0; x < this.Size; x++)
        {
            for (int y = 0; y < this.Size; y++)
            {
                FieldValue fieldValue = GetFieldValue(new Vector2(x,y));

                switch (fieldValue)
                {
                    case FieldValue.Water:
                        this.Values[x, y] = 'W';
                        break;
                    case FieldValue.Ship:
                        this.Values[x, y] = 'S';
                        break;
                    case FieldValue.Hit:
                        this.Values[x, y] = 'H';
                        break;
                    case FieldValue.Miss:
                        this.Values[x, y] = 'M';
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private FieldValue GetFieldValue(Vector2 coords)
    {
        foreach (Ship ship in this.Ships)
        {
            foreach (Vector2 shipCoords in ship.ShipCoords)
            {
                if(shipCoords == coords)
                {
                    return FieldValue.Ship;
                }
            }
        }
        return FieldValue.Water;
    }

    private void PrintField()
    {
        for (int x = 0; x < this.Size; x++)
        {
            string line = "";
            for (int y = 0; y < this.Size; y++)
            {
                line += this.Values[x, y];
            }
            Debug.Log(line);
        }
    }

    public bool Shoot(Vector2 coords)
    {
        bool result = false;

        switch (this.Values[(int)coords.x, (int)coords.y])
        {
            case 'W':
                this.Values[(int)coords.x, (int)coords.y] = 'M';
                break;
            case 'S':
                this.Values[(int)coords.x, (int)coords.y] = 'H';
                result = true; // Hit a ship that wasnt hit before
                break;
            case 'H':
                this.Values[(int)coords.x, (int)coords.y] = 'H';
                break;
            case 'M':
                this.Values[(int)coords.x, (int)coords.y] = 'M';
                break;
            default:
                break;
        }

        return result;
    }
}
