using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Linq;

public class BattleshipCameraAgentV2 : Agent
{
    public Zeeslag Game;
    public Vector2 chosenCoordinates;
    public List<float> firedXCoordinates;
    public List<float> firedYCoordinates;
    public char shotResult;

    public override void OnEpisodeBegin()
    {
        firedXCoordinates.Clear();
        firedYCoordinates.Clear();
        firedXCoordinates = Enumerable.Repeat(-1f, 100).ToList();
        firedYCoordinates = Enumerable.Repeat(-1f, 100).ToList();

        if (Game.GameState == GameState.Completed)
        {
            Game.Restart();            
            Debug.Log("Agent Restarted Game");
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Game.Player2CanShoot);
        sensor.AddObservation(chosenCoordinates);
        sensor.AddObservation(firedXCoordinates);
        sensor.AddObservation(firedYCoordinates);
        sensor.AddObservation(shotResult);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        chosenCoordinates = new Vector2(actionBuffers.DiscreteActions[0], actionBuffers.DiscreteActions[1]);
        transform.localPosition = new Vector3(chosenCoordinates.x, transform.localPosition.y, chosenCoordinates.y);

        if (actionBuffers.DiscreteActions[2] == 1)
        {
            if (Game.Player2CanShoot)
            {
                AddReward(1.0f);

                bool alreadyShot = false;

                for (int i = 0; i < firedXCoordinates.Count; i++)
                {
                    if (firedXCoordinates[i] == chosenCoordinates.x && firedYCoordinates[i] == chosenCoordinates.y)
                    {
                        alreadyShot = true;
                        break;
                    }
                }

                if (alreadyShot) //already shot there
                {
                    Debug.Log("Agent wanted to hit same target twice");
                    AddReward(-1.0f);
                }
                else
                {
                    for (int i = 0; i < firedXCoordinates.Count; i++)
                    {
                        if (firedXCoordinates[i] == -1)
                        {
                            firedXCoordinates[i] = chosenCoordinates.x;
                            break;
                        }
                    }
                    for (int i = 0; i < firedYCoordinates.Count; i++)
                    {
                        if (firedYCoordinates[i] == -1)
                        {
                            firedYCoordinates[i] = chosenCoordinates.y;
                            break;
                        }
                    }
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

                    if (Game.GameState == GameState.Completed)
                    {
                        if (Game.winner == Winner.Player2)
                        {
                            Debug.Log("Agent won the game");
                            SetReward(5.0f);
                        }
                        else if (Game.winner == Winner.Player1)
                        {
                            Debug.Log("Agent lost the game");
                        }
                        EndEpisode();
                    }
                }
            }
            else
            {
                Debug.Log("Agent fired too early");
                AddReward(-1.0f);
            }
        }

               
    }
}
