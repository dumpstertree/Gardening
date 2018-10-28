using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Dumpster.Core.BuiltInModules.Rooms;

namespace Dumpster.BuiltInModules {

	[CreateAssetMenu(menuName = "Dumpster/Modules/Rooms")]
	public class Navigation : Dumpster.Core.Module {
		
		
		// ************** Public *****************

		public Area CurrentArea {
			get{ return _area; }
		}
		
		public void ChangeArea( string areaIdentifer, string doorIdentifer, string transitionTag ) {

			// save door identifier for reloading
			_doorIdentifier = doorIdentifer;
			
			// change the scene
			SceneManager.LoadScene( areaIdentifer );

			// set the timescake to frozen
			Time.timeScale = 0;

			// begin fadeout of scene
			TransitionOut( transitionTag, () => {
				
				// fade in the new scene
				TransitionIn( transitionTag, () => {
					
					// dismiss the transition in
					DismissTransitionOut( transitionTag );
					
					// dismiss out transition
					DismissTransitionIn( transitionTag );

					// after being faded in reset the timescale
					Time.timeScale = 1;
				});
			});
		}


		// ************** Protected *****************

		protected override void OnInit () {

			_doorIdentifier = "";
		}
		protected override void OnReload () {
			
			LoadNewArea( _doorIdentifier );
		}
	
		// ************** Private *****************

		[SerializeField] private TransitionType[] _transitions;

		private Area _area;
		private string _doorIdentifier;

		private void TransitionOut( string transitionTag, Action onComplete ) {
			
			var transition = GetTransitionType( transitionTag );
			if ( transition != null && transition.TransitionOut != null ) {
				
				transition.TransitionOut.PerformTransition( _game, onComplete );
			
			}  else {

				onComplete ();
			}
		}
		private void TransitionIn ( string transitionTag, Action onComplete ) {
			
			var transition = GetTransitionType( transitionTag );
			if ( transition != null && transition.TransitionIn != null ) {
				
				transition.TransitionIn.PerformTransition( _game, onComplete );
			
			}  else {

				onComplete ();
			}
		}
		private void DismissTransitionOut( string transitionTag ) {
			
			var transition = GetTransitionType( transitionTag );
			if ( transition != null && transition.TransitionOut != null ) {
				transition.TransitionOut.DismissTransition();
			}
		}
		private void DismissTransitionIn( string transitionTag ) {
			
			var transition = GetTransitionType( transitionTag );
			if ( transition != null && transition.TransitionIn != null ) {
				transition.TransitionIn.DismissTransition();
			}
		}
		private void LoadNewArea ( string doorIdentifier ) {

			_area = FindObjectOfType<Area>();
			
			if ( _area!=  null ) {
				_area.Activate( doorIdentifier );
			} else {
				Debug.LogWarning( "No area in scene" );
			}
		}
		
		private TransitionType GetTransitionType ( string transitionTag ) {

			foreach ( TransitionType t in _transitions ) {
				if ( t.Name == transitionTag ) {
					return t;
				}
			}

			return null;
		}



		// ************** Data Types *****************

		[System.Serializable]
		private class TransitionType {
			
			[SerializeField] public string Name;
			[SerializeField] public Transition TransitionIn;
			[SerializeField] public Transition TransitionOut;
		}
	}
}