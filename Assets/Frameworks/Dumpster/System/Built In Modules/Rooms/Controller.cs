using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dumpster.Core.BuiltInModules.Rooms {

	public class Controller : Dumpster.Core.Module {

		// module
		protected override void OnInit () {

			_transitionOut = new Dictionary<string,Action<Action>>();
			_transitionIn = new Dictionary<string,Action<Action>>();
		}
		protected override void OnRun () {
			
			LoadNewArea ( "" );
		}

		// public
		public delegate void OnChangeAreaEvent ();
		public OnChangeAreaEvent OnChangeArea;

		public Area CurrentArea {
			get{ return _area; }
		}
	
		public void ChangeArea( string areaIdentifer, string doorIdentifer, string transitionTag ) {

			// set the timescake to frozen
			Time.timeScale = 0;

			// begin fadeout of scene
			TransitionOut( transitionTag, () => {
				
				try {

					// load the new scene
					SceneManager.LoadScene( areaIdentifer );
					WaitForAreaToLoad( areaIdentifer, () => { 
							
							// once the new area is loaded tell anyone listening
							LoadNewArea( doorIdentifer );
							FireOnChangeArea ();

							// fade in the new scene
							TransitionIn( transitionTag, () => {

								// after being faded in reset the timescale
								Time.timeScale = 1;
							}
						);
					});
				} catch {
					
					Debug.LogWarning( "Trying to load a scene for " + areaIdentifer + "  that doesnt exist!" );
					
					// if there was an error fade the screen back in and reset the timescale
					TransitionIn( transitionTag, () => { Time.timeScale = 1; });
				}
			});
		}
		public void RegisterTransition( string transitionTag, Action<Action> transitionOut, Action<Action> transitionIn ) {

			if ( !_transitionOut.ContainsKey( transitionTag )) {
				_transitionOut.Add( transitionTag, transitionOut );
				_transitionIn.Add( transitionTag, transitionIn );
			} else {
				Debug.LogWarning( "Transition with tag " + transitionTag + " already exists" );
			}
		}


		// private 
		private void TransitionOut( string transitionTag, Action onComplete ) {
			
			if ( _transitionOut.ContainsKey( transitionTag ) ) {
				
				var t = _transitionOut[ transitionTag ];
				if ( t != null ) {
					t( onComplete );
					return;
				}
			}

			if ( onComplete != null ) {
				onComplete ();
			}
		}
		private void TransitionIn ( string transitionTag, Action onComplete ) {
			
			if ( _transitionIn.ContainsKey( transitionTag ) ) {
				
				var t = _transitionIn[ transitionTag ];
				if ( t != null ) {
					t( onComplete );
					return;
				}
			}

			if ( onComplete != null ) {
				onComplete ();
			}
		}
		public void WaitForAreaToLoad( string areaName, Action onComplete ){

			StartCoroutine( WaitForAreaToLoadCoroutine( areaName, onComplete) );
		}
		private void LoadNewArea ( string doorIdentifier ) {

			_area = FindObjectOfType<Area>();
			
			if ( _area!=  null ) {
				_area.ActivateArea( doorIdentifier );
			} else {
				Debug.LogWarning( "No area in scene" );
			}
		}

		private Area _area;
		private Dictionary<string,Action<Action>> _transitionOut;
		private Dictionary<string,Action<Action>> _transitionIn;


		// fire event
		private void FireOnChangeArea () {
			
			if (OnChangeArea != null) {
				OnChangeArea ();
			}
		}


		// coroutines
		private IEnumerator WaitForAreaToLoadCoroutine( string areaName, Action onComplete ) {
       		yield return SceneManager.LoadSceneAsync( areaName );
			onComplete();
		}
		private IEnumerator FadeOutCoroutine ( Action onComplete ) {

			yield return null;
			// for( float i = 0; i<TRANSITION_DURATION; i+= Time.fixedDeltaTime ){
			// 	var frac = i/TRANSITION_DURATION;
			// 	_screenWipeInstance.material.SetFloat( PROPERTY_NAME, frac );
			// 	yield return null;
			// }
			// _screenWipeInstance.material.SetFloat( PROPERTY_NAME, 1f );

			if( onComplete != null ) {
				onComplete ();
			}
		}
		private IEnumerator FadeInCoroutine ( Action onComplete ){

			yield return null;
			// for( float i = 0; i<TRANSITION_DURATION; i+= Time.fixedDeltaTime ){
			// 	var frac = 1 - i/TRANSITION_DURATION;
			// 	_screenWipeInstance.material.SetFloat( PROPERTY_NAME, frac );
			// 	yield return null;
			// }

			// _screenWipeInstance.material.SetFloat( PROPERTY_NAME, 0f );
			if( onComplete != null ) {
				onComplete ();
			}
		}
	}
}