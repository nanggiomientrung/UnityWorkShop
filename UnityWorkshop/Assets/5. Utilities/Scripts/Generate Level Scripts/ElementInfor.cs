using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementInfor : MonoBehaviour
{
    // class dùng để attach với các block để gen vị trí và loại block cho Generate Level
    [SerializeField] private int blockID;
    public int GetBlockID()
    {
        return blockID;
    }
}
