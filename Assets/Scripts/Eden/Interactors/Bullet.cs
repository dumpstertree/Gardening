using UnityEngine;
using Dumpster.Core.BuiltInModules.Effects;

public class Bullet : MonoBehaviour {

	// ********** PUBLIC **************

	public void SetBullet ( Eden.Life.BlackBox user , HitData hitData ) {
		
		_shooter = user;
		_hitData = hitData;

		CreateCasing();

		_startPos = user.ProjectileSpawner.position;

		EdensGarden.Instance.Effects.Shake( _startPos, ShakePower.Miniscule, DecayRate.Quick );
		EdensGarden.Instance.Effects.FreezeFrame( 0.05f );

		if (Physics.Raycast( Camera.main.transform.position, Camera.main.transform.forward, out _targetHit, Mathf.Infinity, _layermask )) {

			var interactable = _targetHit.collider.GetComponentInChildren<Eden.Interactable.InteractableObject>();
			if( interactable != null ) {
				_targetObject = interactable;
			}

			_randomPoint1 = Vector3.Lerp( _startPos , _targetPos,  .2f ) + new Vector3( Random.Range( -_randomPoint1MaxOffset, _randomPoint1MaxOffset ), Random.Range( -_randomPoint1MaxOffset, _randomPoint1MaxOffset ), Random.Range( -_randomPoint1MaxOffset, _randomPoint1MaxOffset ) );
			_randomPoint2 = Vector3.Lerp( _startPos , _targetPos,  .8f ) + new Vector3( Random.Range( -_randomPoint2MaxOffset, _randomPoint2MaxOffset ), Random.Range( -_randomPoint2MaxOffset, _randomPoint2MaxOffset ), Random.Range( -_randomPoint2MaxOffset, _randomPoint2MaxOffset ) );
		} 
	}

	// *********** PRIVATE ************

	[SerializeField] private AnimationCurve _speedCurve;
	[SerializeField] private float _speed = 1.0f;
	[SerializeField] private float _randomPoint1MaxOffset = 1.0f;
	[SerializeField] private float _randomPoint2MaxOffset = 0.5f;

	[SerializeField] private GameObject _casingPrefab;
	[SerializeField] private LayerMask _layermask;
	[SerializeField] private GameObject _dustEffectPrefab;
	[SerializeField] private GameObject _collideEffectPrefab;

	private const float CASING_KILL_TIME = 60.0f;

	private Vector3 _startPos;
	private Eden.Life.BlackBox _shooter;
	private HitData _hitData;
	private Eden.Interactable.InteractableObject _targetObject;
	private RaycastHit _targetHit;

	private float _time;
	private Vector3 _randomPoint1;
	private Vector3 _randomPoint2;

	private Vector3 _targetPos {
		get{ return _targetObject != null ? _targetObject.transform.position + (_targetHit.point - _targetObject.transform.position) : _targetHit.point; }
	}

	// ***********************

	private void Update () {

		var distance = Vector3.Distance( transform.position, _targetPos );
		if ( distance > 0.1f ) {
			var frac = _speedCurve.Evaluate( _time += (Time.deltaTime * _speed) );
			transform.position = CalculateCubicBezierPoint( frac, _startPos, _randomPoint1, _randomPoint2, _targetPos ); 

			RaycastHit hit;
			if (Physics.Raycast( transform.position, Vector3.down, out hit, Mathf.Infinity, _layermask )) {
				var inst = Instantiate( _dustEffectPrefab );
				inst.transform.position = hit.point; 
			}

		} else {
			Collide();
		}
	}
	

	private void Collide () {

		if ( _targetObject != null ) {
			_targetObject.HitDelegate.Hit( _shooter, _hitData );
		}

		var inst = Instantiate( _collideEffectPrefab );
		inst.transform.position = transform.position;
		
		Destroy( gameObject );
	}
	private void CreateCasing () {
		
		var inst = Instantiate( _casingPrefab );
		inst.transform.position = transform.position;
		inst.transform.rotation = transform.rotation * Quaternion.AngleAxis( 90, Vector3.right );

		var velocity = new Vector3();
		velocity.x = Random.Range( -3, 1);
		velocity.y = Random.Range(  1, 3);
		velocity.z = Random.Range( -3, 3);

		inst.GetComponent<Rigidbody>().velocity = velocity;

		EdensGarden.Instance.Async.WaitForSeconds( CASING_KILL_TIME, () => { Destroy( inst ); } );
	}
	private Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
	        
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;
        
        Vector3 p = uuu * p0; 
        p += 3 * uu * t * p1; 
        p += 3 * u * tt * p2; 
        p += ttt * p3; 
        
        return p;
	}
}
