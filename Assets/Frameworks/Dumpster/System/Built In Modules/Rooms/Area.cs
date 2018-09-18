using System.Collections.Generic;
using UnityEngine;
	
namespace Dumpster.Core.BuiltInModules.Rooms {

	public class Area : MonoBehaviour {

		// **************** Public *******************

		public GameObject LoadedPlayer {
			get { return _playerInstance; }
		}
			
		public void ActivateArea ( string doorIdentider ) {

			LoadDoors ();
			LoadPlayer ( doorIdentider );
		}

		// **************** Private *******************
		
		private Dictionary<string,Door> _doors;
		private Door _defaultDoor;
		private GameObject _playerInstance;

		[SerializeField] private GameObject _playerPrefab;
		[SerializeField] private LightingPallete.Defaults _lighting;

		private void LoadDoors () {
			
			_doors = new Dictionary<string,Door>();
			var doors = FindObjectsOfType<Door>();

			foreach( Door d in doors )  {
				
				if ( !_doors.ContainsKey( d.Identifier ) ) {
					_doors.Add( d.Identifier, d );
				} else {
					Debug.LogWarning( "Mutiple doors with the identifier " + d.Identifier );
				}
			}

			foreach( Door d in doors )  {

				if ( d.IsDefualtSpawnLocation ) {
					if ( _defaultDoor == null ) {
						_defaultDoor = d;
					} else {
						Debug.LogWarning( "Mutiple default doors set" );
					}
				}
			}

			if ( _defaultDoor == null ) {
				Debug.LogWarning( "No default door set in area'" );
			}
		}
		private void LoadPlayer ( string doorIdentider ) {

			var door = _doors.ContainsKey( doorIdentider ) ? _doors[ doorIdentider ] : _defaultDoor;
			// _playerInstance = door.LoadPlayer( _playerPrefab );
		}
		private void OnDrawGizmos () {
			
			ApplyLighting ();
		}
		private void ApplyLighting () {

			var pallete = LightingPallete.FromDefault( _lighting );

			UnityEngine.RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
			UnityEngine.RenderSettings.ambientSkyColor = pallete.Sky;
			UnityEngine.RenderSettings.ambientEquatorColor = pallete.Equator;
			UnityEngine.RenderSettings.ambientGroundColor = pallete.Ground;
		}
		private struct LightingPallete {

			public Color Sky { get; }
			public Color Equator { get; }
			public Color Ground { get; }

			public static LightingPallete FromDefault ( Defaults pallete ) {
				
				switch ( pallete ){
					case Defaults.SunnyOutside: return SunnyOutside ();
					case Defaults.DarkInside: return DarkInside ();
				}

				return Empty();
			}

			public LightingPallete ( Color sky, Color equator, Color ground ) {
				
				Sky = sky;
				Equator = equator;
				Ground = ground;
			}


			// Lighting defualt definitions
			private static LightingPallete SunnyOutside () {
				return new LightingPallete( 
					new Color( 42f/255f, 113f/255f,255f/255f ),
					new Color( 255f/255f, 184f/255f, 150f/255f ),
					new Color( 0f/255f, 84f/255f, 81f/255f ) 
				); 
			}
			private static LightingPallete DarkInside () {
				return new LightingPallete( 
					new Color( 0/255f, 20/255f, 20/255f ),
					new Color( 0/255f, 80/255f, 80/255f ),
					new Color( 0f/255f, 200/255f, 200/255f ) 
				); 
			}
			private static LightingPallete Empty () {
				return new LightingPallete( 
					new Color( 0f ,0f ,0f ),
					new Color( 0f ,0f ,0f ),
					new Color( 0f ,0f ,0f )
				); 
			}

			public enum Defaults {
				SunnyOutside,
				DarkInside
			}
		}
	}

}