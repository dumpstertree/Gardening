using UnityEngine;
using Dumpster.Events;

namespace Eden.Interactable {
	
	public class Hitable : MonoBehaviour {

		public delegate void HitEvent ( HitData data );
		public HitEvent OnHit;

		[SerializeField] private SmartEvent[] _onHit;

		public void Hit(  HitData data ){

			FireHitEvent( data );
		}
		private void FireHitEvent ( HitData data ) {
			
			if ( OnHit != null ) {
				OnHit ( data );
			}

			foreach ( SmartEvent e in _onHit ){
				e.EventTriggered();
			}
		}
	}
}