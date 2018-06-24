using UnityEngine;
using Dumpster.Events;
using Eden.Interactable;

namespace Eden.Events {
	
	public class AddHealth : SmartEvent {

		[SerializeField] private SmartEvent[] _onAddHealth;
		[SerializeField] private int _health;
		[SerializeField] private Stats _stats;

		public int Health {
			get{ return _health; }
			set { _health = value; }
		}
		public Stats Stats {
			get{ return _stats; }
			set { _stats = value; }
		}

		public override void EventTriggered () {
			
			if ( _stats != null ) { _stats.AddHealth ( _health ); }

			FireAddHealthEvent ();
		}
		private void FireAddHealthEvent () {
			
			foreach ( SmartEvent e in _onAddHealth ) {
				e.EventTriggered();
			}
		}
	}
}