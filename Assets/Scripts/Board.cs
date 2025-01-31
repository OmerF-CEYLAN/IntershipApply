using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.tvOS;
using UnityEngine.Windows;
using static Unity.Collections.AllocatorManager;
using static UnityEditor.Progress;

public class Board : MonoBehaviour
{
    [SerializeField]
    Block[] blocksTypes;

    [SerializeField]
    private GameObject blockPrefab;

    [SerializeField]
    private int rowCount;

    [SerializeField]
    private int columnCount;

    [SerializeField]
    Vector2 blockSpawnHeight;

    Block[,] blocks;

    Vector2[,] positions;

    [SerializeField]
    private float blockDistance;

    static float[] spawnHeight;

    [SerializeField]
    internal List<Sprite> sprites;

    [SerializeField]
    SpriteHolder spriteHolder;

    List<Block>[] blocksByColor;

    [SerializeField]
    int minCountToBeCollapsed;

    void Start()
    {

        SetBoard();
        FindBlockGroups();

        spawnHeight = new float[columnCount];

        blocksByColor = new List<Block>[blocksTypes.Count()];

        for (int i = 0; i < blocksByColor.Length; i++)
        {
            blocksByColor[i] = new List<Block>();
        }

        SetSpritesBasedOnGroups();

        SortByColor();

        DetectDeadLock();
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
                positions[i, j] = new Vector2(j * blockDistance, i * blockDistance);

                blocks[i, j] = Instantiate(blocksTypes[UnityEngine.Random.Range(0, blocksTypes.Length)], positions[i, j] + blockSpawnHeight, Quaternion.identity);

                blocks[i, j].SetRawAndColNo(i, j);

            }
        }
    }

    private void CollapseBlocks(int row,int column)
    {
        for (int i = row; i < rowCount; i++)
        {
            if (i == rowCount - 1)
            {
                blocks[i,column] = Instantiate(blocksTypes[UnityEngine.Random.Range(0,blocksTypes.Length)], positions[i, column] + new Vector2(0f, 5f + spawnHeight[column]) /*positions[i, column] + blockSpawnHeight  + new Vector2(0, cubeDistance * row)*/, Quaternion.identity);
                spawnHeight[column] += blockDistance;
            }
            else blocks[i, column] = blocks[i + 1, column];

            blocks[i,column].SetRawAndColNo(i,column);

        }  

    }
    

    public void ObserveBlockChanges(int rowOfBlock,int columnOfBlock)
    {
        CollapseBlocks(rowOfBlock,columnOfBlock);
        
    }

    public void ObserveBlockChanges()
    {
        ClearBlockGroups();
        FindBlockGroups();

        Array.Clear(spawnHeight,0,spawnHeight.Length);

        SetSpritesBasedOnGroups();

        SortByColor();

        DetectDeadLock();

    }


    public void FindBlockGroups()
    {
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                if(i == rowCount - 1)
                {
                    if(j == columnCount - 1)
                    {
                        break;
                    }
                    if ((blocks[i, j].GetColor() == blocks[i, j + 1].GetColor() && blocks[i, j].GetGroup() != blocks[i, j + 1].GetGroup()))
                    {
                        if (blocks[i, j + 1].GetGroup().Count > 1)
                        {
                            foreach (Block item in blocks[i, j].GetGroup())
                            {
                                blocks[i, j + 1].GetGroup().Add(item);
                                if (item == blocks[i, j]) continue;
                                item.SetGroup(blocks[i, j + 1].GetGroup());
                            }
                            blocks[i, j].SetGroup(blocks[i, j + 1].GetGroup());
                        }
                        else
                        {
                            blocks[i, j].GetGroup().Add(blocks[i, j + 1]);
                            blocks[i, j + 1].SetGroup(blocks[i, j].GetGroup());
                        }

                    }
                }
                else if(j == columnCount - 1)
                {
                    if (blocks[i, j].GetColor() == blocks[i + 1, j].GetColor())
                    {
                        blocks[i, j].GetGroup().Add(blocks[i + 1, j]);
                        blocks[i + 1, j].SetGroup(blocks[i, j].GetGroup());
                    }
                }
                else
                {
                    if (blocks[i, j].GetColor() == blocks[i + 1, j].GetColor())
                    {
                        blocks[i, j].GetGroup().Add(blocks[i + 1, j]);
                        blocks[i + 1, j].SetGroup(blocks[i, j].GetGroup());
                    }
                    if (blocks[i, j].GetColor() == blocks[i, j + 1].GetColor() && blocks[i, j].GetGroup() != blocks[i, j + 1].GetGroup())
                    {
                        if (blocks[i, j + 1].GetGroup().Count > 1)
                        {
                            foreach (Block item in blocks[i, j].GetGroup())
                            {
                                blocks[i, j + 1].GetGroup().Add(item);
                                if (item == blocks[i, j]) continue;
                                item.SetGroup(blocks[i, j + 1].GetGroup());
                            }
                            blocks[i, j].SetGroup(blocks[i,j +1].GetGroup());
                        }
                        else
                        {
                            blocks[i, j].GetGroup().Add(blocks[i, j + 1]);
                            blocks[i, j + 1].SetGroup(blocks[i, j].GetGroup());
                        }

                    }
                }


            }
        }

    }

    public void ClearBlockGroups()
    {
        foreach (Block b in blocks)
        {
            b.SetGroup(b.GetGroup().Where(x => x == b).ToList());
           
        }
    }

    public Vector2[,] GetPositions()
    {
        return positions;
    }

    void SetSpritesBasedOnGroups()
    {
        foreach (Block item in blocks)
        {
            Sprite[] currentSpriteArray = spriteHolder.GetSpriteArrayOfColor(item.GetColor());
            item.SetSprite(currentSpriteArray);
        }
    }

    void DetectDeadLock()
    {
        foreach (Block item in blocks)
        {
            if (item.GetGroup().Count > 1)
            {
                return;
                //no deadlock
            }
        }
        Debug.LogError("Deadlock var");

        Invoke("Shuffle",3f);


    }

    void Shuffle()
    {

        List<Block> temp = new List<Block>();

        while (temp.Count <= 1)
        {
            temp = blocksByColor[UnityEngine.Random.Range(0, blocksByColor.Length)]; //random color group with more then 2 blocks will be selected.
        }

        //önce random karýþtýr.

        int randomRow,randomColumn;

        randomRow = UnityEngine.Random.Range(0, rowCount);
        randomColumn = UnityEngine.Random.Range(0, columnCount);


        for (int i = 0; i < UnityEngine.Random.Range(2,temp.Count); i++)
        {
            //karýþtýrmadan sonra ayný renklten bir kaç kareyi patlayacak þekilde diz.

            if (blocks[randomRow,randomColumn] != null && i == 0)
            {
                SwapBlocks(temp[i].rowNo, temp[i].columnNo, randomRow, randomColumn);
            }
            else
            {
                SwapOneOfTheBlocksAround(temp[i].rowNo, temp[i].columnNo, randomRow, randomColumn);
            }

        }

        FindBlockGroups();

        SetSpritesBasedOnGroups();

        SortByColor();

    }

    void SortByColor()
    {

        foreach (List<Block> item in blocksByColor)
        {
            item.Clear();
        }

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {

                if (blocks[i,j].GetColor() == blocksTypes[0].GetColor())
                {
                    blocksByColor[0].Add(blocks[i,j]);
                }
                else if (blocks[i, j].GetColor() == blocksTypes[1].GetColor())
                {
                    blocksByColor[1].Add(blocks[i, j]);
                }
                else if (blocks[i, j].GetColor() == blocksTypes[2].GetColor())
                {
                    blocksByColor[2].Add(blocks[i, j]);
                }
                else if (blocks[i, j].GetColor() == blocksTypes[3].GetColor())
                {
                    blocksByColor[3].Add(blocks[i, j]);
                }
                else if (blocks[i, j].GetColor() == blocksTypes[4].GetColor())
                {
                    blocksByColor[4].Add(blocks[i, j]);
                }
                else  if (blocks[i, j].GetColor() == blocksTypes[5].GetColor())
                {
                    blocksByColor[5].Add(blocks[i, j]);
                }

            }
        }

        Debug.Log("Blue = " + blocksByColor[0].Count + " Green = " + blocksByColor[1].Count + " Pink = " + blocksByColor[2].Count + " \n Purple = " + blocksByColor[3].Count
            + "  Red = " + blocksByColor[4].Count + "  Yellow = " + blocksByColor[5].Count);

    }

    Block GetSpecificBlock(Block block)
    {
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                if (blocks[i, j] == block)
                {
                    return blocks[i, j];
                }
            }
        }
        return null;
    }

    void SwapBlocks(int rowToChange, int columnToChange, int rowToCome, int columnToCome)
    {
        Block temp;

        temp = blocks[rowToChange, columnToChange];

        blocks[rowToChange, columnToChange].SetRawAndColNo(rowToCome, columnToCome);

        blocks[rowToChange, columnToChange] = blocks[rowToCome, columnToCome];

        blocks[rowToCome, columnToCome].SetRawAndColNo(rowToChange, columnToChange);

        blocks[rowToCome,columnToCome] = temp;

    }

    void SwapOneOfTheBlocksAround(int rowToChange, int columnToChange, int rowToCome, int columnToCome)
    {
        bool isSwapDone = false;

        int i = 0;

        List<int> j = new List<int>{ 0, 1, 2, 3 };

        List<int> temp = j;

        if (rowToCome == 0)
        {

            // = new int[] { 0, 1, 3 };
            j.Remove(2);

        }
        if (rowToCome == rowCount - 1)
        {
            j.Remove(0);

        }
        if(columnToCome == 0)
        {
            j.Remove(3);
            //j = new int[] {0, 1, 2 };
        }
        if(columnToCome == columnCount - 1)
        {
            j.Remove(1);
            //j = new int[] {0, 2, 3 };     
        }

        i = j[UnityEngine.Random.Range(0, j.Count)];


        while (!isSwapDone)
        {
            

            switch (i)
            {
                case 0:

                    SwapBlocks(rowToChange, columnToChange, rowToCome + 1, columnToCome);
                    isSwapDone = true;

                    break;

                case 1:

                    SwapBlocks(rowToChange, columnToChange, rowToCome, columnToCome + 1);
                    isSwapDone = true;

                    break;

                case 2:

                    SwapBlocks(rowToChange, columnToChange, rowToCome - 1, columnToCome);
                    isSwapDone = true;

                    break;

                case 3:

                    SwapBlocks(rowToChange, columnToChange, rowToCome, columnToCome - 1);
                    isSwapDone = true;

                    break;
            }

        }

        

    }

    IEnumerator WaitForShuffle()
    {
        yield return new WaitForSeconds(3);
    }

   


}
