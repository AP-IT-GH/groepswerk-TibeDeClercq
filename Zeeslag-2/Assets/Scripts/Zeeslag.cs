using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum GameState
//{
//    Player1Turn,
//    Player2Turn
//}

public enum GameState
{
    InProgress,
    Completed
}

public enum Winner
{
    None,
    Player1,
    Player2
}

public class Zeeslag : MonoBehaviour
{
    [SerializeField] private bool player1CheatMode = false;
    [SerializeField] private bool player2CheatMode = false;
    [SerializeField] private ObservationGrid player1Grid;
    [SerializeField] private ObservationGrid player2Grid;
    [SerializeField] private GenerateField player1FieldGenerator;
    [SerializeField] private GenerateField player2FieldGenerator;
    [SerializeField] private BattleController player1BattleController;
    [SerializeField] private BattleController player2BattleController;
    public float player1ShootCooldown = 3;
    public float player2ShootCooldown = 3;

    private bool _gameStarted;
    private bool _player1Shot;
    private bool _player2Shot;
    public bool Player1CanShoot;
    public bool Player2CanShoot;

    public Field FieldPlayer1;
    public Field FieldPlayer2;

    public GameState GameState;
    public Winner winner;
    public bool GameRestarted = true;

    private void Start()
    {
        //this._gameStarted = false;
        this._player1Shot = false;
        this._player2Shot = false;
        this.Player1CanShoot = true;
        this.Player2CanShoot = true;
        this.winner = Winner.None;
        this.GameState = GameState.InProgress;
    }

    public void Restart()
    {
        GameRestarted = false;
        StartCoroutine(StartRestart());
    }

    public IEnumerator StartRestart()
    {
        Start();
        FieldPlayer1.ResetField();
        FieldPlayer2.ResetField();
        yield return new WaitForSeconds(0.5f);
        player2Grid.ResetGrid();
        player1Grid.ResetGrid();
        player1FieldGenerator.ResetField();
        player2FieldGenerator.ResetField();
    }
    private IEnumerator Player1Wait()
    {
        if (this._player1Shot)
        {
            this._player1Shot = false;
            this.Player1CanShoot = false;
            yield return new WaitForSeconds(player1ShootCooldown);
            this.Player1CanShoot = true;
        }
    }

    private IEnumerator Player2Wait()
    {
        if (this._player2Shot)
        {
            this._player1Shot = false;
            this.Player2CanShoot = false;
            yield return new WaitForSeconds(player2ShootCooldown);
            this.Player2CanShoot = true;
        }
    }

    public char Player1Shoot(Vector2 coords)
    {        
        if (this.Player1CanShoot)
        {
            Debug.Log("Player 1 Shooting");
            char result = this.FieldPlayer2.Shoot(coords);
            player1BattleController.Shoot(coords);
            this._player1Shot = true;
            UpdateGameState();
            StartCoroutine(Player1Wait());

            if (player1CheatMode && result == 'S')
            {
                player1Grid.RevealShip(coords);
            }

            return result;
        }
        return 'W';
    }

    public char Player2Shoot(Vector2 coords)
    {
        if (this.Player2CanShoot)
        {
            Debug.Log("Player 2 Shooting");
            char result = this.FieldPlayer1.Shoot(coords);
            player2BattleController.Shoot(coords);
            this._player2Shot = true;
            UpdateGameState();
            StartCoroutine(Player2Wait());

            if (player2CheatMode && result == 'S')
            {
                player2Grid.RevealShip(coords);
            }
            if(result == 'S')
            {
                OVRInput.SetControllerVibration(0.5f, 0.8f, OVRInput.Controller.LTouch);
                OVRInput.SetControllerVibration(0.5f, 0.8f, OVRInput.Controller.RTouch);
            }

            return result;
        }
        return 'W'; 
    }

    private void UpdateGameState()
    {
        if(GetHitCount(FieldPlayer1) == FieldPlayer1.GetShipPartCount())
        {
            winner = Winner.Player2;
            GameState = GameState.Completed;
            Debug.Log("Player 2 wins the game");
        }
        else if(GetHitCount(FieldPlayer2) == FieldPlayer2.GetShipPartCount())
        {
            winner = Winner.Player1;
            GameState = GameState.Completed;
            Debug.Log("Player 1 wins the game");
        }
    }

    private int GetHitCount(Field field)
    {
        int hitcount = 0;
        for (int x = 0; x < field.Values.GetLength(0); x++)
        {
            for (int y = 0; y < field.Values.GetLength(1); y++)
            {
                if (field.Values[x,y] == 'H')
                {
                    hitcount++;
                }
            }
        }
        return hitcount;
    }
}
