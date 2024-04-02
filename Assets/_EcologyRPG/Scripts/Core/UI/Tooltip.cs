using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace EcologyRPG._Core.UI
{
    public interface ITooltip
    {
        TooltipData GetTooltipData();
    }
    public class Tooltip : MonoBehaviour
    {
        public Vector2 Offset;
        public GameObject TooltipObject;
        public TextMeshProUGUI Title;
        public Image Icon;
        public TextMeshProUGUI Description;

        ITooltip tooltipData;

        static Tooltip instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
        }

        public void UpdateTooltip()
        {
            var data = tooltipData.GetTooltipData();
            Title.text = data.Title;
            Icon.sprite = data.Icon;
            Description.text = data.Description;
        }
        public void Show(ITooltip tooltipData)
        {
            if (this.tooltipData == tooltipData) return;
            this.tooltipData = tooltipData;
            var data = tooltipData.GetTooltipData();
            TooltipObject.SetActive(true);
            TooltipObject.transform.position = Mouse.current.position.ReadValue() + Offset;
            InvokeRepeating(nameof(UpdateTooltip), 0f, 0.3f);
        }

        public void Hide(object source)
        {
            if (instance != null)
            {
                if (tooltipData == source)
                {
                    tooltipData = null;
                    TooltipObject.SetActive(false);
                    CancelInvoke(nameof(UpdateTooltip));
                }
            }
        }

        public static void HideTooltip(object source)
        {
            if (instance != null)
                instance.Hide(source);
        }

        public static void ShowTooltip(ITooltip tooltipData)
        {
            instance.Show(tooltipData);
        }

    }

    public class TooltipData
    {
        public string Title;
        public Sprite Icon;
        public string Description;
    }
}