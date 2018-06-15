using UnityEngine;

namespace Dumpster.Core.BuiltInModules.Effects {

	public class Controller : Module {

		private GameObject _smoke {
			get{ return Resources.Load( "SmokeEffect" ) as GameObject; }
		}
		private GameObject _fireworks {
			get{ return Resources.Load( "FireworksEffect" ) as GameObject; }
		}
		private GameObject _hit {
			get{ return Resources.Load( "HitEffect" ) as GameObject; }
		}
		private GameObject _faint {
			get{ return Resources.Load( "FaintEffect" ) as GameObject; }
		}
		private GameObject _wakeUp {
			get{ return Resources.Load( "WakeUpEffect" ) as GameObject; }
		}

		public void OneShot( ParticleType type, Vector3 position, Quaternion rotation ) {

			GameObject prefab = null;

			switch( type ) {
				
				case ParticleType.None:
					return;

				case ParticleType.Fireworks:
					prefab =_fireworks;
					break;

				case ParticleType.Hit:
					prefab = _hit;
					break;

				case ParticleType.Smoke:
					prefab = _smoke;
					break;

				case ParticleType.Faint:
					prefab = _faint;
					break;

				case ParticleType.WakeUp:
					prefab = _wakeUp;
					break;
			}

			if ( prefab != null ) {
				
				var go = Instantiate( prefab );
				go.transform.position = position;
				go.transform.rotation = rotation;
			}
		}
	}

	public enum ParticleType {
		None,
		Fireworks,
		Hit,
		Smoke,
		Faint,
		WakeUp
	}
}