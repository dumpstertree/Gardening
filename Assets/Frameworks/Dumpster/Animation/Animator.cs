using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using Dumpster.Core;

namespace Dumpster.Animation {

	public class Animator : MonoBehaviour {

		private void Update () {

			if ( _defualtAnimation != null ) {
				_defualtAnimation.AnimationProgress = _worldAnimationProgress;
			}

			_animations[ "Crouch"].AnimationProgress = 1.0f;

			_weightBlender.Blend();
		}



		// ****************** Public *******************

		public void ChangeProgress ( string identifier, float progress ) {

			if ( _animations.ContainsKey( identifier ) ) {

				var weightedAnimation = _animations[ identifier ];
				weightedAnimation.AnimationProgress = progress;
			}
		}
		public void SetLayerProgress ( string identifier, float progress ) {


			if ( _animations.ContainsKey( identifier ) ) {

				var weightedAnimation = _animations[ identifier ];
				weightedAnimation.LayerProgress = progress;
			}
		}
		// public void SetWeight ( string identifier, float progress ) {

		// 	if ( _animations.ContainsKey( identifier ) ) {

		// 		var blendable = _animations[ identifier ] as IBlendable;				
		// 		_weightBlender.SetWeight( blendable, progress );
		// 	}
		// }
		public void SetAnimationPlaying ( string identifier, bool playing ) {

			if ( _animations.ContainsKey( identifier ) ) {

				var blendable = _animations[ identifier ] as IBlendable;				
				_weightBlender.SetWeight( blendable, playing ? 1f : 0f );
			}
		}



		// ****************** Private *******************
		
		[SerializeField] private AnimationData _baseAnimationData;
		[SerializeField] private AnimationData[] _animationData;

		[SerializeField] private UnityEngine.Animator _animator;

		
		private WeightedAnimation _defualtAnimation;
		private Dictionary<string,WeightedAnimation> _animations;


		private Coroutine _blend;
		private string _currentAnimationIdentifier;

		private PlayableGraph _graph;
		private Playable _playable;

		private Dumpster.Core.WeightBlender _weightBlender;

		private int _numOfAnimations {
			get { return _animationData.Length + 1 ; }
		}
		private float _worldAnimationProgress {
			get { return Time.time - Mathf.Floor( Time.time ); }
		}

		private void Awake () {

			BuildGraph ();
			BuildAnimations ();
		}
		private void BuildGraph () {

			// create graph
			_graph = PlayableGraph.Create( name + "Animator" );
			_graph.SetTimeUpdateMode( DirectorUpdateMode.GameTime );

		   
			// create job
			_playable = AnimationMixerPlayable.Create( _graph, 0, true );
			_playable.SetOutputCount( 1 );
			_playable.SetInputCount( _numOfAnimations );
			

			// create output
			var output = AnimationPlayableOutput.Create( _graph, "output", _animator );
			output.SetSourcePlayable( _playable );

			
			// play the graph
			_graph.Play();


			// show graph
			GraphVisualizerClient.Show( _graph );
		}
		private void BuildAnimations () {

			_animations = new Dictionary<string,WeightedAnimation>();
			var blendables = new List<IBlendable>();

			// build dynamic animations
			for ( int i = 0; i <_animationData.Length; i++ ) {
				
				var data = _animationData[ i ];
				var weightedAnimation = new WeightedAnimation( this, _playable, i, data.Animation.GetInstance( _playable, i ) );
				_animations.Add( data.Identifier, weightedAnimation );

				blendables.Add( weightedAnimation );
			}

			// build base animation
			var baseWeightedAnimation = new WeightedAnimation( this, _playable, _numOfAnimations - 1, _baseAnimationData.Animation.GetInstance( _playable, _numOfAnimations - 1 ) );
			baseWeightedAnimation.AnimationProgress = 1.0f;

			_defualtAnimation = baseWeightedAnimation;


			_weightBlender = new WeightBlender( blendables, baseWeightedAnimation );
		}


	
		// ****************** Data *******************

		[System.Serializable]
		private class AnimationData {

			public string Identifier {
				get { return _identifier; }
			}
			public Templates.Animation Animation {
				get { return _animation; }
			}

			[SerializeField] private string _identifier;
			[SerializeField] private Templates.Animation _animation;
		}
	}
}