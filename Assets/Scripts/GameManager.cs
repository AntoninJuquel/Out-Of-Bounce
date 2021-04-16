using System;
using System.Collections;
using Systems.Chunk;
using Ball;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition;
    private Rigidbody2D _firstBallRb;
    public static GameStatus GameStatus;

    private void Awake()
    {
        GameStatus = GameStatus.Starting;
    }

    private void Start()
    {
        ChunkManager.Instance.StartChunk(startPosition);
        BallManager.Instance.SpawnBall(startPosition, out _firstBallRb);
        CameraController.Instance.SetTarget(_firstBallRb);
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
    Starting,
    Playing,
    Paused,
    GameOver
}