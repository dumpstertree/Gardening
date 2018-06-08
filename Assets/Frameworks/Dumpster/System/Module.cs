using UnityEngine;

namespace Dumpster.Core {

	public abstract class Module : MonoBehaviour {

		public static Module Install ( Game game, System.Type type ) {

			var m = game.gameObject.AddComponent( type ) as Module;
			
			if ( m != null ) {

				m.OnInstall ();
			} else {

				Debug.LogWarning( "Trying to install " + type.ToString() + " which is not a Module!" );
				return null;
			}

			return m;
		}
		public void Init () {

			OnInit();
		}
		public void Run () {

			OnRun ();
		}

		protected virtual void OnInstall () {}
		protected virtual void OnInit () {}
		protected virtual void OnRun () {}
	}
}