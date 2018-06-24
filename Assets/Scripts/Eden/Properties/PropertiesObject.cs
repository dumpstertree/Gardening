using UnityEngine;
using Eden.Properties;

public class PropertiesObject : MonoBehaviour {


	public bool IsMagnet {
		get { return _magnet != null; }
	}
	public Magnet MagnetDelegate {
		get { return _magnet; }
	}
	

	public bool IsMagnetic {
		get { return _magnetic != null; }
	}
	public Magnetic MagneticDelegate {
		get { return _magnetic; }
	}
	


	[SerializeField] private Magnet _magnet;
	[SerializeField] private Magnetic _magnetic;
}
