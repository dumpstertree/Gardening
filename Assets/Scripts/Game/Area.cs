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
		ApplyLighting();
	}

	// *************************
	
	[SerializeField] private UIController.UiContext.Identifier _startingContext;
	[SerializeField] private string _areaName;
	[SerializeField] private List<Door> _doors;
	[SerializeField] private bool _hostile;
	[SerializeField] private Vector3 _size;
	[SerializeField] private LightingPallete.Defaults _lighting;

	[Header("Prefabs")]
	[SerializeField] private Player _playerPrefab;

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

		//_cameraSystem.Init ();
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
	private void ApplyLighting () {

		var pallete = LightingPallete.FromDefault( _lighting );

		UnityEngine.RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
		UnityEngine.RenderSettings.ambientSkyColor = pallete.Sky;
		UnityEngine.RenderSettings.ambientEquatorColor = pallete.Equator;
		UnityEngine.RenderSettings.ambientGroundColor = pallete.Ground;
	}

	// *************************

	private void OnDrawGizmos () {
	
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube( transform.position + new Vector3( 0, _size.y/2f, 0), _size );

		ApplyLighting();
	}

	// *************************

	public enum Identifier {
		Farm,
		Dungeon,
		Town,
		CraftingArea,
		SmallHome,
		Home,
		Housing,
		GardeningShop,
		GunShop,
		GeneralStore
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
