using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class BlockPlacer : MonoBehaviour
{

    bool isDrawed = false;

    [SerializeField]
    Sprite[] sprites;

    List<List<Vector2>> blockPoses = new List<List<Vector2>>();

    Dictionary<Vector2, GameObject> posesAndBlocks;

    [SerializeField]
    private List<GameObject> blocks;

    [SerializeField]
    private GameObject blockPrefab;

    [SerializeField]
    private int rowCount;

    [SerializeField]
    private int columnCount;

    private 

    void Start()
    {

        for (int i = 0; i < rowCount; i++)
        {
            blockPoses.Add(new List<Vector2>());

            for (int j = 0; j < columnCount; j++)
            { 
                blockPoses[i].Add(new Vector2(j*2.2f,i*2.2f));
            }

        }

        Debug.Log(blockPoses.Count);

        foreach (List<Vector2> poses in blockPoses)
        {
            foreach (Vector2 pos in poses)
            {
                blocks.Add(Instantiate(blockPrefab, pos, Quaternion.identity));
            }
            
        }


    }


    void Update()
    {
        DrawPlus();
    }

    public void PlaceBlock()
    {

        
    }

    public void DrawPlus()
    {
        if (!isDrawed)
        {
            int index;

        label:
            index = Random.Range(0, blocks.Count);

            if (index % rowCount == 0 && index % rowCount == rowCount - 1 && index % columnCount == 0 && index % columnCount == columnCount - 1)
            {
                blocks[index].GetComponent<SpriteRenderer>().sprite = sprites[0];
            }
            else goto label;

            isDrawed = true;
        }
        
    }

}
