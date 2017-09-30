using UnityEngine;
using System.Collections;

public class FS_RandomWander : MonoBehaviour
{
	public float speed = .1f;
	public float directionChangeInterval = 1;
	public float maxHeadingChange = 30;
	public float dist = 0f;

	float heading;
	Vector3 targetRotation;
 
	void Awake ()
	{

		// Set random initial rotation
		heading = Random.Range(0, 360);
		transform.eulerAngles = new Vector3(0, heading, 0);
 
		StartCoroutine(NewHeading());
	}
 
	void FixedUpdate ()
	{
		transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
		var forward = transform.TransformDirection(Vector3.forward);
		transform.Translate(forward * speed);
		if (transform.position.y < 1f)
			NewHeadingRoutine();
		dist = transform.position.magnitude;
		if (transform.position.magnitude > 70f){
			//transform.position = new Vector3(0f,10f,0f);
			Destroy(gameObject);
		}
		Vector3 v = transform.position;
		v.y = 10f;
		transform.position = v;
	}
 

	IEnumerator NewHeading ()
	{
		while (true) {
			NewHeadingRoutine();
			yield return new WaitForSeconds(directionChangeInterval);
		}
	}	

	void NewHeadingRoutine ()
	{
		var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 360);
		var ceil  = Mathf.Clamp(heading + maxHeadingChange, 0, 360);
		heading = Random.Range(floor, ceil);
		targetRotation = new Vector3(0, heading, 0);
	}
}	
