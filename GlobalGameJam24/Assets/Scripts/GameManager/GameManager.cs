using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private GameObject m_AIPrefab;

    private bool ai;

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

    public int[] pRdsWon = new int[2];

    public GameObject ScoreUI;
    public TextMeshProUGUI[] ScoreCounterText;


    private GameObject[] players = new GameObject[2];

    private bool playBGM = false;

    private AudioSource BGM;

    enum MenuChoices
    {
        StartGame,
        ControlScreen,
        Credits,
        OnePlayer,
        TwoPlayer,
        MainMenu,
        PlayAgain
    }

    enum GameStates
    {
        Playing,
        GameOver,
        PlayerSelect,
        ControlScreen,
        MainMenu,
        Paused
    }

    private string[] cursorTags = {"MMStartGame", "MMCredits"};

    private GameStates gameState;

    private MenuChoices menuChoice;

    [SerializeField]
    private GameObject m_mainMenu;

    [SerializeField]
    private GameObject m_MMStartGame;

    [SerializeField]
    private GameObject m_MMCredits;

    [SerializeField]
    private GameObject m_playerSelectMenu;

    [SerializeField]
    private GameObject m_MM1PSelect;

    [SerializeField]
    private GameObject m_MM2PSelect;

    [SerializeField]
    private GameObject m_instructionScreen;

    [SerializeField]
    private GameObject m_WinningScreen;

    [SerializeField]
    private TextMeshProUGUI m_winningText;

    [SerializeField]
    private GameObject m_playAgainCursor;

    [SerializeField]
    private GameObject m_mainMenuCursor;



    private void Start() {
        // Start Background Ambience
        SoundManager._instance.PlayBGAmbience();
        BGM = SoundManager._instance.PlayBGM();
        BGM.Stop();

        gameState = GameStates.MainMenu;
        menuChoice = MenuChoices.StartGame;

        startMainMenu();
    }

    private void startMainMenu() {
        BGM.Stop();
        gameState = GameStates.MainMenu;
        menuChoice = MenuChoices.StartGame;
        renderMainMenuUI();
    }

    public void RoundEnd(int player) {
        foreach (GameObject p in players) {
            Destroy(p);
        }

        if (++pRdsWon[player] >= m_rdsToWin) { // WIN CON.
            pRdsWon = new int[2];
            Debug.Log("Player " + (player + 1) + " won!");
            gameState = GameStates.GameOver;
            menuChoice = MenuChoices.StartGame;
            renderGameOverScreen(player == 0 ? "Red wins" : "Blue wins");
            return;
        }

        playRdEndAnimation();
        spawnPlayers();
    }

    private void playRdEndAnimation() {
        
    }

    private void spawnPlayers() {
        players[0] = Instantiate(m_p1Prefab, m_p1StartPos, new Quaternion());
        players[1] = Instantiate(ai ? m_AIPrefab : m_p2Prefab, m_p2StartPos, new Quaternion());
    }

    private void startRound() {
        gameState = GameStates.Playing;
        pRdsWon = new int[2];
		ScoreUI.SetActive(true);

		BGM.Play();
        spawnPlayers();
    }

    private void handleMainMenuInput() {

        bool startGame = Input.GetKeyDown(KeyCode.Return) 
            && menuChoice == MenuChoices.StartGame;
        bool credits = Input.GetKeyDown(KeyCode.Return)
            && menuChoice == MenuChoices.Credits;

        if (startGame) {
            // GameObject.FindGameObjectWithTag("MMStartGame").SetActive(false);
            m_mainMenu.SetActive(false);
            renderPlayerSelectMenu();
            SoundManager._instance.PlayOarCollisionSFX();
            // startRound();
            return;
        }

        if (credits) {
            // do credits
            m_mainMenu.SetActive(false);
            GameObject.FindGameObjectWithTag("MMCredits").SetActive(false);
            SoundManager._instance.PlayOarCollisionSFX();
            return;
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A)) {
            menuChoice = menuChoice == MenuChoices.StartGame ? 
                MenuChoices.Credits : MenuChoices.StartGame;
            changeCursor();
        }

    }

    private void changeCursor() {
        foreach (string tag in cursorTags)
            GameObject.FindGameObjectWithTag(tag)?.SetActive(false);
    
        switch (menuChoice) {
            case MenuChoices.StartGame:
                m_MMStartGame.SetActive(true);
                break;
            case MenuChoices.Credits:
                m_MMCredits.SetActive(true);
                break;
            case MenuChoices.ControlScreen:
                break;
            case MenuChoices.OnePlayer:
                m_MM2PSelect.SetActive(false);
                m_MM1PSelect.SetActive(true);
                break;
            case MenuChoices.TwoPlayer:
                m_MM1PSelect.SetActive(false);
                m_MM2PSelect.SetActive(true);
                break;
            case MenuChoices.MainMenu:
                m_playAgainCursor.SetActive(false);
                m_mainMenuCursor.SetActive(true);
                break;
            case MenuChoices.PlayAgain:
                m_mainMenuCursor.SetActive(false);
                m_playAgainCursor.SetActive(true);
                break;
        }
    }

    private void handlePauseMenuInput() {
    
    }


    private void renderGameOverScreen(string winner) {
        m_WinningScreen.SetActive(true);
        m_winningText.text = winner;
        m_playAgainCursor.SetActive(true);
        m_mainMenuCursor.SetActive(false);
    }

    private void renderMainMenuUI() {
        m_mainMenu.SetActive(true);
        m_MMCredits.SetActive(false);
        m_MMStartGame.SetActive(true);
        menuChoice = MenuChoices.StartGame;
    }

    private void renderPlayerSelectMenu() {
        gameState = GameStates.PlayerSelect;
        m_playerSelectMenu.SetActive(true);
        m_MM1PSelect.SetActive(true);
        m_MM2PSelect.SetActive(false);
        menuChoice = MenuChoices.OnePlayer;
    }


    private void renderControlsMenu() {
        gameState = GameStates.ControlScreen;
        menuChoice = MenuChoices.ControlScreen;
        m_instructionScreen.SetActive(true);
    }

    private void handleGameOverInput() {
        // TODO STUB
        // if (Input.GetKey(KeyCode.Return)) {
        //     startRound();
        // } else if (Input.GetKey(KeyCode.Escape)) {
        //     gameState = GameStates.MainMenu;
        //     BGM.Stop();
        // }

        bool playAgain = Input.GetKeyDown(KeyCode.Return) 
            && menuChoice == MenuChoices.PlayAgain;
        bool mainMenu = Input.GetKeyDown(KeyCode.Return)
            && menuChoice == MenuChoices.MainMenu;
        
        if (playAgain) {
            m_WinningScreen.SetActive(false);
            startRound();
            return;
        }

        if (mainMenu) {
            m_WinningScreen.SetActive(false);
            startMainMenu();
            return;
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A)) {
            menuChoice = menuChoice == MenuChoices.PlayAgain ? 
                MenuChoices.MainMenu : MenuChoices.PlayAgain;
            changeCursor();
        }
    
    }

    private void handlePlayerSelect() {
        bool onePlayer = Input.GetKeyDown(KeyCode.Return) 
            && menuChoice == MenuChoices.OnePlayer;
        bool twoPlayer = Input.GetKeyDown(KeyCode.Return)
            && menuChoice == MenuChoices.TwoPlayer;

        if (onePlayer) {
            gameState = GameStates.ControlScreen;
            m_playerSelectMenu.SetActive(false);
            SoundManager._instance.PlayOarCollisionSFX();
            ai = true;
            startInstructions();
            // startRound();
            return;
        }

        if (twoPlayer) {
            gameState = GameStates.ControlScreen;
            m_playerSelectMenu.SetActive(false);
            SoundManager._instance.PlayOarCollisionSFX();
<<<<<<< HEAD
            ai = false;
            startInstructions();
            // startRound();
            return;
=======
			startInstructions();
			// startRound();
			return;
>>>>>>> 13f608949bf8788d75b3569d13a4b9efb55ec72b
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A)) {
            menuChoice = menuChoice == MenuChoices.OnePlayer ? 
                MenuChoices.TwoPlayer : MenuChoices.OnePlayer;
            changeCursor();
        }
    }


    private void startInstructions() {
        renderControlsMenu();
    }

    private void handleControlScreen() {
        if (Input.anyKeyDown) {
            m_instructionScreen.SetActive(false);
            BGM.Play();
            startRound();
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
            case GameStates.PlayerSelect:
                handlePlayerSelect();
                break;
            case GameStates.ControlScreen:
                handleControlScreen();
                break;
            case GameStates.Playing:
                ScoreCounterText[0].text = pRdsWon?[0].ToString();
                ScoreCounterText[1].text = pRdsWon?[1].ToString();
				break;
            case GameStates.GameOver:
                handleGameOverInput();
                break;
        }
        
    }
}
