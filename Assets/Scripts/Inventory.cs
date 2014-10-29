using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	public List<Item> inventory = new List<Item>();
	public int selected = 0;
	private ItemDatabase database;
	public Texture2D selector;

	// Use this for initialization
	void Start () {
		database = GameObject.FindGameObjectWithTag ("ItemDatabase").GetComponent<ItemDatabase>();
		for (int i = 0; i < database.items.Count; i++) 
		{
			inventory.Add (database.items [i]);
		}
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			if (selected == 0) selected = inventory.Count - 1;
			else selected--;
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			if (selected == inventory.Count - 1) selected = 0;
			else selected++;
		}
	}

	void OnGUI()
	{
		for(int i = 0; i < inventory.Count; i++)
		{
			GUI.Label(new Rect(20+(70)*i, 20, 200, 50), inventory[i].icon);
			if (selected == i) GUI.Label(new Rect(20+(70*i), 20, 200, 50), selector);
		}
	}
}
