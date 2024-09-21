using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public GameObject healthBar;

    Vector2 healthBarStartPosition;
    RectTransform healthBarRectTransform;
    Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
        healthBarRectTransform = healthBar.GetComponent<RectTransform>();
        healthBarStartPosition = healthBarRectTransform.position;
    }

    void Update()
    {
       RenderBars(); 
    }

    void RenderBars()
    {
        healthBarRectTransform.localScale = new Vector3(player.health/player.maxHealth,1,1);
        healthBarRectTransform.position = healthBarStartPosition - new Vector2((healthBarRectTransform.rect.width-(healthBarRectTransform.rect.width*(player.health/player.maxHealth))),0);
    }
}
