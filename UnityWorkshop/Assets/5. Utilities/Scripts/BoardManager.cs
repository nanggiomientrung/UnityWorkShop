using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject[] blockPrefabs;
    private Transform boardHolder; // gameobject để làm cha cho các object được sinh
    private List<Vector3> gridPositions = new List<Vector3>(); // danh sách các vị trí để init cho ground

    public void SetupScene()
    {
        InitialsePositionList();
        BoardSetup();
    }

    void InitialsePositionList()
    {
        // khởi tạo gridPositions 
        // có thể tạo từ một file có sẵn
        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                gridPositions.Add(new Vector3(-10 + i * 1.3f, -2 - j * 1.3f));
            }
        }
    }


    void BoardSetup()
    {
        // tạo boardHolder
        boardHolder = new GameObject("Board").transform;
        // khởi tạo ground
        for (int i = 0; i < gridPositions.Count; i++)
        {
            GameObject instance = Instantiate(blockPrefabs[Random.Range(0, blockPrefabs.Length)], gridPositions[i], Quaternion.identity) as GameObject;
            instance.transform.SetParent(boardHolder);
        }
    }

    //private void Start()
    //{
    //    SetupScene();
    //}
}
