using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        Instance_OnTurnStart();
    }

    private void Instance_OnTurnStart()
    {
        List<BaseUnit> units = BattleState.instance.units.FindAll((BaseUnit compare) => compare.IsAlive());

        units.Sort((BaseUnit lh, BaseUnit rh) => { return lh.time.CompareTo(rh.time); });

        HashSet<BaseUnit> usedUnits = new HashSet<BaseUnit>();

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
