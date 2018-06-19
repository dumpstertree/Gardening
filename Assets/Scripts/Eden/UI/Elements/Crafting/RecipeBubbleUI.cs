using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Elements {
	
	public class RecipeBubbleUI : MonoBehaviour {

		public Toggle Toggle {
			get{ return _toggle; }
		}
		public Crafting.Recipe Recipe {
			get{ return _recipe; }
		}

		[SerializeField] private Image _sprite;
		[SerializeField] private Toggle _toggle;

		private Crafting.Recipe _recipe;

		public void SetRecipe ( Crafting.Recipe recipe ) {
			_recipe = recipe;
			_sprite.sprite = _recipe.OutputObject.Sprite;
		}
	}
}