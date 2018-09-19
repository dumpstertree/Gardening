using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class Animation : PlayableBehaviour {

	
	// ****************** Constructor **********************
	
	public static Animation Create ( Playable parent, int parentInputPort,  Setter setter ) {

		// create playable
		var newPlayable = ScriptPlayable<Animation>.Create( parent.GetGraph() );
		newPlayable.SetInputCount( 1 );
		newPlayable.SetOutputCount( 1 );	


		// get clip
		var animation = newPlayable.GetBehaviour ();
		animation.Init( newPlayable, setter );


		// connect clip playable to parent
		parent.ConnectInput( parentInputPort, newPlayable, 0 );


		// set weight
		newPlayable.SetInputWeight( 0 , 1f );

		
		// return the clip
		return animation;
	}

	
	// ****************** Public **********************

	public ScriptPlayable<Animation> Playable {
		get { return _playable; }
	}
	public void SetProgress ( float progress ) {
		
		foreach ( Clip c in _clips ) {
			c.SetProgress( progress );
		}
	}
	public void SetGrowth ( float growth ) {

		if ( !_shouldRun ) {
			return;
		}

		// clamp growth
		growth = Mathf.Clamp( growth, 0f, 0.9999999f );


		// set default clips
		var size = ( 1f / (_clips.Count-1) );
	 	var index = Mathf.FloorToInt( growth / size );
	 	var nextIndex = index + 1;


	 	// reset all clips weights
		foreach ( Clip c in _clips ) {
			_mixer.SetInputWeight( c.Playable, 0f );
		}


		// get leaving clip
		var clip1 = _clips[ index ];
		var clip2 = _clips[ nextIndex ];


		// get weight
		var remainder = growth - ( index * size );
		var weight = 1f - (remainder / size);


		// // set weights
		_mixer.SetInputWeight( clip1.Playable, weight ); 
		_mixer.SetInputWeight( clip2.Playable, 1 - weight );
	}


	// ****************** Private **********************
	
	
	private bool _shouldRun {
		get { 
			return _clips.Count > 1; 
		}
	}

	private ScriptPlayable<Animation> _playable;
	private List<Clip> _clips;
	private Playable _mixer;

	
	private void Init ( ScriptPlayable<Animation> thisPlayable, Setter setter ) {

		// set playable
		_playable = thisPlayable;


		// create mixer
		_mixer = CreateMixer( thisPlayable, setter.Clips.Length );

		
		// create clips
		_clips = CreateClips( setter.Clips );

		
		// set default weight
		if ( _mixer.GetInputCount() > 0 ) { _mixer.SetInputWeight( 0, 1f ); }

		
		// set defaults
		SetGrowth( setter.StartingGrowth );
		SetProgress( setter.StartingProgress );
	}
	private AnimationMixerPlayable CreateMixer ( Playable parent, int numOfClips ) {


		// create mixer
		var mixer = AnimationMixerPlayable.Create( parent.GetGraph() );
		mixer.SetInputCount( numOfClips);
		mixer.SetOutputCount( 1 );


		// connect mixer
		parent.ConnectInput( 0, mixer, 0 );

		return mixer;
	}
	private List<Clip> CreateClips ( Clip.Setter[] setters ) {

		List<Clip> clips = new List<Clip>(); 
		
		for ( int i=0; i<setters.Length; i++ ) {
			clips.Add( CreateClip( setters[ i ], i ) );
		}

		return clips;
	}
	private Clip CreateClip ( Clip.Setter setter, int parentInputPort ) {
		
		return Clip.Create( _mixer, parentInputPort, setter );
	}


	// ****************** Data **********************

	[System.Serializable]
	public class Setter {

		public Clip.Setter[] Clips { 
			get{ return _clips; } 
		}
		public AvatarMask Mask { 
			get{ return _mask; } 
		}
		public float StartingGrowth {
			get{ return _startingGrowth; } 
		}
		public float StartingProgress {
			get { return _startingProgress; }
		}


		[SerializeField] private Clip.Setter[] _clips;
		[SerializeField] private AvatarMask _mask;
		[SerializeField] private float _startingGrowth = 1f;
		[SerializeField] private float _startingProgress = 1f;
	}
}
