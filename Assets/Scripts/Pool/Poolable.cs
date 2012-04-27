using UnityEngine;
using System.Collections;

public class Poolable : MonoBehaviour {
//	private ObjectPool objectPool;
	private ObjectPool.ObjectPoolEntry objectPoolEntry;

	private GameObject lGameObject;

	void Awake() {
		lGameObject = gameObject;
	}

	public void Initialize(ObjectPool.ObjectPoolEntry objectPoolEntry) {
		this.objectPoolEntry = objectPoolEntry;
	}

	public void PoolBack() {
		objectPoolEntry.PoolObject(lGameObject);
	}
}
