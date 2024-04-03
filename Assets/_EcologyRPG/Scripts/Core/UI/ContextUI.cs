using EcologyRPG.Utility;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace EcologyRPG.Core.UI
{
    public class ContextUI : MonoBehaviour
    {
        static ContextUI instance;

        public static ContextUI Instance { get => instance; }

        public Vector2 Offset;
        public GameObject ContextMenu;
        public GameObject ContextMenuButtonPrefab;
        public GameObjectPool ContextMenuButtonPool;

        List<Button> buttons = new();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            ContextMenuButtonPool = new GameObjectPool(ContextMenuButtonPrefab);
            ContextMenuButtonPool.Preload(5);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnGUI()
        {
            if (!ContextMenu.activeSelf) return;
            Event e = Event.current;
            if (e.isMouse && e.type == EventType.MouseUp)
            {
                var results = GetEventSystemRaycastResults();
                int counter = 0;
                foreach (var result in results)
                {
                    if (!IsOverButton(result.gameObject))
                    {
                        counter++;
                    }
                }
                if (counter == results.Count)
                {
                    Close();
                }
            }
        }

        bool IsOverButton(GameObject hit)
        {
            foreach (var button in buttons)
            {
                if (hit == button.gameObject)
                {
                    return true;
                }
            }
            return false;
        }

        public void Open()
        {
            Reset();
            ContextMenu.SetActive(true);
            ContextMenu.transform.position = Mouse.current.position.ReadValue() + Offset;

        }

        private void Reset()
        {
            foreach (var button in buttons)
            {
                ContextMenuButtonPool.ReturnObject(button.gameObject);
                button.onClick.RemoveAllListeners();
                button.transform.SetParent(null);
            }
            buttons.Clear();
        }

        void Close()
        {
            ContextMenu.SetActive(false);
            Reset();
        }

        public void CreateButton(string text, Action action)
        {
            var position = ContextMenu.transform.position;
            var buttonObj = ContextMenuButtonPool.GetObject(position, Quaternion.identity);
            var button = buttonObj.GetComponent<Button>();
            buttonObj.transform.SetParent(ContextMenu.transform);
            buttonObj.transform.localScale = Vector3.one;
            var textComponent = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            textComponent.text = text;
            button.onClick.AddListener((() =>
            {
                action?.Invoke();
                Close();
            }));
            buttons.Add(button);
        }

        static List<RaycastResult> GetEventSystemRaycastResults()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);
            return raycastResults;
        }
    }
}