using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CubeState { Water, Miss, Hit}

public class ObservationCube : MonoBehaviour
{
    public Vector2 coordinates;
    public ObservationGrid observationGrid;
    public CubeState state = CubeState.Water;
    public bool Selected = false;

    private Renderer _renderer;

    private void Start()
    {
        _renderer = this.gameObject.GetComponent<Renderer>();
    }

    private void Update()
    {
        switch (state)
        {
            case CubeState.Water:
                _renderer.material.color = Color.blue;
                break;
            case CubeState.Miss:
                _renderer.material.color = Color.white;
                break;
            case CubeState.Hit:
                _renderer.material.color = Color.red;
                break;
        }

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
}
