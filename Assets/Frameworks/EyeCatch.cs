using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dumpster.Core.BuiltInModules.Rooms {
	
	[CreateAssetMenu(menuName = "Dumpster/Transition/Eyecatch")]
	public class EyeCatch : Transition {

		// ************* Public *******************
		
		public override void PerformTransition ( Game game, Action onComplete ) {
			
			CreateVisualInstance( game );
			game.StartCoroutine( Transition( game, onComplete ) );
		}
		public override void DismissTransition () {

			Destroy( _visualInstance );
		}

		
		// ************* Private *******************
		
		[Header( "Animation Properties" )]
		[SerializeField] private int _fps = 30;
		[SerializeField] private float _waitOnComplete;
		
		[Header( "Eye Catch" )]
		[SerializeField] private AudioClip _audio;
		[SerializeField] private Sprite[] _frames;

		[Header( "Prefabs" )]
		[SerializeField] private GameObject _visualPrefab;
		
		private GameObject _visualInstance;
		private Image _image;

		private float _timeBetweenFrames {
			get{ return 1.0f/_fps; }
		}


		private void CreateVisualInstance ( Game game ) {

			_visualInstance = GameObject.Instantiate( _visualPrefab, game.transform );
			_image = _visualInstance.GetComponent<Image>();
		}
		private IEnumerator Transition ( Game game, Action onComplete ) {
			
			// play audio
			Game.GetModule<Audio>().PlayScreenAudio( _audio );

			// loop through transition sprites
			for ( int i =0; i<_frames.Length; i++ ) {
				_image.sprite = _frames[ i ];
				yield return new WaitForSecondsRealtime( _timeBetweenFrames );
			}

			// wait
			yield return new WaitForSecondsRealtime( _waitOnComplete );

			// run on complete
			if ( onComplete != null ) {
				onComplete ();
			}
		}
	}
}