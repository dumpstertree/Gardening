using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Dumpster.Animation {

	public class Animator : MonoBehaviour {

		private void Update () {

			_animations[ _baseAnimationData.Identifier ].Layer.SetAnimationProgress( _worldAnimationProgress );
			_animations[ "Crouch" ].Layer.SetAnimationProgress( _worldAnimationProgress );
		}

		// ****************** Public *******************

		public void SetAsActiveAnimation ( string identifier ) {

			if ( identifier == _currentAnimationIdentifier ) {
				return;
			}

			if ( _animations.ContainsKey( identifier ) ) {
				 
				 print( identifier );
				 _currentAnimationIdentifier = identifier;

				 foreach ( WeightedAnimation w in _animations.Values ) {
				 	w.TargetWeight = 0f;
				 }

				 _animations[ identifier ].TargetWeight = 1;

				 if ( _blend != null ) { StopCoroutine( _blend ); }
				_blend = StartCoroutine( BlendWeights( 0.5f ) );
			}
		}
		public void ChangeProgress ( string identifier, float progress ) {

			if ( _animations.ContainsKey( identifier ) ) {
				_animations[ identifier ].Layer.SetAnimationProgress( progress );
			}
		}
		public void SetLayerProgress ( string identifier, float progress ) {

			if ( _animations.ContainsKey( identifier ) ) {
				_animations[ identifier ].Layer.SetLayerProgress( progress );
			}
		}


		// ****************** Private *******************
		
		[SerializeField] private AnimationData _baseAnimationData;
		[SerializeField] private AnimationData[] _animationData;
		[SerializeField] private UnityEngine.Animator _animator;

		private Dictionary<string,WeightedAnimation> _animations;
		private Coroutine _blend;
		private string _currentAnimationIdentifier;

		private PlayableGraph _graph;
		private Playable _playable;

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
			_playable = AnimationMixerPlayable.Create( _graph );
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

			// build dynamic animations
			for ( int i = 0; i <_animationData.Length; i++ ) {
				
				var data = _animationData[ i ];
				var weightedAnimation = new WeightedAnimation( _playable, i, data.Animation.GetInstance( _playable, i ) );
				_animations.Add( data.Identifier, weightedAnimation );
			}

			// build base animation
			var baseWeightedAnimation = new WeightedAnimation( _playable, _numOfAnimations - 1, _baseAnimationData.Animation.GetInstance( _playable, _numOfAnimations - 1 ) );
			_animations.Add( _baseAnimationData.Identifier, baseWeightedAnimation );
		}

		private IEnumerator BlendWeights ( float time ) {

			for( float t=0f; t<time; t+=Time.deltaTime ) {

				foreach ( WeightedAnimation w in _animations.Values ) {

					w.Weight = Mathf.Lerp( w.Weight, w.TargetWeight, t / time );
				}


				yield return null;
			}
			foreach ( WeightedAnimation w in _animations.Values ) {

				w.Weight = w.TargetWeight;
			}

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

		private class WeightedAnimation {

			public Layer Layer {
				get;
			}
			public float TargetWeight{
				get; set;
			}
			public float Weight {
				get { return _weight; }
				set { SetWeight( value ); }
			}
			
			
			private Playable _playableParent;
			private int _inputPort;
			private float _weight;

			public WeightedAnimation ( Playable playableParent, int inputPort, Layer layer ) {

				Layer = layer;

				_playableParent = playableParent;
				_inputPort = inputPort;
			}

			private void SetWeight ( float newWeight ) {

				_weight = newWeight;
				_playableParent.SetInputWeight( _inputPort, _weight );
			}
		}
	}
}