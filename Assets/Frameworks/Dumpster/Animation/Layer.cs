using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

namespace Dumpster.Animation {
	
	public class Layer {


		// ************ Constructor *******************
		
		public Layer ( AnimationLayerMixerPlayable mixer, RangeAnimation[] animations, AnimationCurve blendCurve ) {

			_animations = animations;
			_mixer = mixer;
			_blendCurve = blendCurve;

			SetDefaultValues ();
		}

		
		// *************** Public *********************

		public float AnimationProgress {
			get { return _animationProgress; } 
			set { SetAnimationProgress( value ); }
		}
		public float LayerProgress {
			get { return _animationProgress; } 
			set { SetLayerProgress( value ); }
		}


		// *************** Private *********************


		private float _animationProgress;
		private float _layerProgress;
		public AnimationCurve _blendCurve;


		private RangeAnimation[] _animations;
		private AnimationLayerMixerPlayable _mixer;

		private int _numOfAnimations {
			get{ return _mixer.GetInputCount(); }
		}

		private void SetDefaultValues () {

			SetLayerProgress( 0f );
		}
		private void SetAnimationProgress ( float progress ) {

			_animationProgress = progress;

			foreach ( RangeAnimation animation in _animations ) {
				animation.SetProgress( _animationProgress );
			}
		}
		private void SetLayerProgress ( float progress ) {


			// clamp
			_layerProgress = Mathf.Clamp01( progress );


			// if there are no other animations dont continue
			if ( _numOfAnimations == 0 ) {
				return;
			}

			if ( _numOfAnimations == 1 ) {
				_mixer.SetInputWeight( 0, 1 - progress );
				return;
			}


			// calculate frames
			var trueTargetFrame = Mathf.CeilToInt( (float) _numOfAnimations * _layerProgress );
			var trueLeavingFrame = trueTargetFrame - 1;
			

			// wrap frames
			var targetFrame = (int) Mathf.Repeat( trueTargetFrame, _numOfAnimations );
			var leavingFrame = (int) Mathf.Repeat( trueLeavingFrame, _numOfAnimations );


			// calculate weight
			var weight = ( trueLeavingFrame > 0 ) ? _layerProgress % trueLeavingFrame : _layerProgress;
			var leavingWeight = 1f - weight;

			
			// set weights
			// _mixer.SetInputWeight( targetFrame, weight ); // these are wrong and cause the walk to not trigger correctly
			// _mixer.SetInputWeight( leavingFrame, leavingWeight ); // these are wrong and cause the walk to not trigger correctly
			
			// Debug.Log( targetFrame + " : " +  weight );

			// var weight = (_numOfAnimations * progress) - Mathf.Floor(_numOfAnimations * progress);
			// var leavingWeight = 1f - weight;

			
			// // set weights
			_mixer.SetInputWeight( 0, progress );

			if ( _mixer.GetInputCount() > 1 ) {
				_mixer.SetInputWeight( 1, 1 - progress );
			}
		}
	}
}