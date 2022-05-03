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

    public Field FieldPlayer1;
    public Field FieldPlayer2;

    public GameState GameState;

    private void Start()
    {
        this._gameStarted = false;
    }

    private void FixedUpdate()
    {
        if (!this._gameStarted)
        {
            this._gameStarted = true;
            StartCoroutine(this.GameLogic());
        }
    }

    private IEnumerator GameLogic()
    {
        switch (this.GameState)
        {
            case GameState.Player1Turn:
                Debug.Log("Player1Turn");
                yield return new WaitForSeconds(2);
                this.GameState = GameState.Player2Turn;
                break;
            case GameState.Player2Turn:
                Debug.Log("Player2Turn");
                yield return new WaitForSeconds(2);
                this.GameState = GameState.Player1Turn;
                break;
            default:
                break;
        }
        yield return null;
    }
}
