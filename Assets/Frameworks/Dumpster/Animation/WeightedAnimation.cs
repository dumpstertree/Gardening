using UnityEngine;
using System.Collections;
using UnityEngine.Playables;
using Dumpster.Core;

namespace Dumpster.Animation {

	public class WeightedAnimation: IBlendable {
		

		// **************** Constructor *****************

		public WeightedAnimation ( Animator animator, Playable playableParent, int inputPort, Layer layer ) {

			_animator = animator;
			_playableParent = playableParent;
			_inputPort = inputPort;
			_layer = layer;
		}

			
		// **************** IBlendable *****************

		float IBlendable.TrueWeight { 
			get { return _trueWeight; }
			set { SetTrueWeight( value ); }
		}
		float IBlendable.CurvedWeight {
			get { return _weight; }
		}
		void IBlendable.SetWeight ( float newWeight ) {

			_playableParent.SetInputWeight( _inputPort, newWeight );
		}

	
		// **************** Public *****************

		public float AnimationProgress {
			get { return _animationProgress; }
			set { SetAnimationProgress( value ); }
		}
		public float LayerProgress {
			get { return _layerProgress; }
			set { SetLayerProgress( value ); }
		}
		public bool IsPlaying {
			get { return _isPlaying; }
			set { 

				if( value != _isPlaying ) {
					
					if ( value ) { BlendIntoAnimation(); }
					else { BlendOutOfAnimation(); }

					_isPlaying = value;
				} 
			}
		}

		// **************** Private *****************
			
		const float BLEND_TIME = 0.5f;

		readonly private Layer _layer;
		readonly private Playable _playableParent;
		readonly private Animator _animator;
		readonly private int _inputPort;

		private Coroutine _blend;
		private float _trueWeight;
		private float _weight;
		private float _animationProgress;
		private float _layerProgress;

		private bool _isPlaying;


		private void BlendIntoAnimation () {

			var time = (1 - _weight) * BLEND_TIME;
				
			if ( _blend != null ) { _animator.StopCoroutine( _blend ); }
			_blend = _animator.StartCoroutine( Blend ( time, true ) );
		}
		private void BlendOutOfAnimation () {

			if ( _blend != null ) { _animator.StopCoroutine( _blend ); }
			_blend = _animator.StartCoroutine( Blend ( BLEND_TIME, false ) );
		}


		private void SetTrueWeight ( float newTrueWeight ) {

			if ( newTrueWeight != _trueWeight ) {
				
				_trueWeight = newTrueWeight;
				if ( _trueWeight == 1.0f ) {
					BlendIntoAnimation ();
				} else if (  _trueWeight == 0.0f ) {
					BlendOutOfAnimation ();
				}
				else {
					if ( _blend != null ) { _animator.StopCoroutine( _blend ); }
					_blend = _animator.StartCoroutine( Blend ( 0.1f, false ) );
				}
			}
		}
		private void SetAnimationProgress ( float progress ) {
			
			_animationProgress = progress;

			_layer.AnimationProgress = _animationProgress;
		}
		private void SetLayerProgress ( float progress ) {

			_layerProgress = progress;

			_layer.LayerProgress = _layerProgress;
		}
		

		private IEnumerator Blend ( float time, bool useCurve ) {
			
			var startWeight = _weight;
			var targetWeight = _trueWeight;
			
			for ( float t=0; t<time; t+=Time.deltaTime ) {	

				var curve = 0f;
				if ( useCurve ) {
					curve = _layer._blendCurve.Evaluate( t/time );
				} else {
					curve = t/time;
				}

				_weight = Mathf.Lerp( startWeight, targetWeight, curve );
				yield return null;
			}

			_weight = targetWeight;
		}
	}
}