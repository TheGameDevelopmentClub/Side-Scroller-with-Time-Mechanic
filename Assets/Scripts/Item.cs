using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item {

	public string name;
	public Texture2D icon;
	public string description;
	public ItemType itemType;

	public enum ItemType {
		gun,
		melee,
		buff
	}
}
