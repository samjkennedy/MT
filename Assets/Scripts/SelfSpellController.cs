using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfSpellController : MonoBehaviour
{
    public Player player;
    public Element element;
    public SelfStyle style;
    public SelfSpell spellPrefab;
    private SelfSpell spell;

    private bool isCasting;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCasting && Input.GetButtonDown("Dash")) {
            StartCoroutine(CastSelfSpell());
        }
    }

    IEnumerator CastSelfSpell() {
        isCasting = true;
        spell = Instantiate(spellPrefab, player.transform);
        spell.Cast(player, element, style);

        yield return new WaitForSeconds(style.GetEffectTime());
        isCasting = false;
    }
}
