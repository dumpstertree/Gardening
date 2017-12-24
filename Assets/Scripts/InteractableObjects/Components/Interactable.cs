using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactable.Component {

	public class Interactable : MonoBehaviour {

		public virtual void Interact( Player player, InventoryItem item ){}
	}
}