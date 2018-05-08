using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(BoxCollider) )]
public class StaticCameraPoint : MonoBehaviour {


	public delegate void EnterRangeEvent ();
	public EnterRangeEvent OnEnterRange;

	public delegate void ExitRangeEvent ();
	public ExitRangeEvent OnExitRange;

	public Vector3 TargetCameraPosition {
		get { return _cameraPosition.position; }
	}
	public Quaternion TargetCameraRotation {
		get{ return _cameraPosition.rotation; }
	}

	[SerializeField] private Transform _cameraPosition;
	[SerializeField] private Vector3 _triggerRange;
	[SerializeField] private bool _canRotate;

	private BoxCollider _collider;

	private void Awake () {
		
		_collider = GetComponent<BoxCollider>();
	}
	private void OnTriggerEnter( Collider other ) {

		if ( other.transform == Game.Area.LoadedPlayer.transform ) {
			FireOnRangeEnterEvent ();
		}
    }
   private void OnTriggerExit( Collider other ) {
		
		if ( other.transform == Game.Area.LoadedPlayer.transform ) {
			FireOnRangeExitEvent ();
		}
	}
	private void FireOnRangeEnterEvent () {

		if ( OnEnterRange != null ) {
			OnEnterRange ();
		}
	}
	private void FireOnRangeExitEvent () {

		if ( OnExitRange != null ) {
			OnExitRange ();
		}
	}
	private void Update () {

		if ( _canRotate ) {
			_cameraPosition.LookAt( Game.Area.LoadedPlayer.CameraFocus );
		}
	}
	private void OnDrawGizmos () {

		// find collider
		if ( _collider == null ) {
			_collider = GetComponent<BoxCollider>();
		}

		// update collider
		_collider.isTrigger = true;
		_collider.size = _triggerRange;

		// draw camera position
		if ( _cameraPosition != null ) {
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere( _cameraPosition.position,  0.25f );
			Gizmos.DrawRay( _cameraPosition.position, _cameraPosition.forward );
		} else {
			Gizmos.color = Color.red;
		}

		// update gizmos
		Gizmos.DrawWireCube( transform.position, _triggerRange );
	}
}
