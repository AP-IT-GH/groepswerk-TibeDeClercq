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
    public float OutlineSize = 1.1f;
    public Material TransparentWaterMaterial;
    public Color OutlineColor = Color.white;
    public float PulsateSpeed = 1f;

    private float TargetAlpha = 1f;
    private float minAplha = 0.3f;
    private float maxAplha = 1f;

    private bool isFiring = false;
    private bool rotate = false;

    private void Start()
    {
        Renderer = gameObject.GetComponent<Renderer>();
    }

    private void Update()
    {
        TempSelect(); // TEMP

        if (isFiring)
        {
            var color = Renderer.material.color;
            color.a = Mathf.MoveTowards(color.a, TargetAlpha, Time.deltaTime * PulsateSpeed);
            if (color.a >= maxAplha)
            {
                color.a = maxAplha;
                TargetAlpha = minAplha;
            }
            else if (color.a <= minAplha)
            {
                color.a = minAplha;
                TargetAlpha = maxAplha;
            }
            Renderer.material.color = color;
        }

        if (rotate)
        {
            observationGrid.game.player1BattleController.RotateAllCannons();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Finger")
        {
            Select();
        }
    }

    private void TempSelect()
    {
        if (Selected) // TEMP
        {
            Select();
        }
    }

    public void Select()
    {
        switch (observationGrid.player)
        {
            case Players.Player1:
                if (state == CubeState.Water && observationGrid.game.Player1CanShoot && observationGrid.game.GameState == GameState.InProgress)
                {
                    observationGrid.game.Player1Shoot(coordinates);
                    Renderer.material = TransparentWaterMaterial;
                    RemoveOutline();
                    isFiring = true;                 
                    StartCoroutine(Cooldown());
                }
                break;
            case Players.Player2:                
                observationGrid.game.Player2Shoot(coordinates);
                break;
        }
    }

    public void RotateTowardsTarget()
    {
        observationGrid.game.player1BattleController.GetAllValidCannons();
        observationGrid.game.player1BattleController.GetTarget(coordinates);

        rotate = true;
    }

    public void StopRotate()
    {
        rotate = false;
    }

    public void CreateOutline()
    {
        if (!isFiring && state == CubeState.Water)
        {
            Renderer.materials[1].SetColor("_OutlineColor", OutlineColor);
            Renderer.materials[1].SetFloat("_ScaleFactor", OutlineSize);
        }        
    }

    public void RemoveOutline()
    {
        Renderer.materials[1].SetFloat("_ScaleFactor", 0);
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(observationGrid.game.player1ShootCooldown);
        isFiring = false;
    }
}
