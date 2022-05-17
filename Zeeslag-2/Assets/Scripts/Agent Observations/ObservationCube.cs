using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CubeState { Water, Miss, Hit}

public class ObservationCube : MonoBehaviour
{
    public bool isPlane = false;

    public Vector2 coordinates;
    public ObservationGrid observationGrid;
    public CubeState state = CubeState.Water;
    public bool Selected = false;
    public Renderer Renderer;

    private void Start()
    {
        Renderer = gameObject.GetComponent<Renderer>();
    }

    private void Update()
    {
        TempSelect(); // TEMP
    }

    private void TempSelect()
    {
        if (Selected) // TEMP
        {
            switch (observationGrid.player)
            {
                case Players.Player1:
                    observationGrid.game.Player1Shoot(coordinates);
                    break;
                case Players.Player2:
                    observationGrid.game.Player2Shoot(coordinates);
                    break;
            }
        }
    }

    public void Select()
    {
        switch (observationGrid.player)
        {
            case Players.Player1:
                observationGrid.game.Player1Shoot(coordinates);
                break;
            case Players.Player2:
                observationGrid.game.Player2Shoot(coordinates);
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Finger")
        {
            Select();
        }
    }
}
