using Dumpster.Core;
using Dumpster.BuiltInModules;
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
			
			if ( _trigger != null ) {
			
				_trigger.OnTriggerZoneEnter += () => { Game.GetModule<Navigation>()?.ChangeArea( 
					_targetRoomIdentifier, 
					_targetDoorIdentifier, 
					_transitionIdentifier ); 
				};
			}
		}
		public GameObject LoadPlayer( GameObject playerPrefab ) {

			var playerInstance = Instantiate( playerPrefab );
			playerInstance.transform.position =  new Vector3(_spawnLocation.position.x, GetYPos(), _spawnLocation.position.z);
			playerInstance.transform.rotation = _spawnLocation.rotation;

			return playerInstance;
		}
		private float GetYPos () {

			var ray = new Ray( _spawnLocation.position, Vector3.down);
			RaycastHit hit;

		    if ( UnityEngine.Physics.Raycast( ray, out hit, 10.0f) ) { return hit.point.y; }
			return _spawnLocation.position.y;
		}
	}
}