using UnityEngine;

namespace Dumpster.Core {

	public abstract class Module : MonoBehaviour {

		public static Module Install ( Game game, System.Type type ) {

			var m = game.gameObject.AddComponent( type ) as Module;
			
			if ( m != null ) {
				m._game = game;
				m.OnInstall ();
				game.OnSceneChanged += m.OnSceneChange;
			} else {

				Debug.LogWarning( "Trying to install " + type.ToString() + " which is not a Module!" );
				return null;
			}

			return m;
		}
		public void Init () {

			_hasBeenInitialized = true;
			OnInit();
		}
		public void Run () {

			OnRun ();
		}
		private void OnSceneChange () {

			OnReload();
		}

		protected bool _hasBeenInitialized;
		protected Dumpster.Core.Game _game;

		protected virtual void OnInstall () {}
		protected virtual void OnInit () {}
		protected virtual void OnRun () {}
		protected virtual void OnReload () {}
	}
}