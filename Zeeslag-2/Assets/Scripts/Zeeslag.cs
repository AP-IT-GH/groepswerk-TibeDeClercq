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
    [SerializeField] ObservationGrid player1Grid;
    [SerializeField] ObservationGrid player2Grid;
    [SerializeField] GenerateField player1FieldGenerator;
    [SerializeField] GenerateField player2FieldGenerator;
    [SerializeField] float player1ShootCooldown = 3;
    [SerializeField] float player2ShootCooldown = 3;

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
    //private void FixedUpdate()
    //{
    //    if (!this._gameStarted)
    //    {
    //        this._gameStarted = true;
    //        //StartCoroutine(this.GameLogic());
    //    }
    //}

    //private IEnumerator GameLogic()
    //{
    //    while (this._gameStarted)
    //    {
    //        while (this.GameState == GameState.Player1Turn)
    //        {
    //            Debug.Log("Player1Turn");
    //            if (this._player1Shot)
    //            {
    //                this._player1Shot = false;
    //                this.GameState = GameState.Player2Turn;
    //                yield return new WaitForSeconds(2);
    //            }
    //        }

    //        while (this.GameState == GameState.Player2Turn)
    //        {
    //            Debug.Log("Player2Turn");
    //            if (this._player2Shot)
    //            {
    //                this._player2Shot = false;
    //                this.GameState = GameState.Player1Turn;
    //                yield return new WaitForSeconds(2);
    //            }
    //        }
    //    }
    //}

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
            this._player1Shot = true;
            UpdateGameState();
            StartCoroutine(Player1Wait());

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
            this._player2Shot = true;
            UpdateGameState();
            StartCoroutine(Player2Wait());

            return result;
        }
        return 'W'; 
    }

    private void UpdateGameState()
    {
        if(GetHitCount(FieldPlayer1) == (FieldPlayer1.BigShipCount * 4) + (FieldPlayer1.SmallShipCount * 2))
        {
            winner = Winner.Player2;
            GameState = GameState.Completed;
        }
        else if(GetHitCount(FieldPlayer2) == (FieldPlayer2.BigShipCount * 4) + (FieldPlayer2.SmallShipCount * 2))
        {
            winner = Winner.Player1;
            GameState = GameState.Completed;
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

    //public bool Player1Shoot(Vector2 coords)
    //{
    //    Debug.Log("Player 1 Shooting");
    //    if(this.GameState == GameState.Player1Turn)
    //    {
    //        bool result = this.FieldPlayer2.Shoot(coords);
    //        if (result)
    //        {
    //            this._player1Shot = true;
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    //public bool Player2Shoot(Vector2 coords)
    //{
    //    Debug.Log("Player 2 Shooting");
    //    bool result = this.FieldPlayer1.Shoot(coords);
    //    if (result)
    //    {
    //        this._player2Shot = true;
    //        return true;
    //    }
    //    return false;
    //}
}
