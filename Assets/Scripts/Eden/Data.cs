using UnityEngine;
using System.IO;

namespace Eden {

	public class Data {

		public enum Path {
			Player,
			RangedWeapon,
			Buildable
		}

		private const string PLAYER_PATH = "/player/";
		private const string BUILDABLE_PATH = "/buildable/";
		private const string RANGED_WEAPON_PATH = "/ranged/";

		public void Save<T> ( Path path, string fileName, T data ) {

			if ( !Directory.Exists( Application.persistentDataPath + GetPath( path ) ) ) {
                Directory.CreateDirectory( Application.persistentDataPath + GetPath( path ) );
            }

			var json = JsonUtility.ToJson( data, true );
			var fullPath = Application.persistentDataPath + GetPath( path ) + fileName;
			File.WriteAllText( fullPath, json );
		}
		public T Load<T> ( Path path, string fileName ) {

			var fullPath = Application.persistentDataPath + GetPath( path ) + fileName;
			var json = LoadFileFromPath( fullPath );
			var data = JsonUtility.FromJson<T>( json );

			return data;
		}

		private string GetPath ( Path path ) {
			
			switch ( path ) {
				case Path.Player : return PLAYER_PATH;
				case Path.Buildable : return BUILDABLE_PATH;
				case Path.RangedWeapon : return RANGED_WEAPON_PATH;
			}

			return "/";
		}
		private string LoadFileFromPath ( string path ) {

			return File.Exists( path ) ? File.ReadAllText( path ) : "";
		}
	}
}