using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Dumpster.Core {
	
public class Game : MonoBehaviour {

		public AudioSource AudioSource;

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
		
		[SerializeField] private ModuleInstallInstructions[] _modules;
		[SerializeField] private List<Module> _installedModules;

		[SerializeField] private Transform _dataSourceTransform;
		private IGameDataSource _dataSource { 
			get { 
				return _dataSourceTransform.GetComponent<IGameDataSource>(); 
			} 
		}

		public static Dumpster.Core.Data Data {
			get{ return _instance._data; }
		}


		public delegate void OnInitEvent();
		public OnInitEvent OnInit;

		public delegate void OnUpdateEvent();
		public OnUpdateEvent OnUpdate;

		public delegate void OnSceneChangedEvent();
		public OnSceneChangedEvent OnSceneChanged;

		public delegate void OnFixedUpdateEvent();
		public OnFixedUpdateEvent OnFixedUpdate;

		protected static Game _instance;


		private Data _data;


		private void Awake () {
			
			if ( _instance == null ){ 
				
				_installedModules = new List<Module>();
				_data = new Dumpster.Core.Data ();

				AudioSource = gameObject.AddComponent<AudioSource>();
				
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

			foreach( Module m in _dataSource.Modules() ) {
				m.Install( this );
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
		private void FireOnSceneChangedEvent () {

			if ( OnSceneChanged != null ) {
				OnSceneChanged ();
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
	

	public interface IGameDataSource {
	
		Module[] Modules ();
	}
}