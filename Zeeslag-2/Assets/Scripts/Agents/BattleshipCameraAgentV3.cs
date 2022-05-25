using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class BattleshipCameraAgentV3 : Agent
{
    public Zeeslag Game;
    public Vector2 chosenCoordinates;
    public char shotResult;

    public override void OnEpisodeBegin()
    {
            Game.Restart();
            Debug.Log("Agent Restarted Game");
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Game.Player2CanShoot);
        sensor.AddObservation(chosenCoordinates);
        sensor.AddObservation(shotResult);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        if (Game.Player2CanShoot)
        {
            chosenCoordinates = new Vector2(actionBuffers.DiscreteActions[0], actionBuffers.DiscreteActions[1]);
            shotResult = Game.Player2Shoot(chosenCoordinates);

            if (shotResult == 'S' || shotResult == 'W')
            {
                Debug.Log("Agent hit something");
                AddReward(0.5f);
            }
            else if (shotResult == 'H' || shotResult == 'M')
            {
                Debug.Log("Agent hit same target twice");
                AddReward(-1.0f);
                EndEpisode();
            }

            if (Game.GameState == GameState.Completed)
            {
                AddReward(1.0f);
                EndEpisode();
            }
        }
    }
}
