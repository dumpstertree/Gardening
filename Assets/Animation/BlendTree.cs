using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class BlendTree : PlayableBehaviour {

	// ****************** Constructor **********************
	
	public static BlendTree Create ( AnimationLayerMixerPlayable parent, int parentInputPort, Setter setter ) {

		// create playable
		var newPlayable = ScriptPlayable<BlendTree>.Create( parent.GetGraph() );
		newPlayable.SetInputCount( 4 );
		newPlayable.SetOutputCount( 1 );	


		// get clip
		var blendTree = newPlayable.GetBehaviour ();
		blendTree.Init( newPlayable, setter );


		// connect clip playable to parent
		parent.ConnectInput( parentInputPort, newPlayable, 0 );


		// set weight
		newPlayable.SetInputWeight( 0 , 1f );

		
		// return the clip
		return blendTree;
	}


	// ****************** Public **********************
	
	public ScriptPlayable<BlendTree> Playable {
		get { return _playable; }
	}

	public void SetProgress ( float progress ) {

		if ( _posX != null ) {
			_posX.SetProgress( progress ); 
		}

		if ( _negX != null ) {
			_negX.SetProgress( progress ); 
		}

		if ( _posY != null ) {
			_posY.SetProgress( progress ); 
		}

		if ( _negY != null ) {
			_negY.SetProgress( progress ); 
		}
	}
	public void SetGrowthPoint ( float growthX, float growthY ) {

		growthX = Mathf.Clamp( growthX, -1f, 1f );
		growthY = Mathf.Clamp( growthY, -1f, 1f );

		var distance = Vector2.Distance( Vector2.zero, new Vector2( growthX, growthY ) );
		var centerWeight = Mathf.Clamp( _centerFadeinDistance - distance, 0f, _centerFadeinDistance)/_centerFadeinDistance;
		var magnitude = new Vector2( growthX, growthY ).magnitude + centerWeight;
		var xWeight = growthX / magnitude;
		var yWeight = growthY / magnitude;

		// set center
		if ( _center != null ) {
			_mixer.SetInputWeight( _center.Playable, centerWeight );
			_center.SetGrowth( centerWeight ); 
		}

		Debug.Log( xWeight );

		// set posX
		if ( _posX != null ) {
			if ( growthX > 0 ) {
				_mixer.SetInputWeight( _posX.Playable, xWeight );
				_posX.SetGrowth( magnitude ); 
			} else {
				_mixer.SetInputWeight( _posX.Playable, 0 );
				_posX.SetGrowth( 0 ); 
			}
		} 

		// set negX
		if ( _negX != null ) {
			if ( growthX < 0 ) {
				_mixer.SetInputWeight( _negX.Playable, Mathf.Abs( xWeight ) );
				_negX.SetGrowth( magnitude ); 
			} else {
				_mixer.SetInputWeight( _negX.Playable, 0 );
				_negX.SetGrowth( 0 ); 
			}
		}

		// set posX
		if ( _posY != null ) {
			if ( growthY > 0 ) {
				_mixer.SetInputWeight( _posY.Playable, yWeight );
				_posY.SetGrowth( magnitude ); 
			} else {
				_mixer.SetInputWeight( _posY.Playable, 0 );
				_posY.SetGrowth( 0 ); 
			}
		}

		// set negY
		if ( _negY != null && growthY < 0 ) {
			if ( growthY < 0 ) {
				_mixer.SetInputWeight( _negY.Playable, Mathf.Abs( yWeight ) );
				_negY.SetGrowth( magnitude ); 
			} else {
				_mixer.SetInputWeight( _negY.Playable, 0 );
				_negY.SetGrowth( 0 ); 
			}
		}
	}


	// ****************** Private **********************

	private float _centerFadeinDistance;
	private ScriptPlayable<BlendTree> _playable;
	private Playable _mixer;
	private Animation _center;
	private Animation _posX;
	private Animation _negX;
	private Animation _posY;
	private Animation _negY;
	
	private void Init ( ScriptPlayable<BlendTree> thisPlayable, Setter setter ) {

		_centerFadeinDistance = setter.CenterFadeinDistance;
		_mixer = CreateMixer( thisPlayable, 5 );
		
		if ( setter.Center != null ) { _center = CreateAnimation( setter.Center, 0 ); }	
		if ( setter.PosX   != null ) { _posX = CreateAnimation( setter.PosX, 1 ); }
		if ( setter.NegX   != null ) { _negX = CreateAnimation( setter.NegX, 2 ); }
		if ( setter.PosY   != null ) { _posY = CreateAnimation( setter.PosY, 3 ); }
		if ( setter.NegY   != null ) { _negY = CreateAnimation( setter.NegY, 4 ); }
	}

	private AnimationMixerPlayable CreateMixer ( Playable parent, int numOfInputs ) {


		// create mixer
		var mixer = AnimationMixerPlayable.Create( parent.GetGraph() );
		mixer.SetInputCount( numOfInputs);
		mixer.SetOutputCount( 1 );


		// connect mixer
		parent.ConnectInput( 0, mixer, 0 );

		return mixer;
	}
	private Animation CreateAnimation ( Animation.Setter setter, int parentInputPort ) {
		
		return Animation.Create( _mixer, parentInputPort, setter );
	}


	// ****************** Data **********************
	
	[System.Serializable]
	public class Setter {

		public float StartingBlendPointX;
		public float StartingBlendPointY;
		public float CenterFadeinDistance;
		public Animation.Setter Center;
		public Animation.Setter PosX;
		public Animation.Setter NegX;
		public Animation.Setter PosY;
		public Animation.Setter NegY;
	}
}