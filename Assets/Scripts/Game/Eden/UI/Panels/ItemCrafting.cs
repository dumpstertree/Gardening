using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI.Elements;
using Crafting;
using Dumpster.Core.BuiltInModules.UI;

namespace Eden.UI.Panels {
	
	public class ItemCrafting : InventoryUI1 {

		// ************* PRIVATE *****************

		[Header("Refrence")]
		[SerializeField] private Text _recipeName;
		[SerializeField] private GridLayoutGroup _componentLayoutGroup;
		[SerializeField] private GridLayoutGroup _craftingSlotLayoutGroup;
		[SerializeField] private Button _craftButton;
		[SerializeField] private Button _exitButton;
		[SerializeField] private ItemBubbleUI _craftedSlot;

		[Header("Prefab")]
		[SerializeField] private ComponentBubbleUI _componentBubblePrefab;

		private const int NUM_OF_CUSTOM_SLOTS = 1;
		private const int CRAFTED_SLOT_INDEX = 0;
		private const int CRAFTING_SLOTS = 9;

		private const float CRAFTING_SLOT_PADDING = 25;
		private const int CRAFTING_SLOT_COLLUMNS = 3;

		private const float COMPONENT_PADDING = 25;
		private const int COMPONENT_COLLUMNS = 3;

		private Crafting.Recipe _recipe;
		private Inventory _craftingSlots = new Inventory( CRAFTING_SLOTS + NUM_OF_CUSTOM_SLOTS);

		// ***************************************

		protected override void OnInit () {

			_craftingSlotLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			_craftingSlotLayoutGroup.constraintCount = CRAFTING_SLOT_COLLUMNS;
			_craftingSlotLayoutGroup.cellSize = _itemBubblePrefab.GetComponent<RectTransform>().sizeDelta;
			_craftingSlotLayoutGroup.spacing = new Vector2( CRAFTING_SLOT_PADDING, CRAFTING_SLOT_PADDING );
				
			_componentLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			_componentLayoutGroup.constraintCount = COMPONENT_COLLUMNS;
			_componentLayoutGroup.cellSize = _componentBubblePrefab.GetComponent<RectTransform>().sizeDelta;
			_componentLayoutGroup.spacing = new Vector2( COMPONENT_PADDING, COMPONENT_PADDING );

			_craftButton.onClick.AddListener( () => {
				if ( CheckForComponents() ){
					Craft();
				}
			});

			_craftingSlots.OnInventoryItemChanged += ( index, item ) => {
				if ( index == CRAFTED_SLOT_INDEX ){
					_craftButton.interactable = item == null;
				}
			};

			_exitButton.onClick.AddListener( () => {
				// Exit();
			});

			/*
			_recipePanel.OnRecipeChange += (recipe) => {
				_recipe = recipe;
				_recipeName.text = _recipe.RecipeName;
				ClearComponents();
				
				ReloadComponents( _recipe );
			};
			*/
		}

		// ***************************************

		private void ReloadComponents ( Crafting.Recipe recipe ) {

			foreach( Crafting.Recipe.Component component in recipe.Components ){

				var bubble = Instantiate( _componentBubblePrefab );
				bubble.transform.SetParent( _componentLayoutGroup.transform );
				bubble.SetComponent( component );
			}
		}
		private void ClearComponents (){

			for (int i=_componentLayoutGroup.transform.childCount-1; i>=0; i--){
				var child = _componentLayoutGroup.transform.GetChild( i ).gameObject;
				Destroy( child );
			}
		}
		private bool CheckForComponents () {

			/*
			foreach( Crafting.Component c in _recipe.Components ){

				for( int i =0; i<NUM_OF_CUSTOM_SLOTS; i++ ){

					var item = _craftingSlots.GetInventoryItem( i + NUM_OF_CUSTOM_SLOTS );
					if ( c.Item == item.GetType ) {

						if ( item.Count >= c.Amount ) {

							continue;
						}
					}
				}
			}
			*/
			
			return true;
		}
		private void Craft () {
			
			_craftingSlots.SetInventoryItem( CRAFTED_SLOT_INDEX, _recipe.OutputObject );
		}

		// ***************************************
	 
		protected override Inventory GetInventory (){
			
			return _craftingSlots;
		}
		protected override ItemBubbleUI[] GetItemBubbles (){

			var itemBubbles = new ItemBubbleUI[ CRAFTING_SLOTS + NUM_OF_CUSTOM_SLOTS ];

			itemBubbles[ CRAFTED_SLOT_INDEX ] = _craftedSlot;
			_craftedSlot.Index = CRAFTED_SLOT_INDEX;

			for ( int i = 0; i<CRAFTING_SLOTS; i++  ){

				var itemBubble = Instantiate( _itemBubblePrefab );
				itemBubble.transform.SetParent( _craftingSlotLayoutGroup.transform, false );
				itemBubble.Index = i + NUM_OF_CUSTOM_SLOTS;

				itemBubbles[ i + NUM_OF_CUSTOM_SLOTS] = itemBubble;
			}

			return itemBubbles;
		}
	}
}
