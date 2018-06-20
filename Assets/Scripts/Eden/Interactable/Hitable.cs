using UnityEngine;
using Eden.Life;

namespace Eden.Interactable {
	
	public class Hitable : MonoBehaviour {

		public delegate void HitEvent ( BlackBox user, HitData data );
		public HitEvent OnHit;

		public void Hit( BlackBox user, HitData data ){

			FireHitEvent( user, data );
		}
		private void FireHitEvent ( BlackBox user, HitData data ) {
			
			if ( OnHit != null ) {
				OnHit ( user, data );
			}
		}
	}
}