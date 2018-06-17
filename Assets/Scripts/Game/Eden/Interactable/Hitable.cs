using UnityEngine;
using Eden.Life.Brain;

namespace Eden.Interactable {
	
	public class Hitable : MonoBehaviour {

		public delegate void HitEvent ( BlackBoxBrain user, HitData data );
		public HitEvent OnHit;

		public void Hit( BlackBoxBrain user, HitData data ){

			FireHitEvent( user, data );
		}
		private void FireHitEvent ( BlackBoxBrain user, HitData data ) {
			
			if ( OnHit != null ) {
				OnHit ( user, data );
			}
		}
	}
}