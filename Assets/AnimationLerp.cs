using Unity.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using UnityEngine.Experimental.Animations;

public class AnimationLerp : MonoBehaviour {

	[SerializeField] private Transform _root;

	[SerializeField] private Animator _animator;
	[SerializeField] private AnimationClip[] _clips;
	[SerializeField] private AnimationCurve _curve;

	private PlayableGraph _graph;
	private AnimationScriptPlayable _playable;
	private float _time;
	private int _lastFrame;
	private int __targetFrame;

	[SerializeField] private Transform _hips;
	[SerializeField] private float _speed;

	[SerializeField] private Vector3 _velocity;
	[SerializeField] private float _drag;
	[SerializeField] private float _acceleration;
	[SerializeField] private float _terminalVelocity;
	[SerializeField] private float _balance;

	private float _distanceCovered;

	private Vector3 _localVelocity{
		get { return _root.InverseTransformVector( _velocity ); }
	}


	private int _leftLegTargetFrame {
		get { 
			var t = Mathf.Abs( Mathf.FloorToInt(_distanceCovered % _clips.Length ) );
			if ( t != __targetFrame ) {
				_lastFrame = __targetFrame;
				__targetFrame = t;
			}
			
			return __targetFrame;
		 }
	}


	private void OnEnable () {

		// create graph
		_graph = PlayableGraph.Create( "Lerp" );
		_graph.SetTimeUpdateMode( DirectorUpdateMode.GameTime );

	   
		// create job
		var lerpJob = new LerpJob();
		lerpJob.Setup( _animator, _hips.GetComponentsInChildren<Transform>() );

	   
		// create playable
		_playable = AnimationScriptPlayable.Create( _graph, lerpJob );
		_playable.SetProcessInputs( false );
		
		foreach ( AnimationClip clip in _clips ) {
			_playable.AddInput( AnimationClipPlayable.Create( _graph, clip ), 0, 1.0f);
		}

		
		// creat output
		// var output = AnimationPlayableOutput.Create( _graph, "output", _animator );
		// output.SetSourcePlayable( _playable );
		

		// play the graph
		_graph.Play();
	}
	private void OnDisable () {
	   
		_graph.Destroy();
	}
	private void Update () {
			
		Rotate ();

		var h = Input.GetAxis( "Horizontal" );
		var v = Input.GetAxis( "Vertical" );
		var largerInput = ( Mathf.Abs( h ) > Mathf.Abs( v ) ? Mathf.Abs( h ) : Mathf.Abs( v ) );
		
		if ( largerInput > 0 ) {
			ApplyVelocity ();
		} else {
			ApplyDrag ();
		}

		ApplyTerminalVelocity ();
		Move ();
		Balance ();

		_distanceCovered += _localVelocity.z * Time.deltaTime;
		
		var job = _playable.GetJobData<LerpJob>();
		job.TargetFrameNumber = _leftLegTargetFrame;
		job.LastFrameNumber = _lastFrame;
		job.Time =  EvaluateFromCurve( _distanceCovered - Mathf.Floor( _distanceCovered ) );
		_playable.SetJobData( job );

		_time = job.Time;
	}
	private void OnDrawGizmos () {

		var leftLegRay1 = new Ray( transform.position, Quaternion.AngleAxis( _distanceCovered * 90f , _root.right ) *  _root.forward );
		var leftLegRay2 = new Ray( transform.position, Quaternion.AngleAxis( _distanceCovered * 90f , _root.right ) * -_root.forward );
		
		var rightLegRay1 = new Ray( transform.position, Quaternion.AngleAxis( _distanceCovered * 90f , _root.right ) *  _root.up );
		var rightLegRay2 = new Ray( transform.position, Quaternion.AngleAxis( _distanceCovered * 90f , _root.right ) * -_root.up );


		Gizmos.color = Color.blue;
		Gizmos.DrawRay( leftLegRay1 );
		Gizmos.DrawRay( leftLegRay2 );

		Gizmos.color = Color.red;
		Gizmos.DrawRay( rightLegRay1 );
		Gizmos.DrawRay( rightLegRay2 );
	}


