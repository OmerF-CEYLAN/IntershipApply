using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class Block : MonoBehaviour
{

    [SerializeField]
    internal int rowNo,columnNo;

    static Board board;

    [SerializeField]
    string color;

    [SerializeField]
    List<Block> group;

    [SerializeField]
    float _fallSpeed;

    static float fallSpeed = 6;

    static bool isAllowedToTouch;

    Sprite blockSprite;

    static int maxCountForDefault, maxCountForSpriteA, maxCountForSpriteB;

    static int minCountToCollapse;

    //static is allowed for must be added while collapsing

    void Awake()
    {
        group = new List<Block>
        {
            this
        };

        color = tag;

        blockSprite = GetComponent<SpriteRenderer>().sprite;

        minCountToCollapse = 2;

        maxCountForDefault = 4;
        maxCountForSpriteA = 7;
        maxCountForSpriteB = 9;
    }

    void Start()
    {
        board = FindAnyObjectByType<Board>();

    }

    void FixedUpdate()
    {
        GetComponent<SpriteRenderer>().sortingOrder = rowNo;

        FallDown();
    }

    public void OnMouseUpAsButton()
    {
        if (group.Count < minCountToCollapse)
            isAllowedToTouch = false;
        else
            isAllowedToTouch = true;

        if (isAllowedToTouch)
        {
            foreach (Block item in group)
            {
                if (item == this) continue;
                item.NotifyBlockIsDestroyed();
                Destroy(item.gameObject);
            }

            NotifyBlockIsDestroyed();

            NotifyToFindGroups();
            Destroy(gameObject);
        }


    }

    public void SetRawAndColNo(int rowNo,int columnNo)
    {
        this.rowNo = rowNo;
        this.columnNo = columnNo;
    }

    public void NotifyBlockIsDestroyed()
    {
        board.ObserveBlockChanges(rowNo, columnNo);
    }

    private void NotifyToFindGroups()
    {
        board.ObserveBlockChanges();
    }

    public string GetColor()
    {
        return color;
    }

    public List<Block> GetGroup()
    {
        return group;
    }

    public void SetGroup(List<Block> group)
    {
        this.group = group;
    }

    public void FallDown()
    {
        if(transform.position.y != board.GetPositions()[rowNo,columnNo].y)
        {
            transform.position = Vector2.MoveTowards(transform.position, board.GetPositions()[rowNo,columnNo],fallSpeed * Time.deltaTime);
        }
        if (transform.position.x != board.GetPositions()[rowNo, columnNo].x)
        {
            transform.position = Vector2.MoveTowards(transform.position, board.GetPositions()[rowNo, columnNo], fallSpeed * Time.deltaTime);
        }
    }

    public void SetSprite(Sprite[] spriteArray)
    {

        if (group.Count > maxCountForSpriteB)
        {
            blockSprite = spriteArray[3];
        }
        else if (group.Count > maxCountForSpriteA)
        {
            blockSprite = spriteArray[2];
        }
        else if (group.Count > maxCountForDefault)
        {
            blockSprite = spriteArray[1];
        }
        else
        {
            blockSprite = spriteArray[0];
        }

        GetComponent<SpriteRenderer>().sprite = blockSprite;



    }

}
