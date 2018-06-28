using UnityEngine;

public class Block : MonoBehaviour {
	
	
	// ****************** Events *********************
	
	public delegate void ActionEvent ( Block block );
	public ActionEvent OnAction;

	public delegate void GrabEvent ( Block block );
	public GrabEvent OnGrab;

	public delegate void ReleaseEvent ( Block block );
	public ReleaseEvent OnRelease;
	
	public delegate void ShiftEvent ( Vector3 newCursorPoint );
	public ShiftEvent OnShift;

	public delegate void RotateEvent ( float rotation );
	public RotateEvent OnRotate;

	
	// ****************** Public *********************

	public bool IsGrabbed { 
		set; get;
	}
	public bool IsValid {
		get { return GetIsValid(); }
	}

	
	// ****************** Private *********************

	private bool GetIsValid () {
		
		RaycastHit hit;
 		if (Physics.Raycast( transform.position, Vector3.forward, out hit, Mathf.Infinity )) {
 			return false;
 		} 

 		return true;
	}
	

	public void PerformAction () {

		if ( OnAction != null ) {
			OnAction( this );
		}
	}
	public void Grab () {

		if ( OnGrab != null ) {
			OnGrab ( this );
		} 
	}
	public void Release () {
		
		if ( OnRelease != null ) {
			OnRelease ( this );
		} 	
	}

	public void Shift ( Vector3 cameraSpace ) {

		if ( OnShift != null ) {
			OnShift ( cameraSpace );
		}
	}
	public void Rotate ( float rotation ) {

		if ( OnRotate != null ) {
			OnRotate ( rotation );
		}
	}

	public static GameObject GetInstance ( char forID ) {

		GameObject inst = null;
	
		switch ( forID ) {

			case 'x': return Instantiate( Resources.Load<GameObject>( "Block" ) );

			
			// Projectors
			case '⇡': 
				inst = Instantiate( Resources.Load<GameObject>( "Projector" ));
				inst.transform.rotation = Quaternion.AngleAxis( 0, Vector3.forward );
				break;

			case '⇢':
				inst = Instantiate( Resources.Load<GameObject>( "Projector" ));
				inst.transform.rotation = Quaternion.AngleAxis( 270, Vector3.forward );
				break;

			case '⇣':
				inst = Instantiate( Resources.Load<GameObject>( "Projector" ));
				inst.transform.rotation = Quaternion.AngleAxis( 180, Vector3.forward );
				break;

			case '⇠':
				inst = Instantiate( Resources.Load<GameObject>( "Projector" ));
				inst.transform.rotation = Quaternion.AngleAxis( 90, Vector3.forward );
				break;

			
			// Recievers
			case '∪': 
				inst = Instantiate( Resources.Load<GameObject>( "Reciever" ));
				inst.transform.rotation = Quaternion.AngleAxis( 0, Vector3.forward );
				break;

			case '⊂':
				inst = Instantiate( Resources.Load<GameObject>( "Reciever" ));
				inst.transform.rotation = Quaternion.AngleAxis( 270, Vector3.forward );
				break;

			case '∩':
				inst = Instantiate( Resources.Load<GameObject>( "Reciever" ));
				inst.transform.rotation = Quaternion.AngleAxis( 180, Vector3.forward );
				break;

			case '⊃':
				inst = Instantiate( Resources.Load<GameObject>( "Reciever" ));
				inst.transform.rotation = Quaternion.AngleAxis( 90, Vector3.forward );
				break;

			case 'o':
				inst = Instantiate( Resources.Load<GameObject>( "Light" ));
				inst.transform.rotation = Quaternion.AngleAxis( 0, Vector3.forward );
				break;
		}

		return inst;
	}
}
