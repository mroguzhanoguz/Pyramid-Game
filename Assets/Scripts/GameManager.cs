using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public enum State { WaitToStart, Playing, Completed, Failed, Transition };
    [SerializeField] private State state;
    public State GetCurrentState
    {
        get => state;
    }
    private bool isStarted = false;
    [SerializeField] private HumanPyramid humanPyramid;
    [SerializeField] private CameraSystem cam;
    [SerializeField] private Transform obstaclesParent;
    public Transform finishPointTransform;
    public delegate void GameStateEvent();
    public GameStateEvent OnInitialized, OnPlayStarted, OnCompleted, OnFailed;

    private void Start()
    {
        Initialize();
        StartState_WaitToStart();
    }

    private void Update()
    {
        if(!isStarted) { return; }

        switch(state)
        {
            case State.WaitToStart:
            UpdateState_WaitToStart();
            break;
            case State.Playing:
            UpdateState_Playing();
            break;
            case State.Completed:
            UpdateState_Completed();
            break;
            case State.Failed:
            UpdateState_Failed();
            break;
            default:
            break;
        }
    }

    private void FixedUpdate()
    {
        if(!isStarted) { return; }

        if(state == State.Playing)
        {
            humanPyramid.FixedUpdatePyramid();
            cam.FixedUpdateCamera();
        }
    }

    void Initialize()
    {
        humanPyramid.InitializePyramid();
        InitializeObstacles();
        isStarted = true;
        OnInitialized?.Invoke();
    }

    void InitializeObstacles()
    {
        for(int i = 0; i < obstaclesParent.childCount; i++)
        {
            obstaclesParent.GetChild(i).GetComponent<Obstacle>().Initialize(humanPyramid);
        }
    }
    void StartState_WaitToStart()
    {
        state = State.WaitToStart;
    }
    void UpdateState_WaitToStart()
    {
        if(Input.GetMouseButtonDown(0))
        {
            StartState_Playing();
        }
    }
    void StartState_Playing()
    {
        state = State.Playing;
        OnPlayStarted?.Invoke();

    }
    void UpdateState_Playing()
    {
        humanPyramid.UpdatePyramid();
    }
    public void OnStartedPlacingHumans()
    {
        state = State.Transition;
    }
    public void OnPlacingHumansCompleted()
    {
        StartState_Completed();
    }
    public void OnLostAllHumans()
    {
        StartState_Failed();
    }

    void StartState_Completed()
    {
        state = State.Completed;
        OnCompleted?.Invoke();

    }
    void UpdateState_Completed() { }

    void StartState_Failed()
    {
        state = State.Failed;
        OnFailed?.Invoke();

    }
    void UpdateState_Failed() { }






}

