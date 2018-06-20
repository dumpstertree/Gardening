using UnityEngine;

namespace Eden.Interactable {
	
	public class Actionable : MonoBehaviour {

		public delegate void ActionEvent( Eden.Life.BlackBox user );
		public ActionEvent OnAction;
		
		public void Action( Eden.Life.BlackBox user ) {
			
			FireOnAction( user );
		}
		private void FireOnAction( Eden.Life.BlackBox user ) {

			if ( OnAction != null ) {
				OnAction( user );
			}
		}
	}
}