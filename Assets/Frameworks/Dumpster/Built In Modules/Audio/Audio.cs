using Dumpster.Core;
using UnityEngine;

namespace Dumpster.Core.BuiltInModules {
	
	[CreateAssetMenu( menuName = "Dumpster/Modules/Audio" )]
	public class Audio : Module  {


		// ****************** Public *********************

		public void PlayWorldAudio ( AudioClip clip, Vector3 pos, float volume = 1.0f ) {

			var source = CreateAudioSource( pos );

			source.clip = clip;
			source.volume = volume;
			source.Play();

			Destroy( source.gameObject, clip.length );
		}
		public void PlayScreenAudio ( AudioClip clip, float volume = 1.0f ) {

			_screenAudioSource.clip = clip;
			_screenAudioSource.volume = volume;
			_screenAudioSource.Play();
		}

		
		// ****************** Protected *********************
		
		protected override void OnInit () {
			
			_screenAudioSource = CreateAudioSource( Vector3.zero );
		}

	
		// ****************** Private *********************

		private AudioSource _screenAudioSource;
		
		private AudioSource CreateAudioSource ( Vector3 pos ) {

			var go = new GameObject( "Audio Source" );
			
			go.transform.SetParent( _game.transform, false );
			go.transform.position = pos;

			return go.AddComponent<AudioSource>();
		}
	}
}