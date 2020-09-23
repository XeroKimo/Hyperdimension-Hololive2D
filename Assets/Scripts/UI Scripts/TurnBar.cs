using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Version 0.2

public class TurnBar : MonoBehaviour
{
    private RawImage[] m_images;

    private void Awake()
    {
        m_images = GetComponentsInChildren<RawImage>();
    }

    public void Start()
    {
        BattleState.instance.OnTurnStart += Instance_OnTurnStart;
        //Instance_OnTurnStart();
    }

    private void Instance_OnTurnStart()
    {
        List<BattleUnit> units = BattleState.instance.units.FindAll((BattleUnit compare) => compare.IsAlive());

        units.Sort((BattleUnit lh, BattleUnit rh) => { return lh.time.CompareTo(rh.time); });

        HashSet<BattleUnit> usedUnits = new HashSet<BattleUnit>();

        for(int i = 0; i < units.Count; i++)
        {
            m_images[i].texture = units[i].sprite.sprite.texture;
        }

        for(int i = units.Count; i < m_images.Length; i++)
        {
            m_images[i].texture = units[i % units.Count].sprite.sprite.texture;
        }
    }
}
