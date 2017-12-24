using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interactable.OptionalComponent;

namespace Interactable.Component {

	[RequireComponent(typeof(Destroyable))]
	public class Plantable : MonoBehaviour {

		[SerializeField] private Transform _spawner;

		private Destroyable _destroyable;
	
		public void Plant ( Player player, PlantData data ){

			var go = Instantiate( data.Prefab );
			go.transform.position = _spawner.position;
			go.transform.rotation = _spawner.rotation;

			_destroyable.Destroy();
		}

		private void Awake(){
		
			_destroyable = GetComponent<Destroyable>();
		}
	}
}