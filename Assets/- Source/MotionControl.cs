using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionControl : MonoBehaviour {

	const string X = "Horizontal";
	const string Z = "Vertical";
	public float gain = 1f;
	Vector3 dir;

	void Update () {
			dir = new Vector3(Input.GetAxis(X), 0, Input.GetAxis(Z));
			transform.position += dir * Time.deltaTime * gain;
			if(dir.magnitude>0.2f) transform.forward = dir;
	}

}
