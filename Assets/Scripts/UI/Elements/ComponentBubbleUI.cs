using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UI.Elements {
	
	public class ComponentBubbleUI : MonoBehaviour {

		[SerializeField] private Image _sprite;
		[SerializeField] private Text _text;

		public void SetComponent ( Crafting.Recipe.Component component ) {

			_sprite.sprite = component.Item.Sprite;
			_text.text = component.Amount.ToString();
		}	
	}
}