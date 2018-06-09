using UnityEngine;
	
namespace Dumpster.Core.BuiltInModules.Rooms {

	public class Door : MonoBehaviour {
			
		public string Identifier {
			get{ return _identifier; } 
		}
		public bool IsDefualtSpawnLocation {
			get{  return _isDefualtSpawnLocation; }
		}


		[Header( "Door Properties" )]
		[SerializeField] private Dumpster.Triggers.BoxZone _trigger;
		[SerializeField] private Transform _spawnLocation;
		[SerializeField] private string _identifier;
		[SerializeField] private bool _isDefualtSpawnLocation;

		[Header( "Door Target" )]
		[SerializeField] private string _targetRoomIdentifier;
		[SerializeField] private string _targetDoorIdentifier;
		[SerializeField] private string _transitionIdentifier;

		private void Awake () {
			
			_trigger.OnTriggerZoneEnter += () => { EdensGarden.Instance.Rooms.ChangeArea( 
				_targetRoomIdentifier, 
				_targetDoorIdentifier, 
				_transitionIdentifier ); 
			};
		}
		public GameObject LoadPlayer( GameObject playerPrefab ) {

			var playerInstance = Instantiate( playerPrefab );
			playerInstance.transform.position = _spawnLocation.position;

			return playerInstance;
		}
	}
}