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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseUpAsButton()
    {

        foreach (Block item in group)
        {
            if (item == this) continue;
            Destroy(item);
        }

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

    private void OnDestroy()
    {
        NotifyBlockIsDestroyed();
    }

}
