using UnityEngine;

public class GunParts : MonoBehaviour {

	[SerializeField] private GunPartsRow _gunPartRowPrefab;
	[SerializeField] private Transform _content;

	public delegate void DragEvent( CraftedGun.Component part );
	public DragEvent OnDragBegin;
	public DragEvent OnDragEnd;

	private Player _player{
		get{ return Game.Area.LoadedPlayer; }
	}

	// *************************
	
	public void Reload () {
		
		Clear();

		foreach  ( CraftedGun.Component part in _player.GunParts ) {
			
			AddRow( part );
		}
	}
	
	// *************************

	private void AddRow ( CraftedGun.Component part ) {
		
		var row = Instantiate( _gunPartRowPrefab );
		row.transform.SetParent( _content, false );

		row.SetPart( part );

		row.OnDragBegin += HandleOnDragBegin;
		row.OnDragEnd += HandleOnDragEnd;
	}
	private void Clear () {
		
		foreach( Transform t in _content ) {
			if ( t == transform ){ continue; }
			Destroy( t.gameObject );
		}
	}

	// *************************

	private void HandleOnDragBegin ( CraftedGun.Component part ) {
    	
    	if ( OnDragBegin != null ) {
    		OnDragBegin( part );
    	}
    }
    private void HandleOnDragEnd ( CraftedGun.Component part ) {
    	
    	if ( OnDragEnd != null ) {
    		OnDragEnd( part );
    	}
    }
}
