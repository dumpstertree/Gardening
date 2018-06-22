using UnityEngine;

namespace Eden.Properties {
	
	public class Magnet : MonoBehaviour {

		public GameObject Root {
			get{ return _root; }
		}
		public float ReachRange { 
			get { return _reachRange; } 
		}

		[SerializeField] private float _reachRange = 1.0f;
		[SerializeField] private GameObject _root;
	}
}