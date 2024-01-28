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

    private void Start() {
        spawnPlayers();
    }

    public void RoundEnd(int player) {
        if (++pRdsWon[player] >= m_rdsToWin) {
            pRdsWon = new int[2];
            Debug.Log("Player " + (player + 1) + " won!");
        }

        foreach (Object p in players) {
            Destroy(p);
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
