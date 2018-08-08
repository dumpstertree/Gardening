using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using UnityEngine.Experimental.Animations;
using System.Collections.Generic;

public class FrameAnimation : MonoBehaviour {

	[Range( 0, 1f)]
	[SerializeField] private float _progress;
	
	[Header( "Animation" )]
	[SerializeField] private AnimationClip[] _clips;
	[SerializeField] private AnimationCurve _speedCurve;

	private PlayableGraph _graph;
	private AnimationScriptPlayable _playable;
	private Dictionary<int,AnimationClipPlayable> _clipIndex = new Dictionary<int,AnimationClipPlayable>();
	private AnimationMixerPlayable _mixer;


	public AnimationMixerPlayable RegisterToGraph ( Playable playable, Animator animator, AnimationPlayableOutput output, int port ) {


		// create mixer playable
		_mixer = AnimationMixerPlayable.Create( playable.GetGraph() );
		_mixer.SetInputCount( _clips.Length );
		_mixer.SetOutputCount( 1 );

		
		// connect the mixer playable
		playable.ConnectInput( port, _mixer, 0);


		// create clip playables
		for( int i=0; i<_clips.Length; i++ ){
			
			// create clip playable
			var clip = _clips[ i ];
			var clipPlayable = AnimationClipPlayable.Create( playable.GetGraph(), clip );
			clipPlayable.SetInputCount( 0 );
			clipPlayable.SetOutputCount( 1 );

			// connect the clip playable to the mixer playable
			_mixer.ConnectInput( i, clipPlayable, 0 );

			// store the clip playable for later
			_clipIndex.Add( i, clipPlayable );
		}

		// graph.Connect( _mixer, port, playable, port);

		// var output2 = AnimationPlayableOutput.Create( _graph, "output", animator );
		// output2.SetSourcePlayable( _mixer );

		return _mixer;
	}
	public void SetProgress ( float progress ) {

		var curvedProgress = EvaluateFromCurve( progress );

		var targetFrame = Mathf.CeilToInt( (float)(_clips.Length-1) * curvedProgress );
		var weight = (float)(_clips.Length-1) * curvedProgress - Mathf.FloorToInt( (float)(_clips.Length-1) * curvedProgress );

		foreach ( AnimationClipPlayable a in _clipIndex.Values ) {
			_mixer.SetInputWeight( a, 0f );
		}

		_mixer.SetInputWeight( _clipIndex[ targetFrame ], 1f );
	}
	private float EvaluateFromCurve ( float trueValue ) {

		return _speedCurve.Evaluate( trueValue );
	}
	private void Update () {

		SetProgress( _progress );
	}
}
