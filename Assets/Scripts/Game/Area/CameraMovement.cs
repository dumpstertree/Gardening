using UnityEngine;

public class CameraMovement : MonoBehaviour {

	private Transform _target;
	private Transform _focus;
	private bool _hasBeenSetup;

	public void SetupCamera( Transform target, Transform focus ){
		
		_target = target;
		_focus = focus;

		_hasBeenSetup = true;
	}
	private void Update(){

		if ( _hasBeenSetup ) {
		
			transform.position = Vector3.Lerp( transform.position, _target.transform.position, 0.1f );
			transform.LookAt( _focus );
		}
	}
}
