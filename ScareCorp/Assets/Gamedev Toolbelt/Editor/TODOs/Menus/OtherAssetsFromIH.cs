﻿using UnityEngine;
using UnityEditor;

namespace com.immortalhydra.gdtb.todos
{
	public class OtherAssetsFromIH : MonoBehaviour
	{

#region METHODS

	[MenuItem("Window/Gamedev Toolbelt/TODOs/Our other Assets")]
	private static void GoToAssetStorePage()
	{
		Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/search/page=1/sortby=popularity/query=publisher:15617");
	}

#endregion

	}
}
