using EcologyRPG._Core.Character;
using EcologyRPG._Game.Player;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;
using UnityEngine;

namespace EcologyRPG._Game.UI
{
    class StatBinding
    {
        public TextMeshProUGUI Text;
        public Stat Stat;

        UnityAction<float> OnStatChanged;

        public StatBinding(TextMeshProUGUI text, Stat stat)
        {
            Text = text;
            Stat = stat;

            DisplayText();
            OnStatChanged = (value) =>
            {
                DisplayText();
                if (stat.Data.ShowOptions == ShowOptions.WhenNonZero)
                {
                    Text.gameObject.SetActive(value != 0);
                }
            };
            Stat.OnStatChanged.AddListener(OnStatChanged);
        }

        void DisplayText()
        {
            if (Stat.Data.DisplayOptions == DisplayOptions.Flat)
                Text.text = Stat.Data.displayName + ": " + Stat.Value.ToString("#.#");
            else if (Stat.Data.DisplayOptions == DisplayOptions.Percent)
                Text.text = Stat.Data.displayName + ": " + (Stat.Value * 100).ToString("#.#") + "%";

        }

        public void Destroy()
        {
            Stat.OnStatChanged.RemoveListener(OnStatChanged);
        }
    }

    class AttributeBinding
    {
        public TextMeshProUGUI Text;
        public Attribute Attribute;

        UnityAction<int> OnAttributeChanged;

        public AttributeBinding(TextMeshProUGUI text, Attribute attribute)
        {
            Text = text;
            Attribute = attribute;
            text.text = attribute.Data.displayName + ": " + attribute.Value.ToString("#.#");
            OnAttributeChanged = (value) => Text.text = attribute.Data.displayName + ": " + value;
            Attribute.OnAttributeChanged.AddListener(OnAttributeChanged);
        }

        public void UpdateText()
        {
            Text.text = Attribute.Data.displayName + ": " + Attribute.Value;
        }

        public void Destroy()
        {
            Attribute.OnAttributeChanged.RemoveListener(OnAttributeChanged);
        }
    }

    public class CharacterUI : MonoBehaviour
    {
        public GameObject AttributeView;
        public GameObject StatView;
        public GameObject AttributeTextPrefab;
        public GameObject StatTextPrefab;


        PlayerCharacter player;

        List<StatBinding> StatBindings;
        List<AttributeBinding> AttributeBindings;

        private void Awake()
        {
            player = PlayerManager.PlayerCharacter;
            StatBindings = new List<StatBinding>();
            AttributeBindings = new List<AttributeBinding>();

            for (int i = 0; i < player.Stats._stats.Count; i++)
            {
                if (IsStatHidden(player.Stats._stats[i]))
                    continue;
                CreateStatText(player.Stats._stats[i]);
            }

            for (int i = 0; i < player.Stats._attributes.Count; i++)
            {
                CreateAttributeText(player.Stats._attributes[i]);
            }
        }

        bool IsStatHidden(Stat stat)
        {
            if (stat.Data.ShowOptions == ShowOptions.Always)
            {
                return false;
            }
            else if (stat.Data.ShowOptions == ShowOptions.WhenNonZero)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void OnEnable()
        {
            if (AttributeBindings != null)
            {
                foreach (var binding in AttributeBindings)
                {
                    binding.UpdateText();
                }
            }
        }

        void CreateAttributeText(Attribute attribute)
        {
            var text = Instantiate(AttributeTextPrefab, AttributeView.transform);
            text.transform.position = AttributeView.transform.position;
            var comp = text.GetComponent<TextMeshProUGUI>();

            AttributeBindings.Add(new AttributeBinding(comp, attribute));
        }

        void CreateStatText(Stat stat)
        {
            var text = Instantiate(StatTextPrefab, StatView.transform);
            text.transform.position = StatView.transform.position;
            var comp = text.GetComponent<TextMeshProUGUI>();
            if (stat.Data.ShowOptions == ShowOptions.WhenNonZero)
            {
                text.SetActive(stat.Value != 0);
            }
            else
            {
                text.SetActive(true);
            }
            StatBindings.Add(new StatBinding(comp, stat));
        }

        private void OnDestroy()
        {
            foreach (var binding in StatBindings)
            {
                binding.Destroy();
            }

            foreach (var binding in AttributeBindings)
            {
                binding.Destroy();
            }
        }
    }
}

