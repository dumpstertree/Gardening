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
		if ( _blendTrees.ContainsKey( animation ) ) {
			_blendTrees[ animation ].SetProgress( progress );
		}
	}
	public void SetGrowth ( string animation, float growth ) {
		
		if ( _animations.ContainsKey( animation ) ) {
			_animations[ animation ].SetGrowth( growth );
		}
	}
	public void SetGrowthPoint ( string animation, float growthX, float growthY ) {
		
		if ( _blendTrees.ContainsKey( animation ) ) {
			_blendTrees[ animation ].SetGrowthPoint( growthX, growthY );
		}
	}

	
	// ****************** Private **********************

	[SerializeField] private Animator _animator;
	[SerializeField] private AnimationLayer[] _animationLayers;
	[SerializeField] private BlendTreeLayer[] _blendTreeLayers;

	private PlayableGraph _graph;
	private AnimationLayerMixerPlayable _playable;
	private Dictionary<string,Animation> _animations;
	private Dictionary<string,BlendTree> _blendTrees;


	private void Awake () {

		_animations = new Dictionary<string,Animation>();
		_blendTrees = new Dictionary<string,BlendTree>();
		
		BuildGraph ();
		BuildAnimations ();
		BuildBlendTrees ();

		// this will change when layers are fully supported
		_playable.SetInputWeight( 0, 1f );
	}
	private void BuildGraph () {

		// create graph
		_graph = PlayableGraph.Create( name + "Animator" );
		_graph.SetTimeUpdateMode( DirectorUpdateMode.GameTime );

	   
		// create job
		_playable = AnimationLayerMixerPlayable.Create( _graph );
		_playable.SetOutputCount( 1 );
		_playable.SetInputCount( 3 );


		// create output
		var output = AnimationPlayableOutput.Create( _graph, "output", _animator );
		output.SetSourcePlayable( _playable );
		output.SetSourceOutputPort( 0 );


		// play the graph
		_graph.Play();


		// show graph
		GraphVisualizerClient.Show( _graph );
	}
	private void BuildAnimations () {

		for ( int i=0; i<_animationLayers.Length; i++ ) {
			var l = _animationLayers[ i ];
			_animations.Add( l.Name, Animation.Create( _playable, i, l.Animation ) );
		}
	}
	private void BuildBlendTrees () {

		for ( int i=0; i<_blendTreeLayers.Length; i++ ) {
			var l = _blendTreeLayers[ i ];
			_blendTrees.Add( l.Name, BlendTree.Create( _playable, i, l.BlendTree ) );
		}
	}


	[System.Serializable]
	private class AnimationLayer {

		public int Priority;
		public string Name;
		public Animation.Setter Animation;
	}

	[System.Serializable]
	private class BlendTreeLayer {

		public int Priority;
		public string Name;
		public BlendTree.Setter BlendTree;
	}
}