using UnityEngine;
using Eden.Model.Interactable;
using Dumpster.Events;

namespace Eden.Interactable {
	
	public class Hitable : MonoBehaviour {

		public delegate void HitEvent ( Hit data );
		public HitEvent OnHit;

		[SerializeField] private SmartEvent[] _onHit;

		public void Hit( Hit data ){

			FireHitEvent( data );
		}
		private void FireHitEvent ( Hit data ) {
			
			if ( OnHit != null ) {
				OnHit ( data );
			}

			foreach ( SmartEvent e in _onHit ){
				e.EventTriggered();
			}
		}
	}
}