using UnityEngine;

namespace Dumpster.Core {

	public abstract class Module : ScriptableObject {

		// ************* Public *******************
		public void Install ( Game game ) {
			
			_game = game;
			
			game.OnInit += Init;
			game.OnSceneChanged += Reload;
			game.OnUpdate += Update;
			game.OnFixedUpdate += FixedUpdate;
			game.OnLateUpdate += LateUpdate;
			game.OnDrawGameGizmos += OnDrawGizmos;
		}

		
		// ************ Private *******************

		private void Init () {
		
			_hasBeenInitialized = true;
			OnInit();
		}
		private void Reload () {

			OnReload ();
		}
		private void Update () {

			OnUpdate ();
		}
		private void FixedUpdate () {

			OnFixedUpdate ();
		}
		private void LateUpdate () {

			OnLateUpdate ();
		}
		private void DrawGizmos () {

			OnDrawGizmos ();
		}


		// ************ Protected *******************

		protected Game _game {
			private set; get;
		}

		protected bool _hasBeenInitialized;
		protected bool _hasRun;

		protected virtual void OnInit () {}
		protected virtual void OnReload () {}
		protected virtual void OnUpdate () {}
		protected virtual void OnFixedUpdate () {}
		protected virtual void OnLateUpdate () {}
		protected virtual void OnDrawGizmos () {}
	}
}