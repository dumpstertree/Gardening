using UnityEngine;
using Dumpster.Events;

namespace Eden.Interactable {
	
	public class Actionable : MonoBehaviour {

		// ****************** Public ********************

		public delegate void ActionEvent( Eden.Life.BlackBox user );
		public ActionEvent OnAction;
		
		public void Action( Eden.Life.BlackBox user ) {
			
			FireOnAction( user );
		}

		// ****************** Private ********************

		[SerializeField] private SmartEvent[] _onAction;

		private void FireOnAction( Eden.Life.BlackBox user ) {

			if ( OnAction != null ) {
				OnAction( user );
			}

			foreach ( SmartEvent e in _onAction ){
				e.EventTriggered();
			}
		}
	}
}