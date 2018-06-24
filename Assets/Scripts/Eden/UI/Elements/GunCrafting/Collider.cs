using UnityEngine;
using UnityEngine.EventSystems;

namespace Eden.UI.Elements.GunCrafting {

	public class Collider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

		// ***************************

		public bool MouseIsOver {
			get{ return _mouseIsOver; }
		}
		public Type BlockType {
			get{ return _blockType; }
		}

		// ***************************

		public enum Type {
	    	Space,
	    	Reciever,
	    	Projector
	    }
	    public enum Connection {
			None,
			Circle,
			Square,
		}

		// ***************************

		private const int VISUAL_SIZE = 38;

		[SerializeField] private Type _blockType;
		[SerializeField] private bool _mouseIsOver;
		[SerializeField] private Connection _connection;

		// ***************************

		private void OnDrawGizmos () {

			// draw gizmos
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube( transform.position, GetComponent<RectTransform>().sizeDelta );

			switch ( _blockType ){
				case Type.Space : DrawSpace(); break;
				case Type.Reciever : DrawReciever(); break;
				case Type.Projector : DrawProjector(); break;
			}
		}
		private void DrawSpace () {
			
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube( transform.position, GetComponent<RectTransform>().sizeDelta );
		}
		private void DrawReciever () {
			
			Gizmos.color = Color.blue;
			Gizmos.DrawLine( transform.position, transform.position + (transform.up * 100) );

			switch( _connection ) {

				case Connection.Circle: Gizmos.DrawWireSphere( transform.position, VISUAL_SIZE/2 ); break;
				case Connection.Square: Gizmos.DrawWireCube( transform.position, new Vector3(VISUAL_SIZE,VISUAL_SIZE,VISUAL_SIZE) ); break;
			}
		}
		private void DrawProjector () {
			
			Gizmos.color = Color.red;
			Gizmos.DrawLine( transform.position, transform.position + (transform.up * 100) );

			switch( _connection ) {

				case Connection.Circle: Gizmos.DrawWireSphere( transform.position, VISUAL_SIZE/2 ); break;
				case Connection.Square: Gizmos.DrawWireCube( transform.position, new Vector3(VISUAL_SIZE,VISUAL_SIZE,VISUAL_SIZE) ); break;
			}
		}

		// ***************************

		public void OnPointerEnter( PointerEventData eventData ) {
			_mouseIsOver = true;
	     }
		
		public void OnPointerExit( PointerEventData eventData ) {
			_mouseIsOver = false;
	     }
	}
}