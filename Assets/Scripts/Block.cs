using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    SpriteRenderer spriteRenderer;

    [SerializeField]
    private int rowNo,columnNo;

    static Board board;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        board = FindAnyObjectByType<Board>();

        SetSprite(board.GetRandomSprite());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void OnMouseUpAsButton()
    {
        Destroy(gameObject);
        NotifyBlockIsDestroyed();
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

}
