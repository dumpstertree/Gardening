using UnityEngine;

public class OrbVisual : MonoBehaviour {

	public OrbPosition TrackingObject {
		get{ return _trackingObject; }
		set{ 
			_trackingObject = value;
			_trackingObject.OnStateChange += StateChange; 
		}
	}
	public OrbPosition.State OrbState {
		get{ return _state; }
		set{ _state = value; }
	} 

	//********************************

	private OrbPosition.State _state;
	private OrbPosition _trackingObject;
	private TrailRenderer _trailRend;
	private MeshRenderer _meshRend;
	private ParticleSystemRenderer _particleSystemRend;
	private float _timeOffset;

	private void Awake () {
		
		_trailRend = GetComponent<TrailRenderer>();
		_meshRend = GetComponent<MeshRenderer>();
		_particleSystemRend = GetComponentInChildren<ParticleSystemRenderer>();

		_timeOffset = Random.Range( 0f, 1f );
	}
	private void Update () {

		if ( _trackingObject == null ) {
			return;
		}

		switch ( _state ){
			
			case OrbPosition.State.Passive:
				LivePassiveState();
				break;
			
			case OrbPosition.State.Excited:
				LiveExcitedState();
				break;
		}
	}
	private void StateChange ( OrbPosition.State newState ) {
		
		_state = newState;
	}
	
	//********************************

	private const float PASSIVE_LERP_SPEED = 0.1f;
	private const float PASSIVE_MIN_SCALE = 0.02f;
	private const float PASSIVE_MAX_SCALE = 0.05f;
	private const float PASSIVE_COLOR_R = 94;
	private const float PASSIVE_COLOR_G = 245;
	private const float PASSIVE_COLOR_B = 254;
	private const float PASSIVE_COLOR_A = 255;
	
	private void LivePassiveState () {

		var targetColor =  new Color( PASSIVE_COLOR_R/255f, PASSIVE_COLOR_G/255f, PASSIVE_COLOR_B/255f, PASSIVE_COLOR_A/255f );

		var scale = ( Mathf.Abs( Mathf.Sin( Time.time + _timeOffset ) ) * ( PASSIVE_MAX_SCALE - PASSIVE_MIN_SCALE ) ) + PASSIVE_MIN_SCALE;
		var color = Color.Lerp( _meshRend.material.color, targetColor, PASSIVE_LERP_SPEED );
		var pos = Vector3.Lerp( transform.position, _trackingObject.transform.position, PASSIVE_LERP_SPEED );

		// transform
		transform.localScale = new Vector3( scale, scale, scale );
		transform.position = pos;

		// trail
		_trailRend.material.color = color;
		_trailRend.startWidth = scale;

		// mesh rend
		_meshRend.material.color = color;

		// particles
		_particleSystemRend.material.color = color;
	}

	private const float EXCITED_LERP_SPEED = 0.3f;
	private const float EXCITED_MIN_SCALE = 0.1f;
	private const float EXCITED_MAX_SCALE = 0.15f;
	private const float EXCITED_COLOR_R = 254;
	private const float EXCITED_COLOR_G = 94;
	private const float EXCITED_COLOR_B = 169;
	private const float EXCITED_COLOR_A = 255;

	private void LiveExcitedState () {

		var targetColor = new Color( EXCITED_COLOR_R/255f, EXCITED_COLOR_G/255f, EXCITED_COLOR_B/255f, EXCITED_COLOR_A/255f );

		var scale = ( Mathf.Abs( Mathf.Sin( Time.time + _timeOffset ) ) * ( EXCITED_MAX_SCALE - EXCITED_MIN_SCALE ) ) + EXCITED_MIN_SCALE;
		var color = Color.Lerp( _meshRend.material.color, targetColor, EXCITED_LERP_SPEED );
		var pos = Vector3.Lerp( transform.position, _trackingObject.transform.position, EXCITED_LERP_SPEED );

		// transform
		transform.localScale = new Vector3( scale, scale, scale );
		transform.position = pos;

		// trail
		_trailRend.material.color = color;
		_trailRend.startWidth = scale;

		// mesh rend
		_meshRend.material.color = color;

		// particles
		_particleSystemRend.material.color = color;
	}
}
