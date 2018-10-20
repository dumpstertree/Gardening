using UnityEngine;

namespace Eden.UI.Animators {
	
	public abstract class UIAnimator : MonoBehaviour {

		public void SetState ( string state ) {

			if ( state != _currentState ) {

				OnExitState( _currentState );
				OnEnterState( state );

				_currentState = state;
			}
		}

		protected virtual void OnExitState ( string state ) {}
		protected virtual void OnEnterState ( string state ) {}

		private string _currentState;
	}
}