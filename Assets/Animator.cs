using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Dumpster.Animation {

	public class Animator : MonoBehaviour {

		private void Update () {

			// _animations.SetProgress( _baseAnimationData.Identifier, _worldAnimationProgress );
			_animations.SetProgress( "Crouch", _worldAnimationProgress );
		}



		// ****************** Public *******************

		public void ChangeProgress ( string identifier, float progress ) {

			_animations.SetProgress( identifier, progress );
		}
		public void SetLayerProgress ( string identifier, float progress ) {

			_animations.SetLayerProgress( identifier, progress );
		}
		public void SetWeight ( string identifier, float progress ) {

			_animations.SetWeight( identifier, progress );
		}



		// ****************** Private *******************
		
		[SerializeField] private AnimationData _baseAnimationData;
		[SerializeField] private AnimationData[] _animationData;
		[SerializeField] private UnityEngine.Animator _animator;

		private WeightedAnimationStack _animations;
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

			var animations = new Dictionary<string,WeightedAnimation>();

			// build dynamic animations
			for ( int i = 0; i <_animationData.Length; i++ ) {
				
				var data = _animationData[ i ];
				var weightedAnimation = new WeightedAnimation( _playable, i, data.Animation.GetInstance( _playable, i ) );
				animations.Add( data.Identifier, weightedAnimation );
			}

			// build base animation
			var baseWeightedAnimation = new WeightedAnimation( _playable, _numOfAnimations - 1, _baseAnimationData.Animation.GetInstance( _playable, _numOfAnimations - 1 ) );
			// animations.Add( _baseAnimationData.Identifier, baseWeightedAnimation );
			baseWeightedAnimation.Layer.SetAnimationProgress( 1.0f );
			_animations = new WeightedAnimationStack( this, animations, baseWeightedAnimation );
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

		private class WeightedAnimationStack {

			public WeightedAnimationStack ( MonoBehaviour go, Dictionary<string,WeightedAnimation> animations, WeightedAnimation baseAnimation ) {
				
				_go = go;
				_animations = animations;
				_baseAnimation = baseAnimation;
			}

			private Coroutine _blend;
			private MonoBehaviour _go;
			private Dictionary<string,WeightedAnimation> _animations;
			private WeightedAnimation _baseAnimation;

			private float _weightTotal {
				get { 
					var t = 0f;
					foreach ( WeightedAnimation a in _animations.Values ){
						t += a.TargetWeight;
					}
					return t;
				}
			}

			public void SetWeight ( string identifier, float weight ) {

				if ( _animations.ContainsKey( identifier ) ) {
					
					if ( _animations[ identifier ].TargetWeight != weight ) {
						_animations[ identifier ].TargetWeight = weight;
					
						foreach ( WeightedAnimation a in _animations.Values) {

							a.AveragedTargetWeight = ( _weightTotal > 0f ) ? a.TargetWeight/_weightTotal : 0f;
						}

						_baseAnimation.AveragedTargetWeight = Mathf.Clamp01(1 - _weightTotal);

						print( _baseAnimation.AveragedTargetWeight );
						 if ( _blend != null ) { _go.StopCoroutine( _blend ); }
						_blend = _go.StartCoroutine( BlendWeights( 0.5f ) );
					}
				}
			}
			public void SetProgress ( string identifier, float weight ) {

				if ( _animations.ContainsKey( identifier ) ) {
					_animations[ identifier ].Layer.SetAnimationProgress( weight );
				}
			}
			public void SetLayerProgress ( string identifier, float weight ) {

				if ( _animations.ContainsKey( identifier ) ) {
					_animations[ identifier ].Layer.SetLayerProgress( weight );
				}
			}

			private IEnumerator BlendWeights ( float time ) {

				for( float t=0f; t<time; t+=Time.deltaTime ) {

					foreach ( WeightedAnimation w in _animations.Values ) {
						w.Weight = Mathf.Lerp( w.Weight, w.AveragedTargetWeight, t / time );
					}

					_baseAnimation.Weight = Mathf.Lerp( _baseAnimation.Weight, _baseAnimation.AveragedTargetWeight, t / time );

					yield return null;
				}

				foreach ( WeightedAnimation w in _animations.Values ) {
					w.Weight = w.AveragedTargetWeight;
				}
				
				_baseAnimation.Weight = _baseAnimation.AveragedTargetWeight;
			}
		}
		private class WeightedAnimation {

			public Layer Layer {
				get;
			}
			public float TargetWeight{
				get; set;
			}
			public float AveragedTargetWeight {
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