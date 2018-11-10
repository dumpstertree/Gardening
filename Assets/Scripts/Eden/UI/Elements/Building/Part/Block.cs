using UnityEngine;

namespace Eden.UI.Elements.Building {
	
	public class Block : MonoBehaviour {
		
		
		// ****************** Events *********************
		
		public delegate void ActionEvent ( Block block );
		public ActionEvent OnAction;

		
		// ****************** Public *********************

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
}
