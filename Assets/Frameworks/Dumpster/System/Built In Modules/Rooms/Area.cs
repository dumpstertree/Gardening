using System.Collections.Generic;
using UnityEngine;
	
namespace Dumpster.Core.BuiltInModules.Rooms {

	public class Area : MonoBehaviour {

		public GameObject LoadedPlayer {
			get { return _playerInstance; }
		}
			
		public void ActivateArea ( string doorIdentider ) {

			LoadDoors ();
			LoadPlayer ( doorIdentider );
		}

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
		_playerInstance = door.LoadPlayer( _playerPrefab );
		}

		private Dictionary<string,Door> _doors;
		private Door _defaultDoor;
		private GameObject _playerInstance;

		[SerializeField] private GameObject _playerPrefab;
	}
}