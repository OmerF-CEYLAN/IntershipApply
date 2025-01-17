using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

    void Start()
    {

        SetBoard();
        FindBlockGroups();

        spawnHeight = new float[columnCount];

        SetSpritesBasedOnGroups();
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


}
