using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class BattleshipCameraAgentV2 : Agent
{
    public Zeeslag Game;
    public Vector2 chosenCoordinates;
    public char shotResult;

    private bool firstStart = true;

    public override void OnEpisodeBegin()
    {
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
        sensor.AddObservation(Game.Player2CanShoot);
        sensor.AddObservation(chosenCoordinates);
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(shotResult);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        if (Game.GameRestarted && Game.Player2CanShoot)
        {
            chosenCoordinates = new Vector2(actionBuffers.DiscreteActions[0], actionBuffers.DiscreteActions[1]);
            transform.localPosition = new Vector3(chosenCoordinates.x, transform.localPosition.y, chosenCoordinates.y);

            if (actionBuffers.DiscreteActions[2] == 1)
            {
                //if (Game.Player2CanShoot)
                //{
                    AddReward(0.1f);
                    //Debug.Log("Agent fired in time");

                    char lastResult = shotResult;
                    shotResult = Game.Player2Shoot(chosenCoordinates);

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
                        //AddReward(0.5f);
                    }
                    else if (shotResult == 'M' || shotResult == 'H')
                    {
                        Debug.Log("Agent hit same target twice");
                        SetReward(-5f);
                        EndEpisode();
                    }

                    if (Game.GameState == GameState.Completed)
                    {
                        if (Game.winner == Winner.Player2)
                        {
                            Debug.Log("Agent won the game");
                            AddReward(10.0f);
                        }
                        else if (Game.winner == Winner.Player1)
                        {
                            Debug.Log("Agent lost the game");
                        }
                        EndEpisode();
                    }
                //}
                else
                {
                    //AddReward(-1.0f);
                    //Debug.Log("Agent fired too early");
                }
            }
        }   
    }
}
