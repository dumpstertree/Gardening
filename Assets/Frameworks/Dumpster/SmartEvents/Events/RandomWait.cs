using UnityEngine;

namespace Dumpster.Events {
	
	public class RandomWait : SmartEvent {

		[Header( "Settings" )]
		[SerializeField] private float _minWait = 0f;
		[SerializeField] private float _maxWait = 1f;

		[Header( "Events" )]
		[SerializeField] private SmartEvent[] _onWaited;

	
		//***************** Public *******************
		
		public override void EventTriggered() {

			var rand = Random.Range( _minWait, _maxWait );
			EdensGarden.Instance.Async.WaitForSeconds( rand, FireOnWaited );
		}


		//***************** Private *******************
		
		private void FireOnWaited () {
		
			foreach ( SmartEvent e in _onWaited ) {
				e.EventTriggered();
			}
		}
	}
}