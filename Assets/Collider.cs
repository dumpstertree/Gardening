using UnityEngine;

namespace GunCrafting {

	public class Collider : MonoBehaviour {

		// ***************************
		public int X {
			get{ return GetComponentInParent<GunCraftingPanel>().GetRow( transform.position ); }
		}
		public int Y {
			get{ return GetComponentInParent<GunCraftingPanel>().GetCollumn( transform.position ); }
		}

		// ***************************

		[SerializeField] private int _x;
		[SerializeField] private int _y;

		// ***************************

		private void OnDrawGizmos () {

			_x = X;
			_y = Y;

			Gizmos.color = Color.green;
			Gizmos.DrawWireCube( transform.position, GetComponent<RectTransform>().sizeDelta );

			//GetComponentInParent<GunCraftingPanel>().Snap( transform );
		}
	}
}