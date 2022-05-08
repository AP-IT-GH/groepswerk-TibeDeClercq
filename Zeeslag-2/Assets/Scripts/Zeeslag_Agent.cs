using Unity.MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class Zeeslag_Agent : Agent
{
    public Zeeslag Zeeslag;

    private int _chosenX;
    private int _chosenY;
    private int _shotsHit;

    public override void OnEpisodeBegin()
    {
        Zeeslag.ResetGame();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Zeeslag);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        this._chosenX = (int)(Mathf.Abs(actions.ContinuousActions[0]) * 9);
        this._chosenY = (int)(Mathf.Abs(actions.ContinuousActions[1]) * 9);

        ShootResult result = Zeeslag.Player2Shoot(new Vector2(this._chosenX, this._chosenY));

        switch (result)
        {
            case ShootResult.Hit:
                AddReward(1f);
                this._shotsHit++;
                Debug.Log("Agent hit");
                break;
            case ShootResult.Miss:
                //Debug.Log("Agent miss");
                break;
            case ShootResult.Double:
                SetReward(0f);
                //Debug.Log("Agent double");
                break;
            case ShootResult.Illegal:
                //Debug.Log("Agent Illegal");
                SetReward(0);
                break;
            default:
                break;
        }

        if(this._shotsHit == Zeeslag.FieldPlayer1.BigShipCount * 4 + Zeeslag.FieldPlayer1.SmallShipCount * 2)
        {
            Debug.Log("All targets hit");
            EndEpisode();
        }        
    }
}
