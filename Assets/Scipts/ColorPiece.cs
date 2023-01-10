using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPiece : MonoBehaviour
{
    public ColorSprite[] colorSprites;
    private Dictionary<ColorType, Sprite> colorSpriteDict;
    private SpriteRenderer sprite;
    private ColorType color;

    void Awake()
    {
        sprite = transform.Find("piece").GetComponent<SpriteRenderer>();

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


    public ColorType Color
    {
        get { return color; }
        set { SetColor(value); }
    }

    public void SetColor(ColorType newColor)
    {
        color = newColor;
        if (colorSpriteDict.ContainsKey(newColor)) {
            sprite.sprite = colorSpriteDict[newColor];
        }
    }

    public int NumColors
    {
        get { return colorSprites.Length; }
    }
}
