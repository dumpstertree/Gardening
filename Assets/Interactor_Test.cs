using System.Collections.Generic;
using UnityEngine;

/*
public class Interactor_Test : MonoBehaviour {

	public void ChangeState ( OrbVisual.State newState ) {
		
		foreach ( OrbParams param in _orbParams ) {
			param.State = newState;
		}
	}

	[Header( "Prefabs" )]		
	[SerializeField] private OrbVisual _orbVisual;

	[Header( "Params" )]
	[SerializeField] private List<OrbParams> _orbParams;

	private void Awake () {
		
		foreach ( OrbParams param in _orbParams ) {
			var vis = Instantiate( _orbVisual );
			vis.TrackingObject = param;
			vis.transform.position = param.OrbTransform.position;
		}
	}
	private void Update () {

		foreach ( OrbParams orb in _orbParams ) {
			orb.Update( transform.position );
		}
	}
}


[System.Serializable]
public class OrbParams {

	public delegate void OnStateChangeEvent( OrbVisual.State newState );
	public OnStateChangeEvent OnStateChange;

	public OrbVisual.State State {
		set{ HandleOnChangeState( value ); }
	}

	public void Update ( Vector3 centerPoint ) {
		
		OrbTransform.RotateAround( centerPoint, Axis, Speed * _speedMult );
	}

	// *******************************

	private const float PASSIVE_SPEED_MULT = 1.0f;
	private const float EXCITED_SPEED_MULT = 2.0f;

	public float Speed;
	public Vector3 Axis;
	public Transform OrbTransform;
	private OrbVisual.State _state;
	private float _speedMult = PASSIVE_SPEED_MULT;

	private void HandleOnChangeState( OrbVisual.State newState ){
		Debug.Log( newState );
		if( _state != newState ) {
			
			_state = newState;

			switch ( _state ) { 
				case OrbVisual.State.Passive: _speedMult = PASSIVE_SPEED_MULT; break;
				case OrbVisual.State.Excited: _speedMult = EXCITED_SPEED_MULT; break;
			}

			if ( OnStateChange != null ) {
				OnStateChange( _state );
			}
		}	
	}
}
*/
