using System.Collections.Generic;
using UnityEngine;

public class GunControl : MonoBehaviour {

	Dictionary<string,GunRef> _gunRefs;
	GunDataController _dataController;


	// *************** PUBLIC **************

	public void Init () {
		
		_gunRefs = new Dictionary<string,GunRef>();
		_dataController = new GunDataController();
	}

	public GunRef GetGun ( string id ) {

		if ( !_gunRefs.ContainsKey( id ) ) {

			var gunRef = new GunRef( _dataController.LoadGun( id ) );
			_gunRefs.Add( id, gunRef );

			gunRef.OnGunChanged += () => { SaveGun( id ); }; 
		}

		return _gunRefs[ id ];
	}

	
	// *************** PRIVATE **************
	
	private void SaveGun ( string id ) {

		var gunRef = GetGun( id );

		_dataController.SaveGun( id, gunRef.Gun );
	}
}
