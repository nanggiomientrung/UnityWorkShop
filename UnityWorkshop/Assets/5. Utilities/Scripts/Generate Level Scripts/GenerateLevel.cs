using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GenerateLevel : MonoBehaviour
{
    [SerializeField] private Button generateButton;
    [SerializeField] private Transform elementParent;
    private Dictionary<int, List<LayoutElementPosY>> LayoutGroups = new Dictionary<int, List<LayoutElementPosY>>();
    ElementPosition currentPosition;
    void Start()
    {
        generateButton.onClick.AddListener(GenerateListElement);
    }


    private void GenerateListElement()
    {
        for (int i = 0; i < elementParent.childCount; i++)
        {
            currentPosition = GetElementPosition(elementParent.GetChild(i).position);
            if(LayoutGroups.ContainsKey(currentPosition.xPos))
            {
                LayoutGroups[currentPosition.xPos].Add(new LayoutElementPosY
                {
                    PosY = currentPosition.yPos,
                    ElementNo = elementParent.GetChild(i).GetComponent<ElementInfor>().GetBlockID()
                });
            }
            else
            {
                LayoutGroups.Add(currentPosition.xPos, new List<LayoutElementPosY>()
                {
                    new LayoutElementPosY
                    {
                        PosY = currentPosition.yPos,
                        ElementNo = elementParent.GetChild(i).GetComponent<ElementInfor>().GetBlockID()
                    }
                });
            }
        }
        layoutData.LayoutGroups = LayoutGroups;
        WriteJsonLayoutData();
    }

    private ElementPosition GetElementPosition(Vector3 elementPos)
    {
        return new ElementPosition { xPos = Mathf.RoundToInt((elementPos.x + 9.1f) / 1.3f), yPos = -Mathf.RoundToInt((elementPos.y + 2.6f) / 1.3f) };
    }


    private LayoutData layoutData = new LayoutData();
    private void WriteJsonLayoutData()
    {
        string connectionString = Application.persistentDataPath + "/" + "Level_.json";
        string json = JsonConvert.SerializeObject(layoutData);
        Debug.LogError(connectionString);
        File.WriteAllText(connectionString, json);
    }
}

public class ElementPosition
{
    public int xPos;
    public int yPos;
}
