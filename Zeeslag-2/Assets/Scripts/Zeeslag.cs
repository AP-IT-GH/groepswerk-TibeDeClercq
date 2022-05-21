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
    Paused,
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
    [SerializeField] private bool play = false; //TEMP
    [SerializeField] private bool pause = false; //TEMP
    [SerializeField] private bool restart = false; //TEMP

    [SerializeField] private bool player1CheatMode = false;
    [SerializeField] private bool player2CheatMode = false;
    [SerializeField] private Agent_Evert agent;
    [SerializeField] private ObservationGrid player1Grid;
    [SerializeField] private ObservationGrid player2Grid;
    [SerializeField] private GenerateField player1FieldGenerator;
    [SerializeField] private GenerateField player2FieldGenerator;
    [SerializeField] private BattleController player1BattleController;
    [SerializeField] private BattleController player2BattleController;
    [SerializeField] private int player1MultishotCount = 1;
    [SerializeField] private int player2MultishotCount = 1;
    public float player1ShootCooldown = 3;
    public float player2ShootCooldown = 3;

    private bool _player1Shot;
    private bool _player2Shot;    

    public Field FieldPlayer1;
    public Field FieldPlayer2;

    [HideInInspector] public bool Player1CanShoot;
    [HideInInspector] public bool Player2CanShoot;
    [HideInInspector] public GameState GameState;
    [HideInInspector] public Winner winner;
    [HideInInspector] public bool GameRestarted = true;

    private void Start()
    {
        this._player1Shot = false;
        this._player2Shot = false;
        this.Player1CanShoot = true;
        this.Player2CanShoot = true;
        this.winner = Winner.None;
        this.GameState = GameState.Paused;
    }

    private void Update() //TEMP
    {
        if (restart)
        {
            Restart();
            restart = false;
        }

        if (play)
        {
            play = false;
            Play();
        }

        if (pause)
        {
            pause = false;
            Pause();
        }
    }

    public void Play()
    {
        if (GameState == GameState.Paused)
        {
            this.GameState = GameState.InProgress;
            this.agent.Paused = false;
        }        
    }

    public void Pause()
    {
        if (GameState == GameState.InProgress)
        {
            this.GameState = GameState.Paused;
            this.agent.Paused = true;
        }        
    }

    public void Restart()
    {
        if (GameState == GameState.Paused || GameState == GameState.Completed)
        {
            GameRestarted = false;
            StartCoroutine(StartRestart());
        }           
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
        agent.gameObject.SetActive(false);
        agent.gameObject.SetActive(true);
        GameState = GameState.InProgress;
        agent.Paused = false;
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
            player1BattleController.Shoot(coords, player1MultishotCount);
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
            player2BattleController.Shoot(coords, player2MultishotCount);
            this._player2Shot = true;
            UpdateGameState();
            StartCoroutine(Player2Wait());

            if (player2CheatMode && result == 'S')
            {
                player2Grid.RevealShip(coords);
            }
            if(result == 'S')
            {
                OVRInput.SetControllerVibration(0.8f, 0.8f, OVRInput.Controller.LTouch);
                OVRInput.SetControllerVibration(0.8f, 0.8f, OVRInput.Controller.RTouch);
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
