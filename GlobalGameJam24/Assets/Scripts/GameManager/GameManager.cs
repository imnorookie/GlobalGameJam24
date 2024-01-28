using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private int m_rdsToWin = 2;

    [SerializeField]
    private GameObject m_p1Prefab;

    [SerializeField]
    private Vector3 m_p1StartPos;

    [SerializeField]
    private GameObject m_p2Prefab;

    [SerializeField]
    private Vector3 m_p2StartPos;

    private int[] pRdsWon = new int[2];
    private GameObject[] players = new GameObject[2];

    private bool playBGM = false;

    private AudioSource BGM;

    enum GameStates
    {
        Playing,
        GameOver,
        MainMenu,
        Paused
    }

    private GameStates gameState;

    private void Start() {
        // Start Background Ambience
        SoundManager._instance.PlayBGAmbience();
        BGM = SoundManager._instance.PlayBGM();
        BGM.Stop();

        // Start main menu
        gameState = GameStates.MainMenu;
    }

    public void RoundEnd(int player) {
        foreach (Object p in players) {
            Destroy(p);
        }

        if (++pRdsWon[player] >= m_rdsToWin) { // WIN CON.
            pRdsWon = new int[2];
            Debug.Log("Player " + (player + 1) + " won!");
            gameState = GameStates.GameOver;
            return;
        }

        playRdEndAnimation();
        spawnPlayers();
    }

    private void playRdEndAnimation() {
        
    }

    private void spawnPlayers() {
        players[0] = Instantiate(m_p1Prefab, m_p1StartPos, new Quaternion());
        players[1] = Instantiate(m_p2Prefab, m_p2StartPos, new Quaternion());
    }

    private void startRound() {
        gameState = GameStates.Playing;
        BGM.Play();
        spawnPlayers();
    }

    private void handleMainMenuInput() {
        // TODO STUB
        if (Input.GetKey(KeyCode.Return)) {
            startRound();
        }

    }

    private void handlePauseMenuInput() {

    }

    private void handleGameOverInput() {
        // TODO STUB
        if (Input.GetKey(KeyCode.Return)) {
            startRound();
        } else if (Input.GetKey(KeyCode.Escape)) {
            gameState = GameStates.MainMenu;
            BGM.Stop();
        }
    }

    // Update is called once per frame
    void Update() {
        switch (gameState) {
            case GameStates.MainMenu:
                handleMainMenuInput();
                break;
            case GameStates.Paused:
                handlePauseMenuInput();
                break;
            case GameStates.Playing:
                break;
            case GameStates.GameOver:
                handleGameOverInput();
                break;
        }
        
    }
}