	private void ApplyVelocity () {
		
		// get input
		var h = Input.GetAxis( "Horizontal" );
		var v = Input.GetAxis( "Vertical" );
		var largerInput = ( Mathf.Abs( h ) > Mathf.Abs( v ) ? Mathf.Abs( h ) : Mathf.Abs( v ) );

		// get angle of analog
		var rads = Mathf.Atan2( h, v );
		var degrees = rads * Mathf.Rad2Deg;
		var angle = Camera.main.transform.eulerAngles.y + degrees;

		// get forward vector
		var forwardRotation = Quaternion.Euler( new Vector3( 0, angle, 0 ) );
		var forwardVector = forwardRotation * Vector3.forward;

		// move in forward vector
		_velocity = Vector3.Lerp( _velocity, ( forwardVector * ( (_acceleration) * largerInput ) ), 0.3f);
	}
	private void ApplyDrag () {	

		var x = Mathf.Clamp( Mathf.Abs( _velocity.x ) - _drag * Time.deltaTime, 0f, _terminalVelocity) * ( _velocity.x > 0 ? 1 : -1);
		var y = 0f;
		var z = Mathf.Clamp( Mathf.Abs( _velocity.z ) - _drag * Time.deltaTime, 0f, _terminalVelocity) * ( _velocity.z > 0 ? 1 : -1);

		_velocity = new Vector3( x, y, z );
	}
	private void ApplyTerminalVelocity () {
		
		_velocity = new Vector3( 

			Mathf.Clamp( _velocity.x, -_terminalVelocity, _terminalVelocity ),
			Mathf.Clamp( _velocity.y, -_terminalVelocity, _terminalVelocity ),
			Mathf.Clamp( _velocity.z, -_terminalVelocity, _terminalVelocity )
		);	
	}
	private void Rotate () {
			
		if ( _velocity.magnitude > 0.01f ) {
			_root.localRotation = Quaternion.LookRotation( _velocity );
		}
	}
	private void Move () {

		_root.position += _velocity * Time.deltaTime;
	}
	private void Balance () {

		_root.localRotation = _root.rotation *
							  Quaternion.AngleAxis(  (_localVelocity.z/_terminalVelocity) * _balance, Vector3.right ) * 
							  Quaternion.AngleAxis( (-_localVelocity.x/_terminalVelocity) * _balance, Vector3.forward );
	}



	private float EvaluateFromCurve ( float trueValue ) {

		return _curve.Evaluate( trueValue );
	}	
}

public struct LerpJob : IAnimationJob {

	
	// ***************** Public ***************
	
	public int TargetFrameNumber;
	public int LastFrameNumber;
	public float Time;

	public void Setup( Animator animator, Transform[] transforms ) {

		_transformHandles = new NativeArray<TransformStreamHandle>( transforms.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
		
		for ( var i = 0; i < transforms.Length; i++ ) {
			_transformHandles[ i ] = animator.BindStreamTransform( transforms[ i ] );
		}
	}

	
	// ***************** Private ***************

	private NativeArray<TransformStreamHandle> _transformHandles;


	// ***************** IAnimationJob ***************

	void IAnimationJob.ProcessRootMotion( AnimationStream stream ) {
	}
	void IAnimationJob.ProcessAnimation( AnimationStream stream ) {


		var lastStream = stream.GetInputStream( LastFrameNumber );
		var targetStream = stream.GetInputStream( TargetFrameNumber );

		foreach ( TransformStreamHandle t in _transformHandles ) {
			
			var start  = t.GetLocalRotation( lastStream );
			var target = t.GetLocalRotation( targetStream );

			t.SetLocalRotation( stream, Quaternion.Slerp( start, target, Time  ) );
		}
	}
}