using UnityEngine;

public class OrbPosition : MonoBehaviour {

	public State OrbState {
		set{ HandleOnChangeState( value ); }
	}

	public delegate void OnStateChangeEvent( State newState );
	public OnStateChangeEvent OnStateChange;

	// *******************************

	[SerializeField] private OrbVisual _orbVisualPrefab;
	[SerializeField] private float _speed;
	[SerializeField] private Vector3 _axis;

	private const float PASSIVE_SPEED_MULT = 1.0f;
	private const float EXCITED_SPEED_MULT = 2.0f;

	private State _state;
	private float _speedMult;

	// *******************************
	
	private void Awake () {

		_speedMult = PASSIVE_SPEED_MULT;

		var vis = Instantiate( _orbVisualPrefab );
		vis.transform.position = transform.position;
		vis.TrackingObject = this;
	}
	private void Update () {
		
		// move
		transform.RotateAround( transform.parent.position, _axis, _speed * _speedMult );
	}
	private void HandleOnChangeState ( State newState ) {

		if( _state != newState ) {
			
			// update state
			_state = newState;

			// update speed mult
			switch ( _state ) { 
				case OrbPosition.State.Passive: _speedMult = PASSIVE_SPEED_MULT; break;
				case OrbPosition.State.Excited: _speedMult = EXCITED_SPEED_MULT; break;
			}

			// call event
			if ( OnStateChange != null ) {
				OnStateChange( _state );
			}
		}	
	}

	public enum State {
		Passive,
		Excited
	}
}