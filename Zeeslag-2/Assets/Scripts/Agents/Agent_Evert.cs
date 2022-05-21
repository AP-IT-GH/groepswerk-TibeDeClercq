using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class Agent_Evert : Agent
{
    public bool Paused = true;

    public Zeeslag Game;
    public Vector2 chosenCoordinates;
    public char shotResult;

    private int defaultFieldSize;
    private List<Vector2> shotCoords;
    private int missedCount;
    private int fieldTilesCount;
    private int stepCount;
    //private bool firstStart = true;

    private void Start()
    {
        defaultFieldSize = Game.FieldPlayer1.Size;
    }

    public override void OnEpisodeBegin() //called first
    {
        stepCount = 0;
        fieldTilesCount = Game.FieldPlayer1.Size * Game.FieldPlayer1.Size;
        transform.localPosition = new Vector3(0, transform.localPosition.y, 0);
        missedCount = 0;
        shotResult = new char();
        shotCoords = new List<Vector2>();
        chosenCoordinates = new Vector2(0, 0);

        //if (!firstStart)
        //{
        //    SetFieldVariables();
        //    Game.Restart();
        //    Debug.Log("Agent Restarted Game");
        //    firstStart = false;
        //}        
    }

    public override void CollectObservations(VectorSensor sensor) //called after episode begin
    {
        sensor.AddObservation(chosenCoordinates);
        sensor.AddObservation(shotResult);
    }

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask) //called after collect observations
    {
        //check if player can shoot (uses configurable timer) or if player chose already existing coordinates
        TryShoot(actionMask);

        //check if player reached the edge
        TryMove(actionMask);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers) //called last
    {
        stepCount++;

        //try to fire
        if (actionBuffers.DiscreteActions[2] == 1)
        {
            shotResult = Game.Player2Shoot(chosenCoordinates);
            shotCoords.Add(chosenCoordinates);

            if (shotResult == 'W') missedCount++;
        }

        //try to move
        int x = actionBuffers.DiscreteActions[0];
        int y = actionBuffers.DiscreteActions[1];
        transform.localPosition += new Vector3(x == 0 ? -1 : x == 2 ? 1 : 0, 0, y == 0 ? -1 : y == 2 ? 1 : 0); //move with steps ( +1 -1 0 )
        chosenCoordinates = new Vector2(transform.localPosition.x, transform.localPosition.z);

        //check if game is completed
        if (Game.GameState == GameState.Completed)
        {
            if (Game.winner == Winner.Player2)
            {
                Debug.Log("Agent won the game");
                float waterTileCount = fieldTilesCount - Game.FieldPlayer1.GetShipPartCount();
                AddReward((waterTileCount - missedCount) / waterTileCount); // 1.0 would be the theorethical max score: never missed
            }
            else if (Game.winner == Winner.Player1)
            {
                Debug.Log("Agent lost the game");
            }
            Paused = true;
            EndEpisode();
        }

        if (stepCount > 2000) //to prevent the agent from stopping to fire, which can happen
        {
            AddReward(-1f);
            EndEpisode();
        }
    }

    private void SetFieldVariables()
    {
        Game.FieldPlayer1.Size = (int)Academy.Instance.EnvironmentParameters.GetWithDefault("field_size", defaultFieldSize);
        switch (Game.FieldPlayer1.Size)
        {
            case 4:
                Game.FieldPlayer1.Size2ShipCount = 1;
                Game.FieldPlayer1.Size3ShipCount = 1;
                Game.FieldPlayer1.Size4ShipCount = 0;
                Game.FieldPlayer1.Size5ShipCount = 0;
                Game.FieldPlayer1.Size6ShipCount = 0;
                break;
            case 5:
                Game.FieldPlayer1.Size2ShipCount = 1;
                Game.FieldPlayer1.Size3ShipCount = 1;
                Game.FieldPlayer1.Size4ShipCount = 1;
                Game.FieldPlayer1.Size5ShipCount = 0;
                Game.FieldPlayer1.Size6ShipCount = 0;
                break;
            case 6:
                Game.FieldPlayer1.Size2ShipCount = 1;
                Game.FieldPlayer1.Size3ShipCount = 1;
                Game.FieldPlayer1.Size4ShipCount = 1;
                Game.FieldPlayer1.Size5ShipCount = 1;
                Game.FieldPlayer1.Size6ShipCount = 0;
                break;
            case 7:
                Game.FieldPlayer1.Size2ShipCount = 1;
                Game.FieldPlayer1.Size3ShipCount = 1;
                Game.FieldPlayer1.Size4ShipCount = 1;
                Game.FieldPlayer1.Size5ShipCount = 1;
                Game.FieldPlayer1.Size6ShipCount = 0;
                break;
            case 8:
                Game.FieldPlayer1.Size2ShipCount = 1;
                Game.FieldPlayer1.Size3ShipCount = 1;
                Game.FieldPlayer1.Size4ShipCount = 1;
                Game.FieldPlayer1.Size5ShipCount = 1;
                Game.FieldPlayer1.Size6ShipCount = 1;
                break;
            case 9:
                Game.FieldPlayer1.Size2ShipCount = 1;
                Game.FieldPlayer1.Size3ShipCount = 1;
                Game.FieldPlayer1.Size4ShipCount = 1;
                Game.FieldPlayer1.Size5ShipCount = 1;
                Game.FieldPlayer1.Size6ShipCount = 1;
                break;
            case 10:
                Game.FieldPlayer1.Size2ShipCount = 1;
                Game.FieldPlayer1.Size3ShipCount = 1;
                Game.FieldPlayer1.Size4ShipCount = 1;
                Game.FieldPlayer1.Size5ShipCount = 1;
                Game.FieldPlayer1.Size6ShipCount = 1;
                break;
        }
    }

    private void TryMove(IDiscreteActionMask actionMask)
    {
        if (transform.localPosition.x >= Game.FieldPlayer1.Size - 1 || Paused)
        {
            actionMask.SetActionEnabled(0, 2, false);
        }
        else
        {
            actionMask.SetActionEnabled(0, 2, true);
        }

        if (transform.localPosition.x <= 0 || Paused)
        {
            actionMask.SetActionEnabled(0, 0, false);
        }
        else
        {
            actionMask.SetActionEnabled(0, 0, true);
        }

        if (transform.localPosition.z >= Game.FieldPlayer1.Size - 1 || Paused)
        {
            actionMask.SetActionEnabled(1, 2, false);
        }
        else
        {
            actionMask.SetActionEnabled(1, 2, true);
        }

        if (transform.localPosition.z <= 0 || Paused)
        {
            actionMask.SetActionEnabled(1, 0, false);
        }
        else
        {
            actionMask.SetActionEnabled(1, 0, true);
        }
    }

    private void TryShoot(IDiscreteActionMask actionMask)
    {
        if (Game.Player2CanShoot == false || shotCoords.Contains(chosenCoordinates) || Game.GameRestarted == false || transform.localPosition.x < 0 || transform.localPosition.y > Game.FieldPlayer1.Size-1 || transform.localPosition.z < 0 || transform.localPosition.z > Game.FieldPlayer1.Size-1 || Paused)
        {
            actionMask.SetActionEnabled(2, 1, false);
        }
        else
        {
            actionMask.SetActionEnabled(2, 1, true);
        }
    }
}
