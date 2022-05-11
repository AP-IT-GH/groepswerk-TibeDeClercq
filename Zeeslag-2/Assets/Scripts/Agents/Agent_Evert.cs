using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class Agent_Evert : Agent
{
    public Zeeslag Game;
    public Vector2 chosenCoordinates;
    public char shotResult;

    private bool firstStart = true;
    private List<Vector2> shotCoords;

    public override void OnEpisodeBegin() //called first
    {
        shotResult = new char();
        shotCoords = new List<Vector2>();
        transform.localPosition = new Vector3(0,transform.localPosition.y,0);
        chosenCoordinates = new Vector2(0, 0);

        if (firstStart)
        {
            firstStart = false;
        }
        else
        {
            Game.Restart();
            Debug.Log("Agent Restarted Game");
        }
    }

    public override void CollectObservations(VectorSensor sensor) //called fter episode begin
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
        //try to fire
        if (actionBuffers.DiscreteActions[2] == 1)
        {
            if (Game.Player2CanShoot)
            {
                Debug.Log("Player fired in time");
            }
            else
            {
                Debug.Log("Player fired too late");
            }

            char lastResult = shotResult;
            shotResult = Game.Player2Shoot(chosenCoordinates);
            shotCoords.Add(chosenCoordinates);

            switch (shotResult)
            {
                case 'S':                    
                    Debug.Log("Agent hit a ship");
                    if (lastResult == 'S')
                    {
                        Debug.Log("Agent hit 2 in a row");
                        AddReward(1.0f);
                    }                    
                    break;
                case 'W':
                    Debug.Log("Agent hit water");
                    AddReward(-1.0f);
                    break;
                case 'M':
                case 'H':
                    Debug.Log("Agent hit same target twice");
                    break;
            }            
        }

        //try to move
        int x = actionBuffers.DiscreteActions[0];
        int y = actionBuffers.DiscreteActions[1];
        transform.localPosition += new Vector3(x == 0 ? -1 : x == 2 ? 1 : 0, 0, y == 0 ? -1 : y == 2 ? 1 : 0);
        chosenCoordinates = new Vector2(transform.localPosition.x, transform.localPosition.z);

        //check if game is completed
        if (Game.GameState == GameState.Completed)
        {
            if (Game.winner == Winner.Player2)
            {
                Debug.Log("Agent won the game");
                AddReward(100.0f);
            }
            else if (Game.winner == Winner.Player1)
            {
                Debug.Log("Agent lost the game");
            }
            EndEpisode();
        }
    }

    private void TryMove(IDiscreteActionMask actionMask)
    {
        if (transform.localPosition.x >= Game.FieldPlayer1.Size - 1)
        {
            actionMask.SetActionEnabled(0, 2, false);
        }
        else
        {
            actionMask.SetActionEnabled(0, 2, true);
        }

        if (transform.localPosition.x <= 0)
        {
            actionMask.SetActionEnabled(0, 0, false);
        }
        else
        {
            actionMask.SetActionEnabled(0, 0, true);
        }

        if (transform.localPosition.z >= Game.FieldPlayer1.Size - 1)
        {
            actionMask.SetActionEnabled(1, 2, false);
        }
        else
        {
            actionMask.SetActionEnabled(1, 2, true);
        }

        if (transform.localPosition.z <= 0)
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
        if (Game.Player2CanShoot == false || shotCoords.Contains(chosenCoordinates) || Game.GameRestarted == false || transform.localPosition.x < 0 || transform.localPosition.y > Game.FieldPlayer1.Size-1 || transform.localPosition.z < 0 || transform.localPosition.z > Game.FieldPlayer1.Size-1)
        {
            actionMask.SetActionEnabled(2, 1, false);
        }
        else
        {
            actionMask.SetActionEnabled(2, 1, true);
        }
    }
}
