using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 0649
public class SpriteAssign : MonoBehaviour
{
    [SerializeField] private Sprite emptyImage;
    [SerializeField] private Sprite redGem;
    [SerializeField] private Sprite blueGem;
    [SerializeField] private Sprite purpleGem;
    [SerializeField] private Sprite healGem;
    [SerializeField] private Sprite mpGem;

    private static SpriteAssign instance;


    public static SpriteAssign GetSprite() => instance; 
    void Start()
    {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        }
        else {
            instance = this;
        }
    }

    public Sprite SetImage(RelicData item)
    {
        switch (item.Type)
        {
            case RelicType.Attack:
                return redGem;

            case RelicType.Defense:
                return blueGem;

            case RelicType.Heal:
                return healGem;

            case RelicType.Combo:
                return purpleGem;

            case RelicType.Mp:
                return mpGem;            
        }
        return null;
    }
}
