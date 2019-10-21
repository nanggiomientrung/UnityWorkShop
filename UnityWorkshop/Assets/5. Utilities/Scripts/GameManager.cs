using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [SerializeField]private BoardManager boardScript;
    public int score { get; private set; }
    private float time;

    void Awake()
    {
        // khởi tạo instance
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        //lấy reference của board manager (ngoài ra còn cách kéo thả thông qua inspector
        //boardScript = GetComponent<BoardManager>();

        // gọi hàm InitGame
        InitGame();
    }

    void InitGame()
    {
        // gọi sang Board Manager
        boardScript.SetupScene(3);
        score = 0;
        time = 0;
    }

    
    private void Update()
    {
        
    }

    public void PlayerDeath()
    {
        Debug.LogError("Player DEATHHHHHHHHHH");
    }
}
