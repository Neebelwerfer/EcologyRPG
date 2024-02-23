using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Character;

public class CharacterUI : MonoBehaviour
{
    public GameObject AttributeView;
    public GameObject StatView;
    public GameObject AttributeTextPrefab;
    public GameObject StatTextPrefab;


    PlayerCharacter player;
    TextMeshProUGUI[] AttributeTexts;
    TextMeshProUGUI[] StatsTexts;

    private void OnEnable()
    {
        if(player == null)
        {
            player = PlayerManager.Instance.GetPlayerCharacter();
            AttributeTexts = new TextMeshProUGUI[player.stats._attributes.Count];
            StatsTexts = new TextMeshProUGUI[player.stats._stats.Count];
        }
        for (int i = 0; i < player.stats._attributes.Count; i++)
        {
            CreateAttributeText(player.stats._attributes[i], i);
        }

        for (int i = 0; i < player.stats._stats.Count; i++)
        {
            CreateStatText(player.stats._stats[i], i);
        }
        InvokeRepeating(nameof(UpdateUI), 1, 1f);
    }

    void UpdateUI()
    {
        for (int i = 0; i < player.stats._attributes.Count; i++)
        {
            AttributeTexts[i].text = player.stats._attributes[i].data.displayName + ": " + player.stats._attributes[i].Value;
        }

        for (int i = 0; i < player.stats._stats.Count; i++)
        {
            StatsTexts[i].text = player.stats._stats[i].Data.displayName + ": " + player.stats._stats[i].Value;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < AttributeTexts.Length; i++)
        {
            Destroy(AttributeTexts[i].gameObject);
        }

        for (int i = 0; i < StatsTexts.Length; i++)
        {
            Destroy(StatsTexts[i].gameObject);
        }
        CancelInvoke(nameof(UpdateUI));
    }

    void CreateAttributeText(Attribute attribute, int order)
    {
        var text = Instantiate(AttributeTextPrefab, AttributeView.transform);
        text.transform.position = AttributeView.transform.position;
        var comp = text.GetComponent<TextMeshProUGUI>();
        comp.text = attribute.data.displayName + ": " + attribute.Value;
        AttributeTexts[order] = comp;
    }

    void CreateStatText(Stat stat, int order)
    {
        var text = Instantiate(StatTextPrefab, StatView.transform);
        text.transform.position = StatView.transform.position;
        var comp = text.GetComponent<TextMeshProUGUI>();
        comp.text = stat.Data.displayName + ": " + stat.Value;
        StatsTexts[order] = comp;
    }
}
