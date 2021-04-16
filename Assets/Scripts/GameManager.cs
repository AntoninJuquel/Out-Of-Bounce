using System;
using System.Collections;
using Systems.Chunk;
using Ball;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition;
    private ChunkManager _chunkManager;
    private BallManager _ballManager;

    private Rigidbody2D _firstBallRb;

    public static GameStatus GameStatus;

    private void Awake()
    {
        GameStatus = GameStatus.Starting;
    }

    private void Start()
    {
        _chunkManager = ChunkManager.Instance;
        _ballManager = BallManager.Instance;
        _chunkManager.StartChunk(startPosition);
        _ballManager.SpawnBall(startPosition, out _firstBallRb);
        StartCoroutine(StartRoutine());
    }

    private IEnumerator StartRoutine()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        GameStatus = GameStatus.Playing;
        _firstBallRb.simulated = true;
    }
}

public enum GameStatus
{
    Starting, Playing, Paused, GameOver
}