using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemColor : MonoBehaviour
{
    // Color Type
    public enum ColorType
    {
        STAR,
        HEART,
        QUAD,
        PENTA,
        OCTA,
        BOMB,
        TOTAL_COLOR_TYPES
    }

    [System.Serializable]
    public struct ColorSprite
    {
        public ColorType color;
        public GameObject spriteGemPrefab;
    }

    public ColorSprite[] ColorSprites;

    private ColorType color;

    public ColorType Color
    {
        get
        {
            return color;
        }
        set
        {
            SetColor(value);
        }
    }

    private SpriteRenderer sprite;

    private Dictionary<ColorType, GameObject> colorSpriteDict;

    private void Awake()
    {
        // Get Gem Sprite Renderer
        sprite = GetComponent<SpriteRenderer>();

        // Fill Dictionary of color sprites
        colorSpriteDict = new Dictionary<ColorType, GameObject>();
        foreach (var colorSprite in ColorSprites)
        {
            if (!colorSpriteDict.ContainsKey(colorSprite.color))
            {
                colorSpriteDict.Add(colorSprite.color, colorSprite.spriteGemPrefab);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Set Sprite, Material and Animator using Set Color
    /// </summary>
    /// <param name="newColor"></param>
    public void SetColor(ColorType newColor)
    {
        color = newColor;

        if (colorSpriteDict.ContainsKey(newColor))
        {
            sprite.sprite = colorSpriteDict[newColor].GetComponent<SpriteRenderer>().sprite;
            sprite.material = colorSpriteDict[newColor].GetComponent<SpriteRenderer>().sharedMaterial;
            sprite.gameObject.GetComponent<Animator>().runtimeAnimatorController =
                colorSpriteDict[newColor].GetComponent<Animator>().runtimeAnimatorController;
        }
    }

    // Get total colors
    public int GetTotalColors()
    {
        return ColorSprites.Length;
    }
}
