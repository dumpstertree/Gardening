using UnityEngine;
using System.IO;

namespace Dumpster.Core {

	public class Data {

		public enum Path {
			Player,
			RangedWeapon,
			Buildable
		}

		private const string PLAYER_PATH = "/player/";
		private const string BUILDABLE_PATH = "/buildable/";
		private const string RANGED_WEAPON_PATH = "/ranged/";

		public void Save<T> ( string path, string fileName, T data ) {

			if ( !Directory.Exists( Application.persistentDataPath + path ) ) {
                Directory.CreateDirectory( Application.persistentDataPath + path );
            }

			var json = JsonUtility.ToJson( data, true );
			var fullPath = Application.persistentDataPath + path + fileName;
			File.WriteAllText( fullPath, json );
		}
		public T Load<T> ( string path, string fileName ) {

			var fullPath = Application.persistentDataPath + path + fileName;
			var json = LoadFileFromPath( fullPath );
			var data = JsonUtility.FromJson<T>( json );

			return data;
		}
		private string LoadFileFromPath ( string path ) {

			return File.Exists( path ) ? File.ReadAllText( path ) : "";
		}
	}
}