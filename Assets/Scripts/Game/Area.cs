using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour {

	public Player LoadedPlayer{ get{ return _loadedPlayer; } }

	public void Init () {
		
		_loadedPlayer = LoadPlayer();
		LoadCamera();
		LoadEnemies();
	}
	public void EnterArea ( int doorID ) {

		PlacePlayer( doorID );
		LoadUIContext();
	}

	// *************************
	
	[SerializeField] private UIController.UiContext.Identifier _startingContext;
	[SerializeField] private string _areaName;
	[SerializeField] private List<Door> _doors;
	[SerializeField] private bool _hostile;
	[SerializeField] private Vector3 _size;

	[Header("Prefabs")]
	[SerializeField] private Player _playerPrefab;
	[SerializeField] private CameraSystem _cameraSystem;

	private Player _loadedPlayer;
	
	// ********* INIT ****************

	public Player LoadPlayer () {
		var player = Instantiate( _playerPrefab );		
		player.Init();
		return player;
	}
	private void LoadEnemies () {

		foreach ( Enemy e in FindObjectsOfType<Enemy>() ) {
			e.Init();
		}
	}
	private void LoadCamera(){

		_cameraSystem.Init ();
	}

	// ********* Enter ****************
	
	private void PlacePlayer ( int doorID ) {

		if ( doorID > _doors.Count-1 ) {
			Debug.LogWarning( "Trying to enter a door that doesnt exist. Creating player at last possible door" );
		}

		var id  = Mathf.Clamp( doorID, 0, _doors.Count-1);
		var door = _doors[ id ];
		door.PlacePlayer( _loadedPlayer );
	}
	private void LoadUIContext(){
		
		Game.UIController.ChangeContext( _startingContext );
	}

	// *************************

	private void OnDrawGizmos () {
	
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube( transform.position + new Vector3( 0, _size.y/2f, 0), _size );
	}

	// *************************

	public enum Identifier {
		Farm,
		Dungeon,
		Town,
		CraftingArea,
		SmallHome
	}
}
