using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using System.Collections.Generic;

namespace Dumpster.Animation.Templates {

	public class Animation : ScriptableObject {

		[Header( "Animation" )]
		[SerializeField] private AnimationTemplate[] _animationTemplates;
		[SerializeField] private AnimationCurve _speedCurve;
		[SerializeField] private AnimationCurve _blendCurve;

		public Dumpster.Animation.Layer GetInstance ( Playable playableParent, int port ) {

			var layerMixer = AnimationLayerMixerPlayable.Create( playableParent.GetGraph(), 0 );
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

			var layer = new Layer( layerMixer, animations.ToArray(), _blendCurve );
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

			SetDefaultValues ();
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


			// wrap frames
			var targetFrame = (int) Wrap( trueTargetFrame, _numOfFrames );
			var leavingFrame = (int) Wrap( trueLeavingFrame, _numOfFrames );


			// calculate weight
			var weight = ( trueLeavingFrame > 0 ) ? trueCurvedFrame % trueLeavingFrame : trueCurvedFrame;
			var leavingWeight = 1f - weight;


			// set weights
			_mixer.SetInputWeight( targetFrame, weight );
			_mixer.SetInputWeight( leavingFrame, leavingWeight );

			// _mixer.SetInputWeight( 0, progress );
			// _mixer.SetInputWeight( 1, 1f - progress );
		}

		
		// ***************** Private ***************

		private AnimationMixerPlayable _mixer;
		private AnimationCurve _curve;

		private int _numOfFrames {
			get{ return _mixer.GetInputCount(); }
		}

		private void SetDefaultValues () {

			SetProgress ( 0 );
		}
		private float EvaluateFromCurve ( float trueValue ) {

			return _curve.Evaluate( trueValue );
		}
		private float Wrap ( float index, float max ) {

			return Mathf.Repeat( index, max );
		}
	}
}