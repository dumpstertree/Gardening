using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraSystem : MonoBehaviour {

	public void Init () {
		OnInit ();
	}
	
	protected virtual void OnInit () {}
}
