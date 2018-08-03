using UnityEngine;
using Eden.Properties;

public class PropertiesObject : MonoBehaviour {


	public bool Active {
		set { _active = value; }
		get { return _active; }
	}


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


	public bool IsTargetable {
		get { return _targetable != null; }
	}
	public Targetable TargetDelegate {
		get { return _targetable; }
	}

	
	private void Update () {

		if ( Active ) {

			if ( IsMagnet ) ((IProperty)_magnet).Update();
			if ( IsMagnetic ) ((IProperty)_magnetic).Update();
			if ( IsTargetable ) ((IProperty)_targetable).Update();
		}
	}
	

	[Header( "Property Properties" )]
	[SerializeField] private bool _active = true;

	[Header( "Properties" )]
	[SerializeField] private Magnet _magnet;
	[SerializeField] private Magnetic _magnetic;
	[SerializeField] private Targetable _targetable;
}

public interface IProperty {
	
	void Update ();
}
