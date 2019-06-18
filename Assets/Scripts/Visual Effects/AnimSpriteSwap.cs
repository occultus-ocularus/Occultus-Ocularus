using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AnimSpriteSwap : MonoBehaviour {
    public string spritesheetName;

    private string currentSpritesheetName;
    private Dictionary<string, Sprite> spritesheet;
    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        LoadSpriteSheet();
    }

    private void LateUpdate() {
        if (currentSpritesheetName != spritesheetName)
            LoadSpriteSheet();
            
        spriteRenderer.sprite = spritesheet[spriteRenderer.sprite.name];
    }

    private void LoadSpriteSheet() {
        Sprite[] sprites = Resources.LoadAll<Sprite>(spritesheetName);
        spritesheet = sprites.ToDictionary(x => x.name, x => x);

        currentSpritesheetName = spritesheetName;
    }
}
