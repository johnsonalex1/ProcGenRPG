using UnityEngine;
using System.Collections.Generic;

/**
 * A small class intended to hold a complete tileset for a specific generator
 */
public class TileSet : MonoBehaviour {
	public string generatorName;//the generator associated with the tileset
	public List<Tile> tiles;
	public List<Enemy> enemies;
}
