using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationLight : MonoBehaviour {

	public void Activate () {
		gameObject.SetActive( true );
	}
	public void Deactivate () {
		gameObject.SetActive( false );
	}
}
