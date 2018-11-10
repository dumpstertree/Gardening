using System.Collections.Generic;
using UnityEngine;
using Eden.Model;

namespace Crafting {

	public class Recipe : ScriptableObject {
		
		// *********** PUBLIC ****************

		public string RecipeName { get{ return _recipeName; } }
		public Item OutputObject { get{ return _outputObject; } }
		public List<Component> Components { get{ return _components; } }
		

		// *********** PRIVATE ****************

		[HeaderAttribute("Output")]
		[SerializeField] private string _recipeName;
		[SerializeField] private Item _outputObject;
		[SerializeField] private int _outputCount;

		[HeaderAttribute("Input")]
		[SerializeField] private List<Component> _components;


		// *********************************

		[System.Serializable]
		public struct Component {

			public Item Item { get{ return _item; } }
			public int Amount { get{ return _amount; }  }

			[SerializeField] private Item _item;
			[SerializeField] private int _amount;
		}
	}
}