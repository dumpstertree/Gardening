using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ObjectAnimator : MonoBehaviour {


	public void Animate ( string animationName ) {

		if ( _animations.ContainsKey( animationName ) ) {
			_animations[ animationName ]();
		} else {
			Debug.LogWarning( "Trying to play the animation named " + animationName + "which does not exist!", gameObject );
		}
	}

	// ******************************

	[SerializeField] CanvasGroup _canvasGroup;
	[SerializeField] Image _outline;

	protected Dictionary< string,Action> _animations;
	
	private void Awake () {
		SetStates();
	}
	private void SetStates () {

		_animations = new Dictionary<string,Action>();
		
		_animations.Add( "SET", SetSetVisual );
		_animations.Add( "UNSET_VALID", SetUnsetValidVisual );
		_animations.Add( "UNSET_INVALID", SetUnsetInvalidVisual );
		_animations.Add( "INACTIVE", SetInactiveVisual );
		_animations.Add( "ACTIVE", SetActiveVisual );
	}

	private void SetSetVisual () {

		_canvasGroup.alpha = 1.0f;
	}
	private void SetUnsetValidVisual () {

		_canvasGroup.alpha = 0.8f;
	}
	private void SetUnsetInvalidVisual () {

		_canvasGroup.alpha = 0.2f;
	}
	private void SetInactiveVisual () {
		
		_outline.gameObject.SetActive( false );
	}
	private void SetActiveVisual () {

		_outline.gameObject.SetActive( true );
	}
}
