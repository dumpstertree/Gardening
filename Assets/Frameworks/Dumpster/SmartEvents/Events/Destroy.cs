using UnityEngine;

namespace Dumpster.Events {

	public class Destroy : SmartEvent {

		[Header( "Settings")]
		[SerializeField] private GameObject _objectToDestroy;

		[Header( "Events" )]
		[SerializeField] private SmartEvent[] _onDestroyed;

	
		//***************** Public *******************
		
		public override void EventTriggered() {

			Destroy ( _objectToDestroy );
			FireOnDestroyed ();
		}

		private void FireOnDestroyed () {

			foreach ( SmartEvent e in _onDestroyed ) {
				e.EventTriggered();
			}
		}

	}
}