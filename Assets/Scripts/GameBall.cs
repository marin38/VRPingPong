using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBall : MonoBehaviour {
    public BallState BallState {
        get { return _ballState; }
        set {
            _ballState = value;

            TextMesh tm = GameObject.FindGameObjectWithTag("New text").GetComponent<TextMesh>();
            tm.text = _ballState.ToString();
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
                    if (table.player != CurrentPlayer) // after hitting player hit to his own table side
                    {
                        BallState = BallState.HIT_OPPONENTS_FIELD;
                    }
                    else
                    {
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
                        BallState = BallState.OUT;
                    }
                    break;
                default:
                    BallState = BallState.OUT;
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

    static Game()
    {
        PlayerOneCount = 0;
        PlayerTwoCount = 0;
    }

    public static int PlayerOneCount { get; set; }
    public static int PlayerTwoCount { get; set; }

    public static bool IsGameFinshed()
    {
        return PlayerOneCount == SCORE_TO_WIN || PlayerTwoCount == SCORE_TO_WIN;
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