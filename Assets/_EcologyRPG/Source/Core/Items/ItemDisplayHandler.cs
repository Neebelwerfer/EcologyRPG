using EcologyRPG.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.Core.Items
{
    public class ItemDisplayHandler : MonoBehaviour
    {
        static ItemDisplayHandler instance;
        public static ItemDisplayHandler Instance { get { return instance; } }
        public GameObject itemDisplayPrefab;
        public GameObject displayCanvas;
        GameObjectPool itemDisplayPool;

        readonly List<ItemPickup> itemPickups = new();

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
            itemDisplayPool = new GameObjectPool(itemDisplayPrefab);
            itemDisplayPool.Preload(10);
        }

        public void SpawnItem(Item item, int amount, Vector3 pos)
        {
            var itemDisplay = itemDisplayPool.GetObject(pos, Quaternion.identity);
            itemDisplay.transform.SetParent(displayCanvas.transform);
            var itemPickup = itemDisplay.GetComponent<ItemPickup>();
            itemPickup.Setup(item, amount);
            itemPickups.Add(itemPickup);
        }

        public void RemoveItemPickup(ItemPickup itemPickup)
        {
            itemPickups.Remove(itemPickup);
            itemPickup.transform.SetParent(null);
            itemDisplayPool.ReturnObject(itemPickup.gameObject);
        }

    }
}