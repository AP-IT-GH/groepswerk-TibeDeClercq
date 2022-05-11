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
    public int Size2ShipCount; 
    public int Size3ShipCount;
    public int Size4ShipCount;
    public int Size5ShipCount;
    public int Size6ShipCount;

    public char[,] Values;
    public char[,] DiscoveredValues; //given to other player

    public List<Ship> Ships;

    private void Awake()
    {
        //Debug.Log("Starting a field");
        GenerateShipss();
        CreateField();
        PrintField();
    }

    public void ResetField()
    {
        Awake();
    }

    private void GenerateShipss()
    {
        this.Ships = new List<Ship>();

        this.GenerateShips(2, Size2ShipCount);
        this.GenerateShips(3, Size3ShipCount);
        this.GenerateShips(4, Size4ShipCount);
        this.GenerateShips(5, Size5ShipCount);
        this.GenerateShips(6, Size6ShipCount);
    }

    private void GenerateShips(int shipSize, int shipcount)
    {
        for (int i = 0; i < shipcount; i++)
        {
            Ship ship = new Ship(shipSize, this.Size);
            bool inField = this.InField(ship);
            bool overlap = this.Overlap(ship);

            while (!inField || overlap)
            {
                ship = new Ship(shipSize, this.Size);
                inField = this.InField(ship);
                overlap = this.Overlap(ship);
            }

            this.Ships.Add(ship);
        }
    }

    //private void GenerateSmallShips()
    //{
    //    for (int i = 0; i < this.Size2ShipCount; i++)
    //    {
    //        Ship ship = new Ship(2, this.Size);
    //        bool inField = this.InField(ship);
    //        bool overlap = this.Overlap(ship);

    //        while (!inField || overlap)
    //        {
    //            ship = new Ship(2, this.Size);
    //            inField = this.InField(ship);
    //            overlap = this.Overlap(ship);
    //        }

    //        this.Ships.Add(ship);

    //        //Debug.Log($"SmallShip: Start: {ship.PositionStart.x},{ship.PositionStart.y} End: {ship.PositionEnd.x},{ship.PositionEnd.y}");
    //    }
    //}

    //private void GenerateBigShips()
    //{
    //    for (int i = 0; i < this.Size4ShipCount; i++)
    //    {
    //        Ship ship = new Ship(4, this.Size);
    //        bool inField = this.InField(ship);
    //        bool overlap = this.Overlap(ship);

    //        while (!inField || overlap)
    //        {
    //            ship = new Ship(4, this.Size);
    //            inField = this.InField(ship);
    //            overlap = this.Overlap(ship);
    //        }

    //        this.Ships.Add(ship);

    //        //Debug.Log($"BigShip: Start: {ship.PositionStart.x},{ship.PositionStart.y} End: {ship.PositionEnd.x},{ship.PositionEnd.y}");
    //    }
    //}

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
                        //Debug.Log("Overlap!!!");
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
        this.DiscoveredValues = new char[this.Size, this.Size];

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

        for (int x = 0; x < this.Size; x++)
        {
            for (int y = 0; y < this.Size; y++)
            {
                this.DiscoveredValues[x, y] = 'W';
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
            //Debug.Log(line);
        }
    }

    public char Shoot(Vector2 coords)
    {
        switch (this.Values[(int)coords.x, (int)coords.y])
        {
            case 'W':
                this.Values[(int)coords.x, (int)coords.y] = 'M';
                this.DiscoveredValues[(int)coords.x, (int)coords.y] = 'M';
                return 'W';
            case 'S':
                this.Values[(int)coords.x, (int)coords.y] = 'H';
                this.DiscoveredValues[(int)coords.x, (int)coords.y] = 'H';
                return 'S'; // Hit a ship that wasnt hit before
            case 'H':
                this.Values[(int)coords.x, (int)coords.y] = 'H';
                this.DiscoveredValues[(int)coords.x, (int)coords.y] = 'H';
                return 'H'; // already hit
            case 'M':
                this.Values[(int)coords.x, (int)coords.y] = 'M';
                this.DiscoveredValues[(int)coords.x, (int)coords.y] = 'M';
                return 'M';
            default:
                return 'E';
        }
    }
}
