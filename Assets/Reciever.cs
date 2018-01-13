using UnityEngine;
using UnityEngine.UI;

namespace Gun {
	
	public class Reciever : MonoBehaviour {

		public delegate void OnRecieveEvent();
		public OnRecieveEvent OnRecieve;

		public void Recieve () {
			
			if ( OnRecieve != null ) {
			//	print( transform.parent.parent.name );
				OnRecieve();
			}
		}

		public int Collumn {
			get{ return GetComponentInParent<GunCraftingPanel>().GetCollumn( transform.position ); }
		}
		public int Row {
			get{ return GetComponentInParent<GunCraftingPanel>().GetRow( transform.position ); }
		}

		public Component.Connection Connection;
		[SerializeField] private Sprite _circleRecieverSprite;
		[SerializeField] private Sprite _squareRecieverSprite;

		private const float SNAPPING = 50;
		private const float VISUAL_SIZE = 30;

		private Image __image;
		private Image _image {
			get{ 
				if ( __image == null ){	__image = GetComponent<Image>(); }
				return __image;
			}
		}

		private void OnDrawGizmos () {

			Snap();
			SetSprite();

			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere( transform.position, VISUAL_SIZE );
		}
		private void Snap () {
			
			var x = Mathf.Round( transform.localPosition.x/SNAPPING ) * SNAPPING;
			var y = Mathf.Round( transform.localPosition.y/SNAPPING ) * SNAPPING;
			var z = transform.localPosition.z;
			
			transform.localPosition = new Vector3( x, y, z );
		}
		private void SetSprite () {

			switch( Connection ) {

				case Component.Connection.Circle: _image.sprite = _circleRecieverSprite; break;
				case Component.Connection.Square: _image.sprite = _squareRecieverSprite; break;
			}
		}
	}
}