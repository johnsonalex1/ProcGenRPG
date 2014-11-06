﻿using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public class MapGenerator {

	private static readonly int MaxLength = 1000;
	private static List<System.Type> Generators;
	private static string generatorNameString;
	private Area a;

	/**
	 * Okay, this is fucking awesome, check this out
	 * We load the current assembly, i.e. all the classes in our unity project,
	 * and then iterate through and store any class that subclasses MapGenerator
	 * to get all the generators.  We can then get any generator that we have
	 * made simply by using the name of the class a a string!  Best of all its
	 * simple code which is easy to maintain!! So now, to make any new generator, we simply
	 * extend, and then this stuff here makes sure we can access it easily!
	 */
	public static MapGenerator getNewGeneratorByName(string generatorName) {
		TileSet tiles = World.getTileSetByName(generatorName);
		string gName = generatorName.Replace(" ", "").ToLower();
	
		int count = 0;
		System.Type type = null;
		foreach(System.Type a in Generators) {
			if(a.ToString().ToLower().Equals(gName))
				type = a;
			count++;
		}

		if(type == null || tiles == null)
			throw new NoSuchMapGeneratorException(generatorName, generatorNameString);

		ConstructorInfo constructor = type.GetConstructor(new System.Type[0]);
		MapGenerator generator = (MapGenerator)constructor.Invoke(new System.Type[0]);
		generator.setTiles(tiles.tiles);
		generator.setEnemies(tiles.enemies);
		return generator;
	}

	public void SetArea(Area a) {
		this.a = a;
	}

	protected void SpawnEnemy(int enemyType, float x, float y) {
		GameObject.Instantiate(enemies[enemyType], new Vector3(x, 0.5f, y), Quaternion.identity);
	}
	
	//static constructor, this gets all classes in the assembly that are children of MapGenerator
	static MapGenerator() {
		generatorNameString = "";
		System.Type type = typeof(MapGenerator);
		Generators = new List<System.Type>();
		foreach(System.Type a in type.Assembly.GetExportedTypes()) {
			if(a.IsSubclassOf(type)) {
				Generators.Add(a);
				generatorNameString += a.ToString() + ", ";
			}
		}
		if(generatorNameString.EndsWith(", "))
			generatorNameString = generatorNameString.Substring(0,generatorNameString.Length - 2);
	}

	
	public List<Tile> tiles;
	
	public List<Enemy> enemies;

	protected List<Tile> spawnedTiles;

	protected List<Tile> spawnedWalls;

	/**
	 * Constructor is not used in order to make subclasses of MapGenerator exceedingly simply and clean to implement
	 */
	public void setTiles(List<Tile> tiles) {
		this.tiles = tiles;
	}

	public void setEnemies(List<Enemy> enemies) {
		this.enemies = enemies;
	}

	public void InitWithData(AreaData data) {
		if(data.length == -1)
			data.length = Random.Range(100, MaxLength);
		Random.seed = data.seed;
		Init(data.length);
	}

	public void Init(int length) {
		//seed = Random.seed;

		spawnedTiles = new List<Tile>();
		spawnedWalls = new List<Tile>();
		generateGround(length);
		foreach(Tile t in spawnedTiles) {
			t.Init();
		}
		generateStructures(spawnedTiles, a.HasUp(), a.HasDown(), a.HasRight(), a.HasLeft());
	}

	/**
	 * Overload in a subclass in order to generate basic terrain
	 */
	protected virtual void generateGround(int length) {}

	/**
	 * Overload in a subclass in order to generate walls
	 * and other structures which rely on having already generated
	 * terrain
	 */
	protected virtual void generateStructures(List<Tile> ground, bool up, bool down, bool right, bool left) {}

	/**
	 * Returns the current amount of spawned tiles
	 */
	protected int tileCount() {
		return spawnedTiles.Count;
	}

	protected bool TileExists(float x, float z) {
		foreach(Tile t in spawnedTiles) {
			if(Mathf.Abs(t.X - x) < 2 && Mathf.Abs(t.Z - z) < 2) {
				return true;
			}
		}
		foreach(Tile t in spawnedWalls) {
			if(Mathf.Abs(t.X - x) < 2 && Mathf.Abs(t.Z - z) < 2) {
				return true;
			}
		}
		return false;
	}
	
	protected bool SpawnTile(float x, float z, int type) {
		foreach(Tile t in spawnedTiles) {
			t.Init();
		}
		foreach(Tile t in spawnedWalls) {
			t.Init();
		}
		if(!TileExists(x, z)) {
			if(type == 0) {
				spawnedTiles.Add((Tile)GameObject.Instantiate(tiles[type], new Vector3(x, 0, z), Quaternion.identity));
			}  else if (type == 1) {
				spawnedWalls.Add((Tile)GameObject.Instantiate(tiles[type], new Vector3(x, 3.4f, z), Quaternion.identity));
			}  else {
				spawnedWalls.Add((Tile)GameObject.Instantiate(tiles[type], new Vector3(x, 0, z), Quaternion.identity));
			}
		}  else {
			return false;	
		}
		return true;
		
	}
	
	protected bool ForceTile(float x, float z, int type) {
		foreach(Tile t in spawnedTiles) {
			t.Init();
		}
		foreach(Tile t in spawnedWalls) {
			t.Init();
		}

		if(type == 0) {
			spawnedTiles.Add((Tile)GameObject.Instantiate(tiles[type], new Vector3(x, 0, z), Quaternion.identity));
		}  else if (type == 1) {
			spawnedWalls.Add((Tile)GameObject.Instantiate(tiles[type], new Vector3(x, 3.4f, z), Quaternion.identity));
		}  else {
			GameObject[] items = GameObject.FindGameObjectsWithTag("Wall");
			for (int i = 0; i < items.Length; i++) {
				if(items[i].transform.position.x == x && items[i].transform.position.z == z) {
					GameObject.Destroy(items[i]);
				}
			}
			spawnedWalls.Add((Tile)GameObject.Instantiate(tiles[type], new Vector3(x, 3.4f, z), Quaternion.identity));
		}

		return true;
		
	}

	public void Clear() {
		GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
		GameObject[] grounds = GameObject.FindGameObjectsWithTag("Ground");
		GameObject[] portals = GameObject.FindGameObjectsWithTag("Portal");
		GameObject[] items = new GameObject[walls.Length + grounds.Length + portals.Length];
		walls.CopyTo(items, 0);
		grounds.CopyTo(items, walls.Length);
		portals.CopyTo(items, walls.Length + grounds.Length);
		for (int i = 0; i < items.Length; i++) {
			GameObject.Destroy(items[i]);
		}

	}
	
	
}

/**
 * Throw this exception in cases where a map generator type is requested, but the requested name type does not exist
 */
class NoSuchMapGeneratorException : System.Exception {
	private string invalidId;
	private string pNames;
	public override string Message{get{
			return "Invalid Generator Requested: " + invalidId + "\nAvailiable generators: " + pNames + "\nNames are NOT case sensitive";
		}}

	public NoSuchMapGeneratorException(string name, string possibleNames) : base(){
		invalidId = name;
		pNames = possibleNames;
	}
}
