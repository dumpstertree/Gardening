using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Dumpster.Core {
	
public class Game : MonoBehaviour {

	// ****************** Public ********************

		public static Game Instance {
			get{
				if ( _instance == null ) {
					Debug.LogError( "No 'Game' is currently in use!" );
				} 
				return _instance; 
			}
		}
		public static T GetModule<T>() where T : class {

			if ( _instance == null ) {
				Debug.LogError( "No 'Game' is currently in use!" );
			}

			foreach ( Module m in _instance._installedModules ) {
				if ( m.GetType() == typeof( T ) ) {
					return m as T;
				}
			}

			Debug.LogWarning( "Game does not have a module installed of type '" + typeof( T ).ToString() + "'!" );
			return null;
		}
	

		public delegate void OnInitEvent();
		public OnInitEvent OnInit;

		public delegate void OnUpdateEvent();
		public OnUpdateEvent OnUpdate;

		public delegate void OnSceneChangedEvent();
		public OnSceneChangedEvent OnSceneChanged;

		public delegate void OnFixedUpdateEvent();
		public OnFixedUpdateEvent OnFixedUpdate;

		public delegate void OnLateUpdateEvent();
		public OnLateUpdateEvent OnLateUpdate;

		public delegate void OnDrawGizmosEvent();
		public OnDrawGizmosEvent OnDrawGameGizmos;

		// ****************** Private ********************

		[SerializeField] private ModuleInstallInstructions[] _modules;


		private static Game _instance;
		private List<Module> _installedModules;


		private void Awake () {
			
			if ( _instance == null ){ 
				
				_installedModules = new List<Module>();
				
				SetInstance( this );
				
				ListenForUnityEvents ();

				InstallModules ();
				FireInitEvent ();

			} else { 
			
				Destroy( gameObject ); 
			}
		}
		private void Update () {

			FireOnUpdate ();
		}
		private void FixedUpdate () {
			
			FireOnFixedUpdate ();
		}
		private void LateUpdate () {
			
			FireOnLateUpdate ();
		}
		private void OnDrawGizmos () {

			FireOnDrawGizmosEvent ();
		}
		
		private void SetInstance ( Game newInstance ) {
			
			_instance = newInstance;
			DontDestroyOnLoad( _instance.gameObject );
		}
		private void ListenForUnityEvents () {

			SceneManager.activeSceneChanged += ( scene, mode ) => { 
				FireOnSceneChangedEvent(); 
			};
		}

		private void InstallModules () {

			foreach( ModuleInstallInstructions instructions in _modules ) {
				
				var module = instructions.Module;
				
				if ( instructions.Instantiate ) {
					module = ScriptableObject.Instantiate( module );
				}
				
				module.Install( this );
				_installedModules.Add( module );
			}
		}
		private void FireInitEvent () {
			
			if ( OnInit != null ) {
				OnInit ();
			}
		}
		private void FireOnUpdate () {

			if ( OnUpdate != null ) {
				OnUpdate ();
			}
		}
		private void FireOnFixedUpdate () {

			if ( OnFixedUpdate != null ) {
				OnFixedUpdate ();
			}
		}
		private void FireOnLateUpdate () {

			if ( OnLateUpdate != null ) {
				OnLateUpdate ();
			}
		}
		private void FireOnSceneChangedEvent () {

			if ( OnSceneChanged != null ) {
				OnSceneChanged ();
			}
		}
		private void FireOnDrawGizmosEvent () {

			if( OnDrawGameGizmos != null ) {
				OnDrawGameGizmos ();
			}
		}

		[System.Serializable]
		private class ModuleInstallInstructions {
			
			public Module Module {
				get { return _module; }
			}
			public bool Instantiate {
				get{ return _instantiate; }
			}
			
			[SerializeField] private Module _module;
			[SerializeField] private bool _instantiate = true;
		}
	}
}