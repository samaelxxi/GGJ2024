using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionMarker : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] Color EnemyColor;
    [SerializeField] Color AllyColor;

    public enum Type
    {
        Enemy,
        Ally,
        Selection
    }

    public void SetType(Type type)
    {
        switch (type)
        {
            case Type.Selection:
                spriteRenderer.color = Color.white;
            break;
            case Type.Enemy:
                spriteRenderer.color = EnemyColor;
            break;
            case Type.Ally:
                spriteRenderer.color = AllyColor;
            break;
        }
    }

    public void SetVisible(bool isVisible)
    {
        spriteRenderer.gameObject.SetActive(isVisible);
    }
}
