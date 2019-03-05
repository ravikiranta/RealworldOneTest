using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    #region Variables
    [Header("Dev Settings")]
    [Range(0.1f,10f)][SerializeField] private float customerSpawnGap = 5.0f;

    [Header("References")]
	[SerializeField] private List<GameObject> customerSpawnPoints;
	#endregion

	#region Init
	void Start () {
        StartCoroutine(AutoSpawnCustomers());
	}

    IEnumerator AutoSpawnCustomers()
    {
        while (true)
        {
            CreateCustomers();
            yield return new WaitForSeconds(customerSpawnGap);
        }
    }

	void CreateCustomers() {
		for(int i = 0; i < customerSpawnPoints.Count; i++){
			if(!customerSpawnPoints[i].activeInHierarchy){
				customerSpawnPoints[i].SetActive(true);
                break;
			}
		}
	}
	#endregion
}
