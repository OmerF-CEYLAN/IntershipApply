using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    [SerializeField]
    private int rowNo,columnNo;

    static Board board;

    [SerializeField]
    string color;

    [SerializeField]
    List<Block> group;

    [SerializeField]
    float _fallSpeed;

    static float fallSpeed = 4;

    static bool isAllowedToTouch;

    void Awake()
    {
        group = new List<Block>
        {
            this
        };

        color = tag;

    }

    void Start()
    {
        board = FindAnyObjectByType<Board>();

    }

    void FixedUpdate()
    {
        FallDown();
    }

    public void OnMouseUpAsButton()
    {

        foreach (Block item in group)
        {
            if (item == this) continue;
            item.NotifyBlockIsDestroyed();
            Destroy(item.gameObject);
            Debug.Log("Destroyed");
        }

        NotifyBlockIsDestroyed();

        NotifyToFindGroups();
        Debug.Log("Destroyed");
        Destroy(gameObject);

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
    }

}
