using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject[] blockPrefabs;
    private Transform boardHolder; // gameobject để làm cha cho các object được sinh
    private List<LayoutElementPos> gridPositions = new List<LayoutElementPos>(); // danh sách các vị trí để init cho ground
    private LayoutData layoutData = new LayoutData();

    public void SetupScene(int Level)
    {
        ReadJsonLayoutData(Level);
        InitialsePositionList();
        BoardSetup();
    }

    void InitialsePositionList()
    {
        // độ dài map chính là số cột block của layout.
        GameManager.instance.MapLength = layoutData.LayoutGroups.Count;
        // khởi tạo gridPositions 
        foreach(KeyValuePair<int, List<LayoutElementPosY>> keyValuePair in layoutData.LayoutGroups)
        {
            for (int i = 0; i < keyValuePair.Value.Count; i++)
            {
                gridPositions.Add(new LayoutElementPos
                {
                    PosX = -9.1f + keyValuePair.Key * 1.3f,
                    PosY = -2 - keyValuePair.Value[i].PosY * 1.3f,
                    ElementNo = keyValuePair.Value[i].ElementNo
                });
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
            GameObject instance = Instantiate(blockPrefabs[gridPositions[i].ElementNo], new Vector3(gridPositions[i].PosX, gridPositions[i].PosY,-1), Quaternion.identity) as GameObject;
            instance.transform.SetParent(boardHolder);
        }
    }

    private void ReadJsonLayoutData(int Level)
    {
        var jsonTextFile = Resources.Load<TextAsset>("LayoutData/Level_" + Level).ToString();
        layoutData = JsonConvert.DeserializeObject<LayoutData>(jsonTextFile);
    }
}

public class LayoutData
{
    public Dictionary<int, List<LayoutElementPosY>> LayoutGroups;
}

public class LayoutElementPosY
{
    public int PosY; // vị trí theo Y được init (theo ô bàn cờ)
    public int ElementNo; // dùng để xác định được element nào sẽ được init ~ BlockID
}

public class LayoutElementPos
{
    public float PosX; // vị trí theo X được init
    public float PosY; // vị trí theo Y được init
    public int ElementNo; // dùng để xác định được element nào sẽ được init
}