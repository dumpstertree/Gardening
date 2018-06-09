using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TransitionCreator : MonoBehaviour{

	[SerializeField] private float _fps;
	[SerializeField] private Sprite[] _transitionFrames;
	[SerializeField] private Image _image;
	[SerializeField] private AudioClip _transitionInAudio;
	[SerializeField] private AudioSource _audioSource;
	[SerializeField] private Sprite _niaEndFrame;

	private void Start () {

		EdensGarden.Instance.Rooms.RegisterTransition( "NIA", a => { NiaTransitionOut( a ); }, a => { NiaTransitionIn( a ); } );
	}
	private void NiaTransitionIn( System.Action onComplete ) {

		StartCoroutine( TransitionIn( _transitionFrames, _transitionInAudio, _niaEndFrame, onComplete ) );
	}
	private void NiaTransitionOut( System.Action onComplete ) {

		StartCoroutine( TransitionOut( _transitionFrames, _transitionInAudio, _niaEndFrame, onComplete ) );
	}
	private IEnumerator TransitionIn( Sprite[] transitionSprites, AudioClip audioClip, Sprite endFrame, System.Action oncomplete ) {

		// play audio
		_audioSource.PlayOneShot( audioClip );

		// loop through transition sprites
		var timeBetweenFrames = 1.0f/_fps;
		for ( int i =0; i<transitionSprites.Length; i++ ) {
			
			_image.sprite = transitionSprites[ (transitionSprites.Length-1) - i ];
			yield return new WaitForSecondsRealtime( timeBetweenFrames );
		}

		// turn object off
		_image.gameObject.SetActive( false );

		// run on complete
		if ( oncomplete != null ) {
			oncomplete ();
		}
	}
	private IEnumerator TransitionOut( Sprite[] transitionSprites, AudioClip audioClip, Sprite endFrame, System.Action oncomplete ) {

		// play audio
		_audioSource.PlayOneShot( audioClip );

		// turn on image
		_image.gameObject.SetActive( true );

		// loop through transition sprites
		var timeBetweenFrames = 1.0f/_fps;
		for ( int i =0; i<transitionSprites.Length; i++ ) {
			
			_image.sprite = transitionSprites[ i ];
			yield return new WaitForSecondsRealtime( timeBetweenFrames );
		}

		// set end sprite
		_image.sprite = endFrame;
		yield return new WaitForSecondsRealtime( 3f );

		// run on complete
		if ( oncomplete != null ) {
			oncomplete ();
		}
	}
}
