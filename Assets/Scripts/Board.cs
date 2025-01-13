using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour, IBoardObserver
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
                positions[i, j] = new Vector2(j * cubeDistance, i * cubeDistance);

                blocks[i, j] = Instantiate(blockPrefab, positions[i, j], Quaternion.identity).GetComponent<Block>();

                blocks[i, j].SetRawAndColNo(i, j);

            }
        }
    }

    private void FillWithBlocks(int row,int column)
    {
        for (int i = row; i < rowCount; i++)
        {
            if (i == rowCount - 1)
            {
                blocks[i,column] = Instantiate(blockPrefab, positions[i, column], Quaternion.identity).GetComponent<Block>();
            }
            else blocks[i, column] = blocks[i + 1, column];

            blocks[i, column].gameObject.transform.position = positions[i, column];

            blocks[i,column].SetRawAndColNo(i,column);

        }


        

    }

    public void ObserveBlockChanges(int rowOfBlock,int columnOfBlock)
    {
        FillWithBlocks(rowOfBlock,columnOfBlock);
    }

    public Sprite GetRandomSprite()
    {
        return sprites[Random.Range(0,sprites.Length)];
    }


}
