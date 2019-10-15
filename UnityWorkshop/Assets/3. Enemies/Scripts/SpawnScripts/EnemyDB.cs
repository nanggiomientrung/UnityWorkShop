using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/EnemyDB", fileName = "EnemyDB.asset")]
public class EnemyDB : SerializedScriptableObject
{
    public List<EnemyInfo> listEnemy;

    public BaseEnemy GetEnemyByID(int ID)
    {
        for (int i = 0; i < listEnemy.Count; i++)
        {
            if (listEnemy[i].EnemyID == ID)
                return listEnemy[i].EnemyPrefab;
        }
        return null;
    }

    public BaseEnemy GetEnemyByIndex(int Index)
    {
        if (Index < 0 || Index >= listEnemy.Count)
            return null;
        else return listEnemy[Index].EnemyPrefab;
    }
}

[System.Serializable]
public class EnemyInfo
{
    public int EnemyID;
    public string EnemyName;
    public BaseEnemy EnemyPrefab;
    public int EmemyLife;
    public int EnemyDmg;
    public Sprite EnemyIcon;
}