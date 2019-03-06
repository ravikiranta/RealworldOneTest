using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

namespace Controllers {
    public class ItemSpawnController : MonoBehaviour
    {
        #region Variables
        [Header("Dev Settings")]
        [SerializeField] private string spawnItem;
        [SerializeField] [Range(0.1f,1f)] private float spawnHeight;
        [SerializeField] private bool itemOnSpawner;
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
            }
            else
            {
                Debug.Log("Item Not Found:" + spawnItem);
            }
        }
        #endregion
    }
}