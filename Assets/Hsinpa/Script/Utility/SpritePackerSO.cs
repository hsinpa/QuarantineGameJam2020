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
        int index = random.Next(0, sprites.Count);

        if (index < 0) return null;

        return sprites[index];
    }
}
