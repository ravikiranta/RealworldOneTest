using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using GameInterfaces;

namespace Controllers {
    public class ItemSpawnController : MonoBehaviour,IInteractable
    {
        #region Variables
        [Header("Dev Settings")]
        [SerializeField] private string spawnItem;
        [SerializeField] [Range(0.1f,1f)] private float spawnHeight;
        [SerializeField] private bool itemOnSpawner;
        [SerializeField] private bool spawnRequested;
        [SerializeField] private string interactionControlsMessage;

        [SerializeField] private GameObject itemReference;
        [SerializeField] [Range(0.1f, 5f)] private float itemRespawnTime;
        #endregion

        #region Init
        void Start()
        {
            CreateItem();
        }
        #endregion

        #region SpawnFunctions
        void CreateItem()
        {
            GameObject itemPrefab = GameManager.Instance.ReturnVegetableModel(spawnItem);
            if (itemPrefab != null)
            {
                GameObject item = Instantiate(itemPrefab, transform.position + Vector3.up * spawnHeight, transform.rotation);
                item.name = spawnItem;
                item.transform.SetParent(transform);
                itemOnSpawner = true;
                spawnRequested = false;
                itemReference = item;
            }
            else
            {
                //Debug.Log("Item Not Found:" + spawnItem);
            }
        }
        #endregion

        #region CheckForItemsOnSpawner
        void OnTriggerStay(Collider collider)
        {
            //Debug.Log("Checking for item");
            if(itemReference == null)
            {
                itemOnSpawner = false;
            }
            if (!itemOnSpawner && !spawnRequested)
            {
                itemOnSpawner = true;
                spawnRequested = true;
                Invoke("CreateItem", itemRespawnTime);
            }
        }

        string IInteractable.GetInteractionControls()
        {
            return interactionControlsMessage;
    }
        #endregion
    }
}