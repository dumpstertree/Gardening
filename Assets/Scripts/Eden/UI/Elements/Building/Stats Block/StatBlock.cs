using System.Collections.Generic;
using UnityEngine;

public class StatBlock : MonoBehaviour {

	public void SetBlock ( IStatBlockDelegate statBlockDelegate ) {

		ClearStats();

		foreach ( Stat s in statBlockDelegate.GetStats() ) {
			CreateStat( s );
		}
	}


	[SerializeField] private StatVisual _statVisualPrefab;
	[SerializeField] private Transform _content;

	private List<StatVisual> _statVisuals = new List<StatVisual>();

	private void ClearStats () {

		for ( int i=_statVisuals.Count-1; i>=0; i-- ){
			var s = _statVisuals[ i ];
			Destroy( s.gameObject );
		}

		_statVisuals.Clear();
	}
	private void CreateStat ( Stat stat ) {

		var inst = Instantiate( _statVisualPrefab );
		inst.transform.SetParent( _content );
		inst.SetStat( stat );

		_statVisuals.Add( inst );
	}
}

public class BuiltStats: IStatBlockDelegate {

	public int RateOfFire { 
		get; private set; 
	}
	public int ReloadSpeed { 
		get; private set; 
	}
	public int Accuracy { 
		get; private set; 
	}
	public int NumOfBullets { 
		get; private set; 
	}
	public int ClipSize { 
		get; private set; 
	}
	public int BulletSpeed { 
		get; private set; 
	}
	public int BulletSize { 
		get; private set; 
	}

	public BuiltStats () {

		RateOfFire = 1;
		ReloadSpeed = 1;
		Accuracy = 1;
		NumOfBullets = 1;
		ClipSize = 1;
		BulletSpeed = 1;
		BulletSize = 1;
	}
	public BuiltStats ( int rateOfFire, int reloadSpeed, int accuracy, int numOfBullets, int clipSize, int bulletSpeed, int bulletSize ) {

		RateOfFire = rateOfFire;
		ReloadSpeed = reloadSpeed;
		Accuracy = accuracy;
		NumOfBullets = numOfBullets;
		ClipSize = clipSize;
		BulletSpeed = bulletSpeed;
		BulletSize = bulletSize;
	}
	
	public void Add ( BuiltStats statsToAdd ) {

		RateOfFire   += statsToAdd.RateOfFire;
		ReloadSpeed  += statsToAdd.ReloadSpeed;
		Accuracy 	 += statsToAdd.Accuracy;
		NumOfBullets += statsToAdd.NumOfBullets;
		ClipSize 	 += statsToAdd.ClipSize;
		BulletSpeed  += statsToAdd.BulletSpeed;
		BulletSize   += statsToAdd.BulletSize;
	}
	List<Stat> IStatBlockDelegate.GetStats () {

		return new List<Stat>{ 

			new Stat( "Rate of Fire", null, RateOfFire ),
			new Stat( "Reload Speed", null, ReloadSpeed ),
			new Stat( "Accuracy", null, Accuracy ),
			new Stat( "Number Of Bullets", null, NumOfBullets ),
			new Stat( "Clip Size", null, ClipSize ),
			new Stat( "Bullet Speed", null, BulletSpeed ),
			new Stat( "Bullet Size", null, BulletSize ) 

		};
	}
}

public interface IStatBlockDelegate {

	List<Stat> GetStats ();

}
public struct Stat {

	public string Name { get; }
	public Sprite Sprite { get; }
	public int Level { get; }

	public Stat ( string name, Sprite sprite, int level ) {

		Name = name;
		Sprite =sprite;
		Level = level;
	}
}