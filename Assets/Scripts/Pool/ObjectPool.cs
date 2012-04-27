using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Repository of commonly used prefabs.
/// </summary>

[AddComponentMenu("Gameplay/ObjectPool")]
public class ObjectPool : MonoBehaviour {

	#region member
	/// <summary>
	/// Member class for a prefab entered into the object pool
	/// </summary>
	[Serializable]
	public class ObjectPoolEntry {

		public Transform Container;

		/// <summary>
		/// the object to pre instantiate
		/// </summary>

		[SerializeField]
		public GameObject Prefab;

		/// <summary>
		/// quantity of object to pre-instantiate
		/// </summary>
		[SerializeField]
		public int Count;

		[HideInInspector]
		public GameObject[] pool;

		[HideInInspector]
		public int objectsInPool = 0;

		/// <summary>
		/// Pools the object specified.  Will not be pooled if there is no prefab of that type.
		/// </summary>
		/// <param name='obj'>
		/// Object to be pooled.
		/// </param>
		public void PoolObject(GameObject obj) {
			obj.SetActiveRecursively(false);
			obj.transform.parent = Container;
			if (obj.rigidbody != null) {
				obj.rigidbody.velocity = Vector3.zero;
			}

			pool[objectsInPool++] = obj;
		}


		public GameObject GetObjectFromPool(bool onlyPooled) {
			if (objectsInPool > 0) {

				GameObject pooledObject = pool[--objectsInPool];
				pooledObject.transform.parent = null;
				pooledObject.SetActiveRecursively(true);

				return pooledObject;
			} else if (!onlyPooled) {
				//TODO should actually pool this in...
				Debug.Log("Exceeded pool capacity. Creating a new instance.");
				GameObject obj = (GameObject)Instantiate(Prefab);
				obj.name = obj.name + "_not_pooled";
				return obj;
			}
			return null;
		}

		public void Initialize(Transform containerTransform) {
			if (Container == null) {
				Container = containerTransform;
			}

			//create the repository
			pool = new GameObject[Count];

			//fill it                       
			for (int n = 0; n < Count; n++) {
				GameObject newObj = (GameObject)Instantiate(Prefab);
				newObj.AddComponent<Poolable>().Initialize(this);
				newObj.name = Prefab.name;
				PoolObject(newObj);
			}
		}
	}
	#endregion

	/// <summary>
	/// The object prefabs which the pool can handle
	/// by The amount of objects of each type to buffer.
	/// Consider using a dictionary if there are many. 
	/// </summary>
	public ObjectPoolEntry[] Entries;
	private Dictionary<GameObject, ObjectPoolEntry> entryDictionary;

	/// <summary>
	/// The container object that we will keep unused pooled objects so we dont clog up the editor with objects.
	/// </summary>
	public Transform DefaultContainer;
	

	public ObjectPoolEntry GetPoolEntryForPrefab(GameObject prefab) {
		if (entryDictionary == null) {
			entryDictionary = new Dictionary<GameObject, ObjectPoolEntry>();

			foreach (ObjectPoolEntry objectPrefab in Entries) {
				entryDictionary.Add(objectPrefab.Prefab, objectPrefab);
			}
		}
		return entryDictionary[prefab];
	}

	void Awake() {
		if (DefaultContainer == null) {
			DefaultContainer = transform;
		}

		//Loop through the object prefabs and make a new list for each one.
		//We do this because the pool can only support prefabs set to it in the editor,
		//so we can assume the lists of pooled objects are in the same order as object prefabs in the array

		foreach (ObjectPoolEntry objectPrefab in Entries) {
			objectPrefab.Initialize(DefaultContainer);
		}
	}


	/// <summary>
	/// Gets a new object for the name type provided.  If no object type exists or if onlypooled is true and there is no objects of that type in the pool
	/// then null will be returned.
	/// </summary>
	/// <returns>
	/// The object for type.
	/// </returns>
	/// <param name="prefabGameObject"> 
	/// Object type.
	/// </param>
	/// <param name='onlyPooled'>
	/// If true, it will only return an object if there is one currently pooled.
	/// </param>
	public GameObject GetObjectForPrefab(GameObject prefabGameObject, bool onlyPooled) {

		if (entryDictionary.ContainsKey(prefabGameObject)) {
			ObjectPoolEntry objectPoolEntry = entryDictionary[prefabGameObject];

			return objectPoolEntry.GetObjectFromPool(onlyPooled);
		}

		//If we have gotten here either there was no object of the specified type or non were left in the pool with onlyPooled set to true
		return null;
	}

}

