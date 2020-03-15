using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpritePackerSO : ScriptableObject
{
    [SerializeField]
    private List<Sprite> sprites = new List<Sprite>();



    private System.Random random = new System.Random();

    public Sprite FindSpriteByName(string name) {
        return sprites.Find(x => x.name == name);
    }

    public Sprite FindSpriteByRandom()
    {
        var filterSprites = sprites.FindAll(x => x.name.IndexOf("people") >= 0);
        int index = random.Next(0, filterSprites.Count);
        if (index < 0) return null;

        return filterSprites[index];
    }
}
