using UnityEngine;

namespace Model.Template {

	[System.Serializable]
	public class InteractData {

		public Controller.Item.InteractData GetController () {
			return new Controller.Item.InteractData();
		}

		[SerializeField] private string _animation;
	}
}