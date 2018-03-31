using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class EditorHelper : MonoBehaviour {

	[MenuItem("Assets/BatchCreateArtistFont(Center)")]
	static public void BatchCreateArtistFontCenter()
	{
		ArtistFont.BatchCreateArtistFont(-0.5f);
	}

	[MenuItem("Assets/BatchCreateArtistFont(Top)")]
	static public void BatchCreateArtistFontTop()
	{
		ArtistFont.BatchCreateArtistFont(1.0f);
	}

	[MenuItem("Assets/BatchCreateArtistFont(Bottom)")]
	static public void BatchCreateArtistFontBottom()
	{
		ArtistFont.BatchCreateArtistFont(0);
	}
}
