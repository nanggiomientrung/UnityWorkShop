using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/TestDBA", fileName = "TestDB.asset")]
public class TestScriptableObject : ScriptableObject
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
