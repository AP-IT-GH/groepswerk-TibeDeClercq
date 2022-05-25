using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Orientation
{
    Vertical,
    Horizontal
}

public class Ship
{
    public List<Vector2> ShipCoords;
    public Vector2 PositionStart;
    public Vector2 PositionEnd;
    public Orientation Orientation;
    public int Size;
    public bool IsRevealed = false;

    public Ship(int shipSize, int fieldSize)
    {
        this.Size = shipSize;
        ChooseOrientation();
        ChoosePosition(shipSize, fieldSize);
        FillShipCoords();
    }

    private void ChooseOrientation()
    {
        if (Random.Range(0, 2) == 1)
        {
            this.Orientation = Orientation.Horizontal;
        }
        else
        {
            this.Orientation = Orientation.Vertical;
        }
    }

    private void ChoosePosition(int shipSize, int fieldSize)
    {
        this.PositionStart.x = Random.Range(0, fieldSize);
        this.PositionStart.y = Random.Range(0, fieldSize);

        if(this.Orientation == Orientation.Horizontal)
        {
            this.PositionEnd.x = this.PositionStart.x + shipSize - 1;
            this.PositionEnd.y = this.PositionStart.y;
        }
        else if (this.Orientation == Orientation.Vertical)
        {
            this.PositionEnd.x = this.PositionStart.x;
            this.PositionEnd.y = this.PositionStart.y + shipSize - 1;
        }
        else
        {
            Debug.Log("Illegal orientation");
        }
    }

    private void FillShipCoords()
    {
        this.ShipCoords = new List<Vector2>();
        switch (this.Orientation)
        {
            case Orientation.Vertical:
                for (int y = 0; y < this.Size; y++)
                {
                    this.ShipCoords.Add(new Vector2(this.PositionStart.x, this.PositionStart.y + y));
                }
                break;
            case Orientation.Horizontal:
                for (int x = 0; x < this.Size; x++)
                {
                    this.ShipCoords.Add(new Vector2(this.PositionStart.x + x, this.PositionStart.y));
                }
                break;
            default:
                break;
        }
    }
}
