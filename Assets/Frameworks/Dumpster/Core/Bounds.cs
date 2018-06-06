using UnityEngine;

namespace Dumpster.Core {

	[System.Serializable]
	public class Bounds {

		[SerializeField] private Transform _transform;

		[SerializeField] private Vector3 _size;
		[SerializeField] private Vector3 _offset;

		public float Width {
			get{ return _size.x; }
		}
		public float Height {
			get{ return _size.y; }
		}
		public float Depth {
			get{ return _size.z; }
		}
		public Vector3 Size {
			get{ return _size; }
		}
		public Vector3 Offset {
			get{ return _offset; }
		}
		public Vector3 Center {
			get { return _transform.position + _offset; }
		}
		public Vector3 TopLeftFront {
			get{ 
				var x = Width/2;
				var y = Height/2;
				var z = Depth/2;
				return _transform.position + _offset + new Vector3( x, y, z );  
			}
		}
		public Vector3 TopRightFront {
			get{ 
				var x = -Width/2;
				var y = Height/2;
				var z = Depth/2;
				return _transform.position + _offset + new Vector3( x, y, z );  
			}
		}
		public Vector3 BottomLeftFront{
			get{ 
				var x = Width/2;
				var y = -Height/2;
				var z = Depth/2;
				return _transform.position + _offset + new Vector3( x, y, z );  
			}
		}
		public Vector3 BottomRightFront{
			get{ 
				var x = -Width/2;
				var y = -Height/2;
				var z = Depth/2;
				return _transform.position + _offset + new Vector3( x, y, z );  
			}
		}
		public Vector3 TopLeftBack { 
			get{ 
				var x = Width/2;
				var y = Height/2;
				var z = -Depth/2;
				return _transform.position + _offset + new Vector3( x, y, z );  
			}
		}
		public Vector3 TopRightBack {
			get{ 
				var x = -Width/2;
				var y = Height/2;
				var z = -Depth/2;
				return _transform.position + _offset + new Vector3( x, y, z );  
			}
		}
		public Vector3 BottomLeftBack {
			get{ 
				var x = Width/2;
				var y = -Height/2;
				var z = -Depth/2;
				return _transform.position + _offset + new Vector3( x, y, z );  
			}
		}
		public Vector3 BottomRightBack {
			get{ 
				var x = -Width/2;
				var y = -Height/2;
				var z = -Depth/2;
				return _transform.position + _offset + new Vector3( x, y, z );  
			}
		}
	}
}