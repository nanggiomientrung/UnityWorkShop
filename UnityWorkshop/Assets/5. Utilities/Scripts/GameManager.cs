using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [SerializeField]private BoardManager boardScript;
    public int score { get; private set; }
    [SerializeField] private Text scoreText;
    private float time;
    [SerializeField] private MovingCamera cam;

    void Awake()
    {
        // khởi tạo instance
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        // gọi hàm InitGame
        InitGame();
    }

    void InitGame()
    {
        // gọi sang Board Manager
        boardScript.SetupScene(3);
        score = 0;
        time = 0;

        // khởi tạo score
        scoreText.text = "Score: 0";
    }

    // độ dài map
    private int mapLength;
    public int MapLength
    {
        get
        {
            return mapLength;
        }
        set
        {
            mapLength = value;
            cam.SetMapLength(mapLength);
        }
    }
    
    private void Update()
    {
        
    }

    public void PlayerDeath()
    {
        Debug.LogError("Player DEATHHHHHHHHHH");
    }

    public void IncreaseScore(int addedScore)
    {
        score += addedScore;
        scoreText.text = "Score: " + score;
    }
}
