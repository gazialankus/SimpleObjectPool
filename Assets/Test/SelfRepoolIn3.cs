using UnityEngine;
using System.Collections;

public class SelfRepoolIn3 : MonoBehaviour {

	private bool isCountingDown;
	private float startTime;

	public void RepoolIn3() {
		startTime = Time.time;
		isCountingDown = true;
	}

	// Update is called once per frame
	void Update () {
		if (isCountingDown && Time.time - startTime > 3) {
			isCountingDown = false;
			GoToSleep();
		}
	}

	private Poolable poolable;

	private void GoToSleep() {
		if (poolable == null) {
			poolable = GetComponent<Poolable>();
		}

		if (poolable != null) {
			poolable.PoolBack();
		} else {
			Destroy(gameObject);
		}
	}
}
