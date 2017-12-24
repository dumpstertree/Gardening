using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI.Elements;

namespace UI.Panels {

	public class RecipePanel : UiPanel {

		// ************* PUBLIC *****************

		public delegate void RecipeChangeEvent ( Crafting.Recipe recipe );
		public RecipeChangeEvent OnRecipeChange;

		// ************* PRIVATE *****************

		[SerializeField] private GridLayoutGroup _layoutGroup;
		[SerializeField] private RecipeBubbleUI _recipeBubblePrefab;
		[SerializeField] private ToggleGroup _toggleGroup;
			
		private const int COLLUMNS = 3;
		private const float PADDING = 25;

		private PlayerRecipes _playerRecipes {
			get{ return Game.Area.LoadedPlayer.PlayerRecipes; }
		}

		// *************************************

		protected override void OnInit () {
			
			_layoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			_layoutGroup.constraintCount = COLLUMNS;
			_layoutGroup.cellSize = _recipeBubblePrefab.GetComponent<RectTransform>().sizeDelta;
			_layoutGroup.spacing = new Vector2( PADDING, PADDING );
		}
		protected override void OnPresent () {

			Clear();
			Load();
		}

		// *************************************

		private void Clear () {
			
			for ( int i = _layoutGroup.transform.childCount-1; i > 0; i-- ) {
				var item = _layoutGroup.transform.GetChild( i );
				Destroy( item.gameObject );
			}
		}
		private void Load () {

			var recipes = _playerRecipes.KnowRecipes;
			RecipeBubbleUI first = null;

			// create icons
			foreach( Crafting.Recipe recipe in recipes ){

				var bubble = Instantiate( _recipeBubblePrefab );
				bubble.transform.SetParent( _layoutGroup.transform );
				bubble.SetRecipe ( recipe );

				bubble.Toggle.group = _toggleGroup;

				bubble.Toggle.onValueChanged.AddListener( value => { 
					if (value) { HandleChangeRecipe( bubble ); };
				});

				if ( first == null ) {
					first = bubble;
				}
			}

			// select first icon
			if ( first != null ) {
				first.Toggle.isOn = true;
			}
		}
		private void HandleChangeRecipe( RecipeBubbleUI recipeBubbleUi){

			if ( OnRecipeChange != null ){
				OnRecipeChange( recipeBubbleUi.Recipe );
			}
		}
	}
}
