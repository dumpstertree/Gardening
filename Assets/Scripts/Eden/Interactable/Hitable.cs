using UnityEngine;
using Eden.Life;
using Dumpster.Events;

namespace Eden.Interactable {
	
	public class Hitable : MonoBehaviour {

		public delegate void HitEvent ( BlackBox user, HitData data );
		public HitEvent OnHit;

		[SerializeField] private SmartEvent[] _onHit;

		public void Hit( BlackBox user, HitData data ){

			FireHitEvent( user, data );
		}
		private void FireHitEvent ( BlackBox user, HitData data ) {
			
			if ( OnHit != null ) {
				OnHit ( user, data );
			}

			foreach ( SmartEvent e in _onHit ){
				e.EventTriggered();
			}
		}
	}
}