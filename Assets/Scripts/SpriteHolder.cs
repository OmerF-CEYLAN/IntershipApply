using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHolder : MonoBehaviour
{

    [SerializeField]
    internal Sprite[] redSprites, greenSprites, blueSprites,yellowSprites,purpleSprites,pinkSprites;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public Sprite[] GetSpriteArrayOfColor(string color)
    {
        if (redSprites[0].name.Contains(color))
        {
            return redSprites;
        }
        else if(greenSprites[0].name.Contains(color))
        {
            return greenSprites;
        }
        else if (blueSprites[0].name.Contains(color))
        {
            return blueSprites;
        }
        else if (yellowSprites[0].name.Contains(color))
        {
            return yellowSprites;
        }
        else if (purpleSprites[0].name.Contains(color))
        {
            return purpleSprites;
        }
        else if (pinkSprites[0].name.Contains(color))
        {
            return pinkSprites;
        }
        else return null;

    }
}
