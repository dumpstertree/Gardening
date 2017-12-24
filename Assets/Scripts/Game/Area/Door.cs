using UnityEngine;

public class Door : MonoBehaviour {

	// *********** PUBLIC **************
	
	public void ChangeArea () {
		
		Game.AreaController.ChangeArea( TargetID, TargetDoorID );
	}
	public void PlacePlayer ( Player p ) {

		p.transform.position = new Vector3( _spawnLocation.position.x, GetYPos(), _spawnLocation.position.z );
		p.transform.rotation = _spawnLocation.rotation;
	}

	// ********** PRIVATE **************

	[Header("Target")]
	[SerializeField] private Area.Identifier TargetID;
	[SerializeField] private int TargetDoorID;
	[Header("Spawn")]
	[SerializeField] private Transform _spawnLocation;
	[SerializeField] private int _orbsPerMeter;
	[SerializeField] private Vector3 Size;
	[Header("Prefab")]
	[SerializeField] private GameObject _orbPrefab;
	
	private void Awake () {

		CreateVisual();
		GetComponent<BoxCollider>().size = Size;
	}
	private void OnDrawGizmos () {
		
		GetComponent<BoxCollider>().size = Size;
		
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere( _spawnLocation.transform.position, 1.0f );

		Gizmos.matrix = transform.localToWorldMatrix;;
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube( Vector3.zero, Size );
	}
	private void OnTriggerEnter ( Collider collider ) {
		
		if ( collider.GetComponent<Player>() != null ){
			ChangeArea();
		}
	}
	private void CreateVisual () {

		var orbsCount = Size.x * _orbsPerMeter;
		var distance = Size.x / (orbsCount-1);

		for( int i=0; i<orbsCount; i++ ){
		
			var x = -(Size.x/2) + (distance * i);
			var y = -(Size.y/2) + 0.25f;
			var z = Size.z/2;
			var orb = Instantiate( _orbPrefab );

			orb.transform.SetParent( transform, false );
			orb.transform.localPosition = new Vector3(  x, y, z ); 
		}
	}
	private float GetYPos () {

		var ray = new Ray( _spawnLocation.position, Vector3.down);
		RaycastHit hit;

	    if ( Physics.Raycast( ray, out hit, 10.0f) ) {
	    	return hit.point.y;
		}

		return _spawnLocation.position.y;
	}
}
