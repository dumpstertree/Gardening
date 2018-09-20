using System.Collections;
using UnityEngine;

public class KindaBlendTree {

	private Animator2 _animator;

	private AnimationDampener _posX;
	private AnimationDampener _negX;
	private AnimationDampener _posY;
	private AnimationDampener _negY;

	private float _lastX;
	private float _lastY;
	private float _lastWeight;

	private Coroutine _weightCoroutine;

	public KindaBlendTree ( Animator2 animator, string posX, string negX, string posY, string negY ) {
		
		_animator = animator;
		
		_posX = new AnimationDampener( animator, posX );
		_negX = new AnimationDampener( animator, negX );
		_posY = new AnimationDampener( animator, posY );
		_negY = new AnimationDampener( animator, negY );
	}

	public void SetProgress ( float progress, float overTime = 0f ) {
		
		_posX.SetProgress( progress, overTime );
		_negX.SetProgress( progress, overTime );
		_posY.SetProgress( progress, overTime );
		_negY.SetProgress( progress, overTime );
	}
	public void SetWeight ( float weight, float overTime = 0f ) {

		if ( Mathf.Approximately( 0f, overTime ) ) {
		
			_lastWeight = weight;
			UpdateAnimations ();
		
		} else {

			_weightCoroutine = _animator.StartCoroutine (

				Dampen( _lastWeight, weight, overTime, 
					
					v => {
						_lastWeight = v;
						UpdateAnimations ();
					}
				)
			);
		}
	}
	public void SetBlendPoint ( float x, float y ) {

		x = Mathf.Clamp( x, -1f, 1f );
		y = Mathf.Clamp( y, -1f, 1f );
		
		var magnitude = new Vector2( x, y ).magnitude ;
		var xWeight = x / magnitude;
		var yWeight = y / magnitude;

		_lastX = xWeight;
		_lastY = yWeight;

		UpdateAnimations();
	}
	private void UpdateAnimations () {


		// set posX
		if ( _lastX > 0 ) {
			_posX.SetWeight( _lastX * _lastWeight);
		} else {
			_posX.SetWeight( 0 );
		}


		// set negX
		if ( _lastX < 0 ) {
			_negX.SetWeight( Mathf.Abs( _lastX ) * _lastWeight);
		} else {
			_negX.SetWeight( 0 );
		}


		// set posY
		if ( _lastY > 0 ) {
			_posY.SetWeight( _lastY * _lastWeight);
		} else {
			_posY.SetWeight( 0 );
		}


		// set negY
		if ( _lastY < 0 ) {
			_negY.SetWeight( Mathf.Abs( _lastY ) * _lastWeight);
		} else {
			_negY.SetWeight( 0 );
		}
	}

	private IEnumerator Dampen ( float startValue, float targetValue, float time, System.Action<float> onUpdate ) {

		for ( float t=0f; t<time; t+=Time.deltaTime ) {

			var progress = t/time;
			var newValue = Mathf.Lerp( startValue, targetValue, progress );

			onUpdate( newValue );
			yield return null;
		}

		onUpdate( targetValue );
	}
}
