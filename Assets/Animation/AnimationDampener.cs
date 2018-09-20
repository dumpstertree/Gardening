using System.Collections;
using UnityEngine;

public class AnimationDampener  {

	private Animator2 _animator;
	private string _animation;

	private float _weight;
	private float _progress;
	private float _growth;

	private Coroutine _weightCoroutine;
	private Coroutine _progressCoroutine;
	private Coroutine _growthCoroutine;

	public AnimationDampener ( Animator2 animator, string animation ) {
		
		_animator = animator;
		_animation = animation;
	}

	public void SetWeight ( float targetvalue, float overTime = 0f ) {

		if ( _weightCoroutine != null ) {
			_animator.StopCoroutine( _weightCoroutine );
		}

		if ( overTime > 0f ) {
			
			_weightCoroutine = _animator.StartCoroutine (

				Dampen( _weight, targetvalue, overTime, 
					
					v => {
						_weight = v;
						_animator.SetWeight( _animation, _weight );
					}
				)
			);
		} 
		else {

			_weight = targetvalue;
			_animator.SetWeight( _animation, _weight );
		}
	}
	public void SetProgress ( float targetvalue, float overTime = 0f ) {

		if ( _progressCoroutine != null ) {
			_animator.StopCoroutine( _progressCoroutine );
		}

		if ( overTime > 0f ) {
			
			_progressCoroutine = _animator.StartCoroutine (

				Dampen( _progress, targetvalue, overTime, 
					
					v => {
						_progress = v;
						_animator.SetProgress( _animation, _progress );
					}
				)
			);
		} 
		else {

			_progress = targetvalue;
			_animator.SetProgress( _animation, _progress );
		}
	}
	public void SetGrowth ( float targetvalue, float overTime = 0f ) {

		if ( _growthCoroutine != null ) {
			_animator.StopCoroutine( _growthCoroutine );
		}

		if ( overTime > 0f ) {
			
			_growthCoroutine = _animator.StartCoroutine (

				Dampen( _growth, targetvalue, overTime, 
					
					v => {
						_growth = v;
						_animator.SetGrowth( _animation, _growth );
					}
				)
			);
		} 
		else {

			_growth = targetvalue;
			_animator.SetGrowth( _animation, _growth );
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
