using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private BoardManager boardScript;
    [SerializeField] private Text ScoreText;
    private int score;
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
        boardScript = GetComponent<BoardManager>();

        // gọi hàm InitGame
        InitGame();
    }

    void InitGame()
    {
        // gọi sang Board Manager
        boardScript.SetupScene();
        score = 0;
        time = 0;
        ScoreText.text = score.ToString();
    }

    
    private void Update()
    {
        if(time>=1f)
        {
            time += Time.deltaTime - 1;
            score += 50;
            ScoreText.text = score.ToString();
        }
        else
        {
            time += Time.deltaTime;
        }
    }
}
