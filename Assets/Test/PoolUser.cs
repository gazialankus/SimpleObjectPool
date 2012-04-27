using UnityEngine;
using System.Collections;

public class PoolUser : MonoBehaviour {
	private ObjectPool objectPool;
	private ObjectPool.ObjectPoolEntry objectPoolEntry;

	public GameObject PrefabToCreate;

	// Use this for initialization
	void Awake() {
		objectPool = GameObject.Find("Object Pool").GetComponent<ObjectPool>();
		objectPoolEntry = objectPool.GetPoolEntryForPrefab(PrefabToCreate);
	}

	public void OnGUI() {
		if (GUILayout.Button("Get object from pool")) {
			GameObject newobj = objectPoolEntry.GetObjectFromPool(false);
			newobj.GetComponent<SelfRepoolIn3>().RepoolIn3();
		}
	}
}
