using Dumpster.Core;
using Dumpster.BuiltInModules;
using Dumpster.Tweening;
using Eden.Life;
using Eden.Model;
using UnityEngine;
using Eden.UI.Elements;
using Eden.Characteristics;

namespace Eden.UI.Panels {
	
	public class QuickSlotUI: Dumpster.BuiltInModules.Panel {
		

		//***************** Override **********************

		protected override void OnInit () {

			base.OnInit ();

			_items = new Eden.UI.Elements.Item[ 3 ];
			_movementTweens = new Tween[ 3 ];
			_scaleTweens = new Tween[ 3 ];
		}
		protected override void OnPresent () {
			
			base.OnPresent ();

			Clear ();
			Reload ();
		}

		
		//***************** Private **********************


		[SerializeField] private Eden.UI.Elements.Item _itemPrefab;
		[SerializeField] private RectTransform _content;
		
		private Eden.UI.Elements.Item[] _items;
		private Tween[] _movementTweens;
		private Tween[] _scaleTweens;

		private float _animationDuration = 0.2f;
		private float _spacing = 125;
		private int _index = -1;


		private Actor _blackBox {
			
			get{ return Game.GetModule<Navigation>()?.CurrentArea.LoadedPlayer.GetComponent<Actor>(); }
		}
		private int _leftMostArrayIndex {
			
			get{ return 0; }
		}
		private int _rightMostArrayIndex {
			
			get{ return _items.Length - 1; }
		}


		private void Update () {

			CheckIndexChange ();
		}
		private void Clear () {

			foreach( Eden.UI.Elements.Item item in _items ) {
				if ( item != null ) {
					Destroy( item.gameObject );
				}
			}
		}
		private void Reload () {

		
			// left
			_items[ 0 ] = CreateItem (
				 _blackBox.GetCharacteristic<EquippedItemsInventory>().Inventory.GetInventoryItem( 
					Wrap( _blackBox.GetCharacteristic<EquippedItemsInventory>().NumOfItem - 1 )
				), 0
			);
			_items[ 0 ].transform.localScale = ScaleForIndex( 0 );

		
			// center
			_items[ 1 ] = CreateItem (
				 _blackBox.GetCharacteristic<EquippedItemsInventory>().Inventory.GetInventoryItem( 
					Wrap( _blackBox.GetCharacteristic<EquippedItemsInventory>().NumOfItem )
				), 1
			);
			_items[ 1 ].transform.localScale = ScaleForIndex( 1 );


			// right
			_items[ 2 ] = CreateItem (
				 _blackBox.GetCharacteristic<EquippedItemsInventory>().Inventory.GetInventoryItem( 
					Wrap( _blackBox.GetCharacteristic<EquippedItemsInventory>().NumOfItem + 1 )
				), 2
			);
			_items[ 2 ].transform.localScale = ScaleForIndex( 2 );


			_index = _blackBox.GetCharacteristic<EquipedItemSwapper>().EquipedIndex;
		}
	
		
		private void CheckIndexChange () {

			var index = _blackBox.GetCharacteristic<EquipedItemSwapper>().EquipedIndex;

			if ( _index == index ) {
				return;
			}

			if ( index == Wrap( _index - 1 ) ) {
				ShiftIndexRight( index );
			
			} else if ( index == Wrap( _index + 1 ) ) {
				ShiftIndexLeft( index );
			
			}  else {
				SetIndex ();
			}

			_index = index;
		}
		private void SetIndex () {

			Clear ();
			Reload ();
		}
		private void ShiftIndexRight ( int newIndex ) {

			DestroyItemOnRight ();
			ShiftItemsRight ();

			AddItemOnLeft( newIndex );

			Move ();
		}
		private void ShiftIndexLeft ( int newIndex ) {

			DestroyItemOnLeft ();
			ShiftItemsLeft ();

			AddItemOnRight( newIndex );

			Move ();
		}


		private void DestroyItemOnLeft () {

			var item = _items[ _leftMostArrayIndex ];

			Tween.Vector3( 
				setter: v =>  item.transform.localScale = v, 
				startValue: item.transform.localScale,
				targetValue: Vector3.zero, 
				time: _animationDuration
			).OnComplete( () => Destroy( item.gameObject ) );
		}
		private void DestroyItemOnRight () {

			var item = _items[ _rightMostArrayIndex ];

			Tween.Vector3( 
				setter: v =>  item.transform.localScale = v, 
				startValue: item.transform.localScale,
				targetValue: Vector3.zero, 
				time: _animationDuration
			).OnComplete( () => Destroy( item.gameObject ) );
		}
		private void AddItemOnLeft (  int centerIndex  ) {

			var newItemEquipIndex = ItemForIndex( centerIndex - 1 );
			var newItemInstance = CreateItem(  newItemEquipIndex, _leftMostArrayIndex );

			_items[ _leftMostArrayIndex ] = newItemInstance;
		}
		private void AddItemOnRight ( int centerIndex ) {

			var newItemEquipIndex = ItemForIndex( centerIndex + 1 );
			var newItemInstance = CreateItem( newItemEquipIndex, _rightMostArrayIndex );

			_items[ _rightMostArrayIndex ] = newItemInstance;
		}
		private void ShiftItemsLeft () {

			_items = ArrayMath.ShiftArrayLeft<Eden.UI.Elements.Item>( _items );	
		}
		private void ShiftItemsRight () {

			_items = ArrayMath.ShiftArrayRight<Eden.UI.Elements.Item>( _items );
		}
		private void Move () {
			
			for( int i=0; i<_items.Length; i++ ) {

				var item = _items[ i ];

				// move
				if( _movementTweens[ i ] != null ) { _movementTweens[ i ].Kill(); }

				_movementTweens[ i ] = Tween.Vector3( 
					setter: v => item.transform.localPosition = v , 
					startValue: item.transform.localPosition,
					targetValue: PosForIndex( i ), 
					time: _animationDuration
				);

				// scale
				if( _scaleTweens[ i ] != null ) { _scaleTweens[ i ].Kill(); }
				
				_scaleTweens[ i ] = Tween.Vector3( 
					setter: v => item.transform.localScale = v , 
					startValue: item.transform.localScale,
					targetValue: ScaleForIndex( i ), 
					time: _animationDuration
				);
			}
		}



		private int Wrap ( int index ) {

			var max = (float) _blackBox.GetCharacteristic<EquippedItemsInventory>().NumOfItem ;
			return Mathf.RoundToInt( Mathf.Repeat( (float)index, max ) );
		}
		private Eden.Model.Item ItemForIndex ( int index ) {
		
			return  _blackBox.GetCharacteristic<EquippedItemsInventory>().Inventory.GetInventoryItem( Wrap( index ) );
		}
		private Vector3 PosForIndex ( int index ) {

			var half = Mathf.Floor( _items.Length/2f );
			var start = -(half * _spacing);

			return new Vector3( start + (index * _spacing), 0f, 0f );
		}
		private Vector3 ScaleForIndex ( int index ) {
			
			if ( index == 1 ) {
				return new Vector3( 1f, 1f, 1f );
			} else if ( index == 0 || index == 2 ) {
				return new Vector3( 0.75f, 0.75f, 0.75f );
			} else {
				return Vector3.zero;
			}
		}
		private Eden.UI.Elements.Item CreateItem ( Eden.Model.Item item, int index ) {

			var inst = Instantiate( _itemPrefab );
			inst.transform.SetParent( _content );
			inst.SetBackingItem( item );

			inst.transform.localPosition = PosForIndex( index );
			inst.transform.localScale = Vector3.zero;
			return inst;
		}
	}
}