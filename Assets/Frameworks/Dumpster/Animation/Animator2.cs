using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;


public class Animator2 : MonoBehaviour {

	
	// ****************** Public **********************

	public void SetProgress ( string animation, float progress ) {

		if ( _animations.ContainsKey( animation ) ) {
			_animations[ animation ].SetProgress( progress );
		}
	}
	public void SetGrowth ( string animation, float growth ) {
		
		if ( _animations.ContainsKey( animation ) ) {
			_animations[ animation ].SetGrowth( growth );
		}
	}
	public void SetWeight ( string animation, float weight) {

		if ( _animations.ContainsKey( animation ) ) {
			_playable.SetInputWeight( _animations[ animation ].Playable, weight );
		}
	}

	
	// ****************** Private **********************

	[SerializeField] private Animator _animator;
	[SerializeField] private AnimationLayer[] _animationLayers;

	private PlayableGraph _graph;
	private AnimationLayerMixerPlayable _playable;
	private Dictionary<string,Animation> _animations;

	private void Awake () {

		_animations = new Dictionary<string,Animation>();		 
		 var layers = new List<AnimationLayer>( _animationLayers ) ;
		layers.Sort( ( a1, a2 )=> a1.Priority.CompareTo( a2.Priority ) );
		
		BuildGraph ();
		BuildAnimations ( layers );
	}
	private void BuildGraph () {

		// create graph
		_graph = PlayableGraph.Create( name + "Animator" );
		_graph.SetTimeUpdateMode( DirectorUpdateMode.GameTime );

	   
		// create job
		_playable = AnimationLayerMixerPlayable.Create( _graph );
		_playable.SetOutputCount( 1 );
		_playable.SetInputCount( _animationLayers.Length );


		// create output
		var output = AnimationPlayableOutput.Create( _graph, "output", _animator );
		output.SetSourcePlayable( _playable );
		output.SetSourceOutputPort( 0 );


		// play the graph
		_graph.Play();


		// show graph
		GraphVisualizerClient.Show( _graph );
	}
	private void BuildAnimations ( List<AnimationLayer> animationLayers ) {

		for ( int i=0; i<animationLayers.Count; i++ ) {
		
			var l = animationLayers[ i ];
			_animations.Add( l.Name, Animation.Create( _playable, i, l.Animation ) );

			if ( l.Animation.Mask != null ) {
				_playable.SetLayerMaskFromAvatarMask( (uint)i, l.Animation.Mask );
			}
		}
	}

	[System.Serializable]
	private class AnimationLayer {

		[SerializeField] public int Priority;
		[SerializeField] public string Name;
		[SerializeField] public Animation.Setter Animation;
	}
}