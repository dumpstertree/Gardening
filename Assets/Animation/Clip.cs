using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class Clip : PlayableBehaviour {

	
	// ****************** Constructor **********************
	
	public static Clip Create ( Playable parent, int parentInputPort, Setter setter ) {

		
		// create playable
		var newPlayable = ScriptPlayable<Clip>.Create( parent.GetGraph() );
		newPlayable.SetInputCount( 1 );
		newPlayable.SetOutputCount( 1 );	


		// get clip
		var clip = newPlayable.GetBehaviour ();
		clip.Init( setter, newPlayable );
	

		// connect clip playable to parent
		parent.ConnectInput( parentInputPort, newPlayable, 0 );


		// set weight
		newPlayable.SetInputWeight( 0 , 1f );

		clip._playable = newPlayable;
		
		// return the clip
		return clip;
	}

	// ****************** Public **********************

	public ScriptPlayable<Clip> Playable {
		get { return _playable; }
	}

	public void SetProgress ( float progress ) {

		

		// if it should not run quit
		if ( !_shouldRun ) {
			return;
		}


		// clamp progress
		progress = Mathf.Clamp01( progress );


		// set default clips
	 	var index = -1;


	 	// iterate through clips to find out 
		var progressAsTime = progress * _totalClipLength;
		var startTime = 0f;

		for ( int i=0; i<_numOfClips; i++ ) {
		 	
		 	var t = TimeToIndex( i );
		 	if ( t > progressAsTime ) { break; }

		 	index = i;
			startTime = t;
		}

	 	var nextIndex = Mathf.RoundToInt( Mathf.Repeat( (float)index + 1f, (float)_clips.Count) );

	 	// reset all clips weights
		foreach ( AnimationClipPlayable c in _clips ) {
			_mixer.SetInputWeight( c, 0f );
		}


		// get leaving clip
		var clip1 = _clips[ index ];
		var clip2 = _clips[ nextIndex ];


		// get weight
		var timeIntoThisAnimation = progressAsTime - startTime;
		var weight = 1 - timeIntoThisAnimation / clip1.GetAnimationClip().length;


		// set weights
		_mixer.SetInputWeight( clip1, weight ); 
		_mixer.SetInputWeight( clip2, 1 - weight );


		// set time
		// clip1.SetTime( weight * clip1.GetAnimationClip().length );
		// clip2.SetTime( weight * clip2.GetAnimationClip().length );
	}
	

	// ****************** Private **********************
	
	private bool _shouldRun {
		get { 
			return _clips.Count > 0; 
		}
	}
	private int _numOfClips {
		get { 
			return ( _looping ) ? _clips.Count : _clips.Count -1; 
		}
	}

	private List<AnimationClipPlayable> _clips;
	private Playable _mixer;
	private ScriptPlayable<Clip> _playable;
	private float _totalClipLength;
	private bool _looping;


	private void Init ( Setter setter, ScriptPlayable<Clip> thisPlayable ) {
		
		// init data
		_looping = setter.Looping;

		// create mixer
		_mixer = CreateMixer ( thisPlayable, setter.Clips.Length );
		
		// create clips
		_clips = CreateClips( _mixer, setter.Clips );

		// calculate total length
		_totalClipLength = 0f;

		for( int i=0; i<_numOfClips; i++ ) {
			_totalClipLength += _clips[ i ].GetAnimationClip().length;
		}
	}
	private AnimationMixerPlayable CreateMixer ( Playable parent, int numOfClips ) {

		// create mixer
		var mixer = AnimationMixerPlayable.Create( parent.GetGraph() );
		mixer.SetInputCount( numOfClips);
		mixer.SetOutputCount( 1 );
		mixer.SetInputWeight( 0, 1f );

		// connect mixer
		parent.ConnectInput( 0, mixer, 0 );

		return mixer;
	}
	private List<AnimationClipPlayable> CreateClips ( Playable parent, AnimationClip[] clips  ) {

		var clipPlayables = new List<AnimationClipPlayable>();
		
		for ( int i=0; i<clips.Length; i++ ){
			clipPlayables.Add(  CreateClip( parent.GetGraph(), _mixer, i, clips[ i ] )  );
		}

		return clipPlayables;
	}
	private AnimationClipPlayable CreateClip ( PlayableGraph graph, Playable parent, int parentInputPort, AnimationClip clip ) {

		var playable = AnimationClipPlayable.Create( graph, clip );
		
		playable.SetInputCount( 0 );
		playable.SetOutputCount( 1 );
		
		parent.ConnectInput( parentInputPort, playable, 0 );

		return playable;
	}
	private float TimeToIndex ( int index ) {

		if ( index > _numOfClips ) {
			return 0f;
		}

		var length = 0f;
		for( int i=0; i<index; i++ ) {
			
			var c = _clips[ index ];
			var l = c.GetAnimationClip().length;
			length += l;
		}

		return length;
	}


	// ****************** Data **********************

	[System.Serializable]
	public class Setter {

 		public AnimationClip[] Clips { 
 			get { return _clips; } 
 		}
 		public bool Looping {
 			get { return _looping; }
 		}

 		[SerializeField] private AnimationClip[] _clips;
 		[SerializeField] private bool _looping;
	}
}