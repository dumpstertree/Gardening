using UnityEngine;
using UnityEngine.UI;

namespace Gun {

	public class Projector : MonoBehaviour {

		public delegate void OnProjectEvent();
		public OnProjectEvent OnProject;

		public void Project () {
			
			var pos = transform.position + (transform.up * 100);		
			var x = GetComponentInParent<GunCraftingPanel>().GetCollumn( pos );
			var y = GetComponentInParent<GunCraftingPanel>().GetRow( pos );
			var slot = GetComponentInParent<GunCraftingPanel>().GetSlot( y, x );
			var reciever = slot.Reciever;

			if ( reciever != null  && reciever.Connection == Connection ) {
				reciever.Recieve();
			}
			if (reciever == null) {
				print( transform.parent.parent.name + " no reciever " );
				print( x + " : " + y );
			}
			if (reciever != null  && reciever.Connection != Connection ) {
				print( transform.parent.parent.name + " wrong connection " );
			}

			if ( OnProject != null ) {
				OnProject();
			}
		}

		public int Collumn {
			get{ return GetComponentInParent<GunCraftingPanel>().GetCollumn( transform.position ); }
		}
		public int Row {
			get{ return GetComponentInParent<GunCraftingPanel>().GetRow( transform.position ); }
		}

		// *****************************

		public Component.Connection Connection;
		
		[SerializeField] private Sprite _circleProjectorSprite;
		[SerializeField] private Sprite _squareProjectorSprite;
		[SerializeField] private Image __image;
		
		private const float SNAPPING = 50;
		private const float VISUAL_SIZE = 30;

		private Image _image {
			get{ 
				if ( __image == null ){	__image = GetComponent<Image>(); }
				return __image;
			}
		}

		// *****************************

		private void OnDrawGizmos () {

			Snap();
			SetSprite();

			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere( transform.position, VISUAL_SIZE );
			Gizmos.DrawLine( transform.position, transform.position + (transform.up * 100) );
		}
		private void Snap () {
			
			var x = Mathf.Round( transform.localPosition.x/SNAPPING ) * SNAPPING;
			var y = Mathf.Round( transform.localPosition.y/SNAPPING ) * SNAPPING;
			var z = transform.localPosition.z;
			
			transform.localPosition = new Vector3( x, y, z );
		}
		private void SetSprite () {

			switch( Connection ) {

				case Component.Connection.Circle: _image.sprite = _circleProjectorSprite; break;
				case Component.Connection.Square: _image.sprite = _squareProjectorSprite; break;
			}
		}	
	}
}
