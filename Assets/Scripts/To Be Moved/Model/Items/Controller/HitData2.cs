using UnityEngine;

namespace Controller.Item {

	public class HitData2 {
		
		private Slash _slashPrefab {
			get{ return Resources.Load<GameObject>( "Slash" ).GetComponent<Slash>(); }
		}

		private const float COOLDOWN_TIME = 1.0f;
		private bool _inCooldown = false;

		public void Hit ( Eden.Life.BlackBox user ) {

			if ( !_inCooldown ) {

				_inCooldown = true;
				
				var inst = GameObject.Instantiate( _slashPrefab );
				inst.transform.position = user.MeleeSpawner.position;
				inst.transform.rotation = user.MeleeSpawner.rotation;

				var hitData = new HitData();
				hitData.Power = 1;

				inst.Set( user, hitData );

				EdensGarden.Instance.Async.WaitForSeconds( COOLDOWN_TIME , ()=> { _inCooldown = false; } );
			}

		}
	}
}