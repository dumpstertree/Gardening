using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Eden.UI.Panels {
	
	public class QuickSlot: Dumpster.Core.BuiltInModules.UI.Panel {

		[SerializeField] private ItemBubbleUI _itemPrefab;
		[SerializeField] private RectTransform _content;
		[SerializeField] private List<ItemBubbleUI> _itemInstances;

		[SerializeField] private float _animationDuration = 0.5f;
		[SerializeField] private float _xSpacing= 50;
		[SerializeField] private int _numVisibleItems = 3;

		private Eden.Life.BlackBox _blackBox {
			get{ return EdensGarden.Instance.Rooms.CurrentArea.LoadedPlayer.GetComponent<Eden.Life.BlackBox>(); }
		}

		//***************** Override **********************

		protected override void OnInit () {

			base.OnInit ();

			_itemInstances = new List<ItemBubbleUI>();
		}
		protected override void OnPresent () {
			
			base.OnPresent ();

			_blackBox.QuickslotChip.OnIndexChanged += index => { SetIndex( index, false ); };

			Clear ();
			Reload ();
			SetIndex( _blackBox.QuickslotChip.Index, true ); 
		}
		protected override void OnDismiss () {
			
			_blackBox.QuickslotChip.OnIndexChanged -= index => { SetIndex( index, false ); };

			base.OnDismiss ();
		}


		//***************** Private **********************
		
		private void Clear () {

			for ( int i=0; i<_itemInstances.Count; i++ ) {
				Destroy( _itemInstances[ 0 ] );
				_itemInstances.RemoveAt( 0 );
			}
		}
		private void Reload () {

			for ( int i =1; i<_blackBox.EquipedItems.InventoryCount; i++ ) {
				CreateItem ( _blackBox.EquipedItems.GetInventoryItem( i ) );
			}
		}
		private void CreateItem ( InventoryItem item ) {

			var inst = Instantiate( _itemPrefab );
			inst.transform.SetParent( _content );
			inst.SetItem( item );

			_itemInstances.Add( inst );
		}
		private void SetIndex ( int index, bool instant ) {

			StopAllCoroutines();

			for ( int i=0; i< _itemInstances.Count; i++ ){

				var item = _itemInstances[ i ];
				var dif = index - i - 1;

				var pos = new Vector3( -_xSpacing * dif, 0, 0 );
				var scale = ( Mathf.Abs(dif) > _numVisibleItems/2 ) ? 0f : 1f;

				StartCoroutine( Animate( item.gameObject,  pos, new Vector3( scale, scale, scale ) ) );
			}
		}
		private IEnumerator Animate ( GameObject go, Vector3 targetPos, Vector3 targetScale ) {

			var startPos = go.transform.localPosition;
			var startScale = go.transform.localScale;

			for ( float t=0f; t<_animationDuration; t+= Time.deltaTime ) {
				var frac = t/_animationDuration;
				go.transform.localPosition = Vector3.Lerp( startPos, targetPos, frac );
				go.transform.localScale = Vector3.Lerp( startScale, targetScale, frac );
				yield return null;
			}
			
			go.transform.localPosition = targetPos;
			go.transform.localScale = targetScale;
		}	
	}
}