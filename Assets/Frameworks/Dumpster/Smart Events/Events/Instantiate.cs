using UnityEngine;

namespace Dumpster.Events {

	public class Instantiate : SmartEvent {

		[Header( "Settings")]
		[SerializeField] private GameObject _objectToInstantiate;
		[SerializeField] private Transform _instantiateAt;

		[Header( "Events" )]
		[SerializeField] private SmartEvent[] _onInstantiate;

	
		//***************** Public *******************
		
		public override void EventTriggered() {

			var inst = Instantiate( _objectToInstantiate );
			inst.transform.position = _instantiateAt.transform.position;
			inst.transform.rotation = _instantiateAt.transform.rotation;

			FireOnInstantiate ();
		}

		private void FireOnInstantiate () {

			foreach ( SmartEvent e in _onInstantiate ) {
				e.EventTriggered();
			}
		}

	}
}