using UnityEngine;

namespace Dumpster.Core {
	
	public abstract class Game : MonoBehaviour {
		
		protected static Game _instance;

		private void Awake () {

			if ( Game._instance == null ){ 
				SetInstance( this );
			} else { 
				Destroy( gameObject ); 
			}
		}
		
		private void SetInstance ( Game newInstance ) {
			
			_instance = newInstance;

			DontDestroyOnLoad( _instance.gameObject );

			BuildGame ();
			InitGame ();
			PlayGame ();
		}

		protected abstract void BuildGame ();
		protected abstract void InitGame ();
		protected abstract void PlayGame ();

		public delegate void OnSceneChangedEvent ();
		public OnSceneChangedEvent OnSceneChanged;

		protected void FireOnSceneChangedEvent () {
			
			if ( OnSceneChanged != null ) {
				OnSceneChanged ();
			}
		}
	}
}