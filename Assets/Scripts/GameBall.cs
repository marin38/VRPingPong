using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBall : MonoBehaviour {
    public BallState BallState {
        get { return _ballState; }
        set {
            _ballState = value;
            LatestUpdateState = Time.time;

            GameObject gameObject = GameObject.FindGameObjectWithTag("New text");
            if (gameObject != null)
            {
                TextMesh tm = gameObject.GetComponent<TextMesh>();
                tm.text = _ballState.ToString();
            }
        }
    }

    private BallState _ballState;

    public float LatestUpdateState { get; set; }
    public Player CurrentPlayer { get; set; }

    // Use this for initialization
    void Start () {
        BallState = BallState.PITCHED_BY_PLAYER;
        LatestUpdateState = Time.time;
        CurrentPlayer = Player.PLAYER_TWO;
    }
	
	// Update is called once per frame
	void Update () {
        // TODO: handle the outs
        if (transform.position.y < 0.2)
        {
            switch (BallState)
            {
                case BallState.HIT_BY_PLAYER:
                    // the ball is hit by 
                    Game.IncreaseCount(Game.Opponent(CurrentPlayer));
                    BallState = BallState.OUT;
                    break;
                case BallState.HIT_OPPONENTS_FIELD:
                    // the ball is hit by 
                    Game.IncreaseCount(CurrentPlayer);
                    BallState = BallState.OUT;
                    break;
            }
        }

        if (Time.time - LatestUpdateState > 5.0)
        {
            BallState = BallState.OUT;
            LatestUpdateState = Time.time;
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        // TODO: collision either with a rocket or table

        TableSide table = collision.collider.GetComponent<TableSide>();
        if (table != null)
        {
            switch(BallState)
            {
                case BallState.HIT_BY_PLAYER:
                case BallState.PITCHED_AFTER_HITTING_PLAYERS_FIELD:
                    if (table.player != CurrentPlayer) 
                    {
                        BallState = BallState.HIT_OPPONENTS_FIELD;
                    }
                    else // after hitting player hit to his own table side
                    {
                        Game.IncreaseCount(Game.Opponent(CurrentPlayer));
                        BallState = BallState.OUT;
                    }
                    break;
                case BallState.PITCHED_BY_PLAYER:
                    if (table.player == CurrentPlayer)
                    {
                        BallState = BallState.PITCHED_AFTER_HITTING_PLAYERS_FIELD;
                    }
                    else
                    {
                        Game.IncreaseCount(Game.Opponent(CurrentPlayer));
                        BallState = BallState.OUT;
                    }
                    break;
            }
        }

        Racket racket = collision.collider.GetComponent<Racket>();
        if (racket != null)
        {
            switch(BallState)
            {
                case BallState.PITCHING_FLYING:
                    this.BallState = BallState.PITCHED_BY_PLAYER;
                    break;
                case BallState.HIT_OPPONENTS_FIELD:
                    if (CurrentPlayer != racket.player)
                    {
                        this.BallState = BallState.HIT_BY_PLAYER;
                        CurrentPlayer = racket.player;
                    } else
                    {
                        this.BallState = BallState.OUT;
                    }
                    break;
                default:
                    this.BallState = BallState.OUT;
                    break;
            }
        }

    }
}

public static class Game
{

    private static readonly int SCORE_TO_WIN = 21;

    private static readonly TextMesh player1Counter = GameObject.Find("Player1Counter").GetComponent<TextMesh>();
    private static readonly TextMesh player2Counter = GameObject.Find("Player2Counter").GetComponent<TextMesh>();

    static Game()
    {
        PlayerOneCount = 0;
        PlayerTwoCount = 0;
    }

    public static void IncreaseCount(Player player)
    {
        if (player == Player.PLAYER_ONE)
        {
            PlayerOneCount += 1;
        } else
        {
            PlayerTwoCount += 1;
        }
    }

    public static int PlayerOneCount {
        get { return _p1Count;  }
        set { _p1Count = value; player1Counter.text = _p1Count.ToString(); }
    }
    private static int _p1Count;

    public static int PlayerTwoCount {
        get { return _p2Count; }
        set { _p2Count = value;  player2Counter.text = _p2Count.ToString(); }
    }
    private static int _p2Count;

    public static bool IsGameFinshed()
    {
        return PlayerOneCount == SCORE_TO_WIN || PlayerTwoCount == SCORE_TO_WIN;
    }

    public static Player Opponent(Player player)
    {
        return Player.PLAYER_ONE == player ? Player.PLAYER_TWO : Player.PLAYER_ONE;
    }

    public static Player? getWinner()
    {
        if (!IsGameFinshed())
        {
            return null;
        }

        if (PlayerOneCount == SCORE_TO_WIN)
        {
            return Player.PLAYER_ONE;
        }

        return Player.PLAYER_TWO;
    }
}

public enum Player
{
    PLAYER_ONE, PLAYER_TWO
}

public enum BallState
{
    PITCHING_FLYING, // when a ball is flying and waiting to be hit by pitcher
    PITCHED_BY_PLAYER, // when a ball is hit during the pitch and not hit ground by now
    PITCHED_AFTER_HITTING_PLAYERS_FIELD, // after pitching we have a state when it is going to the opponent's field, same as HIT_BY_PLAYER except that we need to handle collision with net
    PITCHED_NET_COLLISION, // after pitching the net collided with the ball

    HIT_BY_PLAYER, // when a ball is going to hit the opponent's field
    HIT_OPPONENTS_FIELD, // when a ball 

    OUT // A ball in an out
}