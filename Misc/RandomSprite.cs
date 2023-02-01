using System.Collections.Generic;
using UnityEngine;

public class RandomSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spr;
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();

    private void Start()
    {
        spr.sprite = sprites.Random();
    }
}