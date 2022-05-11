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
    private bool checkCoordinates;

    public override void OnEpisodeBegin()
    {
        shotCoords = new List<Vector2>();
        checkCoordinates = false;
        transform.localPosition = new Vector3(0,transform.localPosition.y,0);

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

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(chosenCoordinates);
        sensor.AddObservation(shotResult);       
    }

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        //check if player can shoot (uses timer) or if player chose already existing coordinates
        if (checkCoordinates)
        {
            if (Game.Player2CanShoot == false || shotCoords.Contains(chosenCoordinates) || Game.GameRestarted == false)
            {
                if (shotCoords.Contains(chosenCoordinates))
                {
                    Debug.Log("tried to fire twice, disabling firing");
                }
                actionMask.SetActionEnabled(2, 1, false);
            }
            else
            {
                actionMask.SetActionEnabled(2, 1, true);                
            }
            checkCoordinates = false;
        }
        else
        {
            actionMask.SetActionEnabled(2, 1, false);
        }
        
        if (!checkCoordinates)
        {
            //check if player reached the edge
            if (transform.localPosition.x == Game.FieldPlayer1.Size - 1)
            {
                actionMask.SetActionEnabled(0, 2, false);
            }
            else
            {
                actionMask.SetActionEnabled(0, 2, true);
            }

            if (transform.localPosition.x == 0)
            {
                actionMask.SetActionEnabled(0, 0, false);
            }
            else
            {
                actionMask.SetActionEnabled(0, 0, true);
            }

            if (transform.localPosition.z == Game.FieldPlayer1.Size - 1)
            {
                actionMask.SetActionEnabled(1, 2, false);
            }
            else
            {
                actionMask.SetActionEnabled(1, 2, true);
            }

            if (transform.localPosition.z == 0)
            {
                actionMask.SetActionEnabled(1, 0, false);
            }
            else
            {
                actionMask.SetActionEnabled(1, 0, true);
            }
        }
        else
        {
            actionMask.SetActionEnabled(0, 0, false);
            actionMask.SetActionEnabled(1, 0, false);
            actionMask.SetActionEnabled(0, 2, false);
            actionMask.SetActionEnabled(1, 2, false);
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        switch (actionBuffers.DiscreteActions[0])
        {
            case 0:
                transform.localPosition = new Vector3(transform.localPosition.x - 1, transform.localPosition.y, transform.localPosition.z);
                break;
            case 1:
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
                break;
            case 2:
                transform.localPosition = new Vector3(transform.localPosition.x + 1, transform.localPosition.y, transform.localPosition.z);
                break;
        }

        switch (actionBuffers.DiscreteActions[1])
        {
            case 0:
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 1);
                break;
            case 1:
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
                break;
            case 2:
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 1);
                break;
        }
        transform.localPosition = new Vector3(actionBuffers.DiscreteActions[0], transform.localPosition.y, actionBuffers.DiscreteActions[1]);

        chosenCoordinates = new Vector2(transform.localPosition.x, transform.localPosition.z);
        checkCoordinates = true;

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
                

            if (shotResult == 'S')
            {
                if (lastResult == 'S')
                {
                    Debug.Log("Agent hit 2 in a row!");
                    AddReward(2.0f); //2 hits in a row
                }
                else
                {
                    Debug.Log("Agent hit a ship!");
                    AddReward(1.0f);
                }
            }
            else if (shotResult == 'W')
            {
                Debug.Log("Agent hit water!");
                AddReward(-0.5f);
            }
            else if (shotResult == 'M' || shotResult == 'H')
            {
                Debug.Log("Agent hit same target twice");
                //AddReward(-1f);
                //EndEpisode();
            }

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
        
    }
}
