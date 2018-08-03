using UnityEngine;

namespace Eden.Model.Building {
	
	[System.Serializable]
	abstract public class Part<T> {

		public string PrefabID{
			get{ return _prefabID; }
		}	
		public char[,] Blocks {
			get{ return _blocks; }
		}
		public T Stats {
			get{ return _partStats; }
		}

		[SerializeField] protected string _prefabID;
		[SerializeField] protected char[,] _blocks;
		[SerializeField] protected T _partStats;

		protected Part( char[,] blocks, T stats, string prefabID ) {

			// Projectors 	⇡ ⇢ ⇣ ⇠
			// Recievers  	∪ ⊂ ∩ ⊃

			_prefabID = prefabID;
			_blocks = blocks;
			_partStats = stats; 
		}
	}
}