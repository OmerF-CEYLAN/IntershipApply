using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Unity.Collections.AllocatorManager;
using static UnityEditor.Progress;

public class Board : MonoBehaviour
{
    [SerializeField]
    Block[] blocksToCreate;

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
        FindBlockGroups();

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

                blocks[i, j] = Instantiate(blocksToCreate[Random.Range(0, blocksToCreate.Length)], positions[i, j], Quaternion.identity);

                blocks[i, j].SetRawAndColNo(i, j);

            }
        }
    }

    private void CollapseBlocks(int row,int column)
    {
        for (int i = row; i < rowCount; i++)
        {
            if (i == rowCount - 1)
            {//Daha sonra fill with blocks ile yapýlacak
                blocks[i,column] = Instantiate(blocksToCreate[Random.Range(0,blocksToCreate.Length)], positions[i, column], Quaternion.identity);
            }
            else blocks[i, column] = blocks[i + 1, column];

            blocks[i, column].gameObject.transform.position = positions[i, column];

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
                    if (blocks[i, j].GetColor() == blocks[i, j + 1].GetColor())
                    {
                        if(blocks[i, j + 1].GetGroup().Count > 1)
                        {
                            //foreach (Block item in blocks[i,j].GetGroup())
                            //{
                            //    blocks[i,j + 1].GetGroup().Add(item);
                            //    item.SetGroup(blocks[i,j + 1].GetGroup());
                            //}
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
                    if (blocks[i, j].GetColor() == blocks[i, j + 1].GetColor())
                    {
                        if (blocks[i, j + 1].GetGroup().Count > 1)
                        {
                            //foreach (Block item in blocks[i, j].GetGroup())
                            //{
                            //    blocks[i, j + 1].GetGroup().Add(item);
                            //    item.SetGroup(blocks[i, j + 1].GetGroup());
                            //}
                            //for (int k = 0; k < blocks[i, j].GetGroup().Count; k++)
                            //{
                            //    blocks[i, j + 1].GetGroup().Add(blocks[i, j].GetGroup()[k]);
                            //    blocks[i, j].GetGroup()[k].SetGroup(blocks[i, j + 1].GetGroup());
                            //}
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


}
