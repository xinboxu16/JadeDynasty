using UnityEngine;
using System.Collections;

public class FS_AddSome : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(MakeSomeNewOnes());
	}
	
	IEnumerator MakeSomeNewOnes ()
	{
		while (true) {
			GameObject go = (GameObject) Instantiate(gameObject);
			Destroy(go.GetComponent<FS_AddSome>());
			yield return new WaitForSeconds(1f);
		}
	}
}
