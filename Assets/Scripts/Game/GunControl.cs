using System.Collections.Generic;
using UnityEngine;
using Model;

public class GunControl : MonoBehaviour {

	Dictionary<string,Gun> _gunRefs;
	GunDataController _dataController;


	// *************** PUBLIC **************

	public void Init () {
		
		 _gunRefs = new Dictionary<string,Gun>();
		 _dataController = new GunDataController();
	}

	public Gun GetGun ( string id ) {

		if ( !_gunRefs.ContainsKey( id ) ) {

			var gun = _dataController.LoadGun( id );
			_gunRefs.Add( id, gun );

			// gunRef.OnGunModified += () => { SaveGun( id ); }; 
		}

		return _gunRefs[ id ];
	}

	public delegate void GunModifiedEvent ( string gunID );
	public GunModifiedEvent OnGunModified;


	// *************** PRIVATE **************
	
	private void SaveGun ( string id, Gun gun ) {

		_dataController.SaveGun( id, gun );
	}
}
