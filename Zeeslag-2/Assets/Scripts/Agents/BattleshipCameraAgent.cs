using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class BattleshipCameraAgent : Agent
{
    public Zeeslag Game;
    public Vector2 chosenCoordinates;
    public char shotResult;

    public override void OnEpisodeBegin()
    {
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
        sensor.AddObservation(shotResult);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        chosenCoordinates = new Vector2(Mathf.Round(Mathf.Clamp(actionBuffers.ContinuousActions[0],0,1) * 9), Mathf.Clamp(actionBuffers.ContinuousActions[1],0,1) * 9);
        shotResult = Game.Player2Shoot(chosenCoordinates);

        if (shotResult == 'S')
        {
            Debug.Log("Agent hit a ship!");
            AddReward(1.0f);
        }
        else if (shotResult == 'H' || shotResult == 'M') // already hit there dummy try again
        {
            Debug.Log("Agent hit same target twice");
            AddReward(-1.0f);
            EndEpisode();
        }

        if (Game.GameState == GameState.Completed)
        {
            if (Game.winner == Winner.Player2)
            {
                Debug.Log("Agent won the game");
                SetReward(5.0f);
            }
            Debug.Log("Agent lost");
            EndEpisode();
        }
    }
}
