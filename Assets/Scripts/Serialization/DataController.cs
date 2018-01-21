using System.IO;

public abstract class DataController {	

	protected string LoadFileFromPath ( string path ) {

		if ( File.Exists( path ) ) {
			return File.ReadAllText( path );
		}

		return "";
	}
 }
