using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.Collections.AllocatorManager;

public class BlockPlacer : MonoBehaviour
{

    [SerializeField]
    Sprite[] sprites;

    [SerializeField]
    private GameObject blockPrefab;

    [SerializeField]
    private int rowCount;

    [SerializeField]
    private int columnCount;

    Block[,] blocks;

    Vector2[,] positions;

    [SerializeField]
    private float cubeDistance;

    void Start()
    {

        SetBoard();

    }


    void Update()
    {
        
    }

    private void SetBoard()
    {
        positions = new Vector2[rowCount, columnCount];

        blocks = new Block[rowCount, columnCount];

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                positions[i, j] = new Vector2(j*cubeDistance, i*cubeDistance);

                blocks[i, j] = Instantiate(blockPrefab, positions[i, j], Quaternion.identity).GetComponent<Block>();
            }
        }

    }
    

}
