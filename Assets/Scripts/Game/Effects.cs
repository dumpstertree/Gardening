using UnityEngine;

namespace Application {
	
	public class Effects : MonoBehaviour {

		private GameObject _smoke {
			get{ return Resources.Load( "SmokeEffect" ) as GameObject; }
		}
		private GameObject _fireworks {
			get{ return Resources.Load( "FireworksEffect" ) as GameObject; }
		}
		private GameObject _hit {
			get{ return Resources.Load( "HitEffect" ) as GameObject; }
		}

		public void OneShot( Type type, Vector3 position, Quaternion rotation ) {

			GameObject prefab = null;

			switch( type ) {
				
				case Type.None:
					return;

				case Type.Fireworks:
					prefab =_fireworks;
					break;

				case Type.Hit:
					prefab = _hit;
					break;

				case Type.Smoke:
					prefab = _smoke;
					break;
			}

			var go = Instantiate( prefab );
			go.transform.position = position;
			go.transform.rotation = rotation;
		}

		public enum Type {
			None,
			Fireworks,
			Hit,
			Smoke
		}
	}
}