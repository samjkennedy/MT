using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfSpell : MonoBehaviour
{

    public Element elementPrefab;
    private Element element;
    public SelfStyle stylePrefab;
    private SelfStyle style;
    //TODO: self mutations

    private Player player;

    public void Cast(Player player, Element elementPrefab, SelfStyle stylePrefab) {
        this.player = player;
        
        style = Instantiate(stylePrefab, transform);
        element = Instantiate(elementPrefab, player.transform);
        style.Cast(player, element);
        
        Destroy(element.gameObject, style.GetEffectTime());
        Destroy(gameObject, style.GetEffectTime());
    }

}
