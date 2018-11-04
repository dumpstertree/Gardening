using UnityEngine;

namespace Dumpster.Events {

	public class LimitedPass : SmartEvent {

		[Header( "Settings")]
		[SerializeField] private int _numOfTimesCanPass = 1;

		[Header( "Events" )]
		[SerializeField] private SmartEvent[] _onPass;

		private int _numOfPasses;

	
		//***************** Public *******************
		
		public override void EventTriggered() {

			if ( _numOfPasses < _numOfTimesCanPass ) {
				_numOfPasses++;
				FireOnPass();
			}
		}

		private void FireOnPass () {

			foreach ( SmartEvent e in _onPass ) {
				e.EventTriggered();
			}
		}	
	}
}