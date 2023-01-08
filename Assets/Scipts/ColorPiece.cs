using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPiece : MonoBehaviour
{
    public ColorSprite[] colorSprites;
    private Dictionary<ColorType, Sprite> colorSpriteDict;
    private SpriteRenderer sprite;
    private ColorType color;

    public ColorType Color
    {
        get { return color; }
        set { color = value; }
        //set { SetColor(value); }
    }

    void Awake()
    {
        colorSpriteDict = new Dictionary<ColorType, Sprite>();
        for (int i = 0; i < colorSprites.Length; i++) {
            if (!colorSpriteDict.ContainsKey(colorSprites[i].color)) {
                colorSpriteDict.Add(colorSprites[i].color, colorSprites[i].sprite);
            }
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
