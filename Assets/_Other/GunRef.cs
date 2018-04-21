using System.Collections;
using UnityEngine;

public class GunRef {

	// ************* INIT ****************

	public GunRef () {}
	public void GunModified () {
		
		if ( OnGunModified != null ) {
			OnGunModified();
		}
	}


	// ************* DELEGATES ****************

	public delegate void GunModifiedEvent ();
	public GunModifiedEvent OnGunModified;
	
	// ************ PUBLIC *****************

 	public Model.Gun Gun {
 		get{ return _gun; }
 	}

 	// *********** PRIVATE *****************

 	private Model.Gun _gun;

}
