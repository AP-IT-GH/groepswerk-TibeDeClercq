using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Player1Turn,
    Player2Turn
}

public class Zeeslag : MonoBehaviour
{
    private bool _gameStarted;
    private bool _player1Shot;
    private bool _player2Shot;

    public Field FieldPlayer1;
    public Field FieldPlayer2;

    public GameState GameState;

    private void Start()
    {
        this._gameStarted = false;
        this._player1Shot = false;
        this._player2Shot = false;
    }
    private void FixedUpdate()
    {
        if (!this._gameStarted)
        {
            this._gameStarted = true;
            //StartCoroutine(this.GameLogic());
        }
    }

    private IEnumerator GameLogic()
    {
        while (this._gameStarted)
        {
            while (this.GameState == GameState.Player1Turn)
            {
                Debug.Log("Player1Turn");
                if (this._player1Shot)
                {
                    this._player1Shot = false;
                    this.GameState = GameState.Player2Turn;
                    yield return new WaitForSeconds(2);
                }
            }

            while (this.GameState == GameState.Player2Turn)
            {
                Debug.Log("Player2Turn");
                if (this._player2Shot)
                {
                    this._player2Shot = false;
                    this.GameState = GameState.Player1Turn;
                    yield return new WaitForSeconds(2);
                }
            }
        }
    }

    public bool Player1Shoot(Vector2 coords)
    {
        if(this.GameState == GameState.Player1Turn)
        {
            bool result = this.FieldPlayer2.Shoot(coords);
            if (result)
            {
                this._player1Shot = true;
                return true;
            }
        }
        return false;
    }

    public bool Player2Shoot(Vector2 coords)
    {
        bool result = this.FieldPlayer1.Shoot(coords);
        if (result)
        {
            this._player2Shot = true;
            return true;
        }
        return false;
    }
}
