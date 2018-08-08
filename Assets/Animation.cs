﻿using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using System.Collections.Generic;

namespace Dumpster.Animation.Templates {

	public class Animation : ScriptableObject {

		[Header( "Animation" )]
		[SerializeField] private AnimationTemplate[] _animationTemplates;
		[SerializeField] private AnimationCurve _speedCurve;

		public Dumpster.Animation.Layer GetInstance ( Playable playableParent, int port ) {

			var layerMixer = AnimationMixerPlayable.Create( playableParent.GetGraph(), 0, true );
			layerMixer.SetInputCount( _animationTemplates.Length );
			layerMixer.SetOutputCount( 1 );

			playableParent.ConnectInput( port, layerMixer, 0);

			var animations = new List<RangeAnimation>();
			var x = 0;
			foreach( AnimationTemplate animationTemplate in _animationTemplates ) {


				// create animation mixer playable
				var mixer = AnimationMixerPlayable.Create( playableParent.GetGraph(), 0, true );
				mixer.SetInputCount( animationTemplate.Clips.Length );
				mixer.SetOutputCount( 1 );

				
				// connect the mixer playable
				layerMixer.ConnectInput( x, mixer, 0 );


				// create clip playables
				for( int i=0; i<animationTemplate.Clips.Length; i++ ) {
					
					// create clip playable
					var clip = animationTemplate.Clips[ i ];
					var clipPlayable = AnimationClipPlayable.Create( playableParent.GetGraph(), clip );
					clipPlayable.SetInputCount( 0 );
					clipPlayable.SetOutputCount( 1 );

					
					// connect the clip playable to the mixer playable
					mixer.ConnectInput( i, clipPlayable, 0 );
				}

				animations.Add( new Dumpster.Animation.RangeAnimation( mixer, _speedCurve ) );
				x++;
			}

			var layer =  new Layer( layerMixer, animations.ToArray() );
			return layer;
		}

		[System.Serializable]
		private class AnimationTemplate {

			public AnimationClip[] Clips;
		}
	}
}

namespace Dumpster.Animation {

	public class RangeAnimation {


		// ***************** Constructor ***************

		public RangeAnimation ( AnimationMixerPlayable mixer, AnimationCurve curve ) {

			_mixer = mixer;
			_curve = curve;
		}

		
		// ***************** Public ***************

		public void SetProgress ( float progress ) {

			// clamp progress
			progress = Mathf.Clamp01( progress );


			// calculate speed
			var curvedProgress = EvaluateFromCurve( progress ) * progress;
			var trueCurvedFrame = ((float)_numOfFrames) * curvedProgress;

			// calculate frames
			var trueTargetFrame = Mathf.CeilToInt( trueCurvedFrame );
			var trueLeavingFrame = trueTargetFrame - 1f;
			var targetFrame = (int) Wrap( trueTargetFrame, _numOfFrames );
			var leavingFrame = (int) Wrap( trueLeavingFrame, _numOfFrames );


			// calculate weight
			var weight = ( trueLeavingFrame > 0 ) ? trueCurvedFrame % trueLeavingFrame : trueCurvedFrame;
			var leavingWeight = 1f - weight;


			// set weights
			_mixer.SetInputWeight( leavingFrame, leavingWeight );
			_mixer.SetInputWeight( targetFrame, weight );

			// Debug.Log( trueCurvedFrame +  " % " + leavingFrame);
			// Debug.Log( "Target : " + targetFrame + " : " + weight );
			// Debug.Log( "Leaving : " + leavingFrame + " : " + leavingWeight );

		}

		
		// ***************** Private ***************

		private AnimationMixerPlayable _mixer;
		private AnimationCurve _curve;

		private int _numOfFrames {
			get{ return _mixer.GetInputCount(); }
		}


		private float EvaluateFromCurve ( float trueValue ) {

			return _curve.Evaluate( trueValue );
		}
		private float Wrap ( float index, float max ) {

			return Mathf.Repeat( index, max );
		}
	}
}

namespace Dumpster.Animation {

	public class Layer {

		private RangeAnimation[] _animations;
		private AnimationMixerPlayable _mixer;

		private int _numOfAnimations {
			get{ return _animations.Length; }
		}

		public Layer ( AnimationMixerPlayable mixer, RangeAnimation[] animations ) {

			_animations = animations;
			_mixer = mixer;

			if ( _mixer.GetInputCount() > 0 ) {
				_mixer.SetInputWeight( 0, 1.0f );
			}
		}


		public void SetAnimationProgress ( float progress ) {

			foreach ( RangeAnimation animation in _animations ) {
				animation.SetProgress( progress );
			}
		}
		public void SetLayerProgress ( float progress ) {

			progress = Mathf.Clamp01( progress );


			// calculate frames
			var trueTargetFrame = Mathf.CeilToInt( (float) _numOfAnimations * progress);
			var trueLastFrame = trueTargetFrame - 1;
			var targetFrame = (int) Mathf.Repeat( trueTargetFrame, _numOfAnimations );
			var lastFrame   = (int) Mathf.Repeat( trueLastFrame,   _numOfAnimations );


			// calculate weight
			var weight = (_numOfAnimations * progress) - Mathf.Floor(_numOfAnimations * progress);
			var leavingWeight = 1f - weight;

			
			// set weights
			_mixer.SetInputWeight( 0, 1- progress );
			_mixer.SetInputWeight( 1, progress );
		}
	}
}