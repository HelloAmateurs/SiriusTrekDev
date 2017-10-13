using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Main : MonoBehaviour
{
    static public Main S;
    private GameObject currentLevel;
    public GameObject level01;
    public GameObject level02;
    public GameObject prefabPlanet;
    GameObject currentPlanet;
	public GameObject prefabWasd;
	GameObject currentWasd;
	public GameObject prefabLogo;
	GameObject currentLogo;
	public GameObject prefabExplore;
	GameObject currentExplore;
	public GameObject prefabHazard;
	GameObject currentHazard;

	public GameObject[] prefabAsteroids; 

	public float asteroidSpawnPadding = 1.5f;
	public float asteroidSpawnRate;

    void Awake()
    {
        S = this;
        Utils.SetCameraBounds(this.GetComponent<Camera>());
		asteroidSpawnRate = 20;
		Invoke ("SpawnAsteroid", asteroidSpawnRate);
       
    }

	// this controls the order of the intro title and tutorial
	IEnumerator OliverRoutine()
	{
		Spawn(prefabLogo, ref currentLogo, new Vector3(0, 50, -5));
		yield return new WaitForSeconds(4f);

		Spawn(prefabWasd, ref currentWasd, new Vector3(15, 50, -5));
		yield return new WaitForSeconds(8f);
		
		Spawn(prefabExplore, ref currentExplore, new Vector3(-50, 0, 1));
		yield return new WaitForSeconds(4f);

		Spawn(prefabHazard, ref currentHazard, new Vector3(-50, 15, 1));
		yield return new WaitForSeconds(10f);

		Spawn(prefabPlanet, ref currentPlanet, new Vector3(-15, 100, -5));

		Destroy(currentLogo);
		Destroy(currentWasd);
		Destroy(currentExplore);
		Destroy(currentHazard);
	}

    void Start()
    {
        currentLevel = Instantiate(level01);
		// intro and tutorial objects
		StartCoroutine(OliverRoutine());

    }

    public void loadLevel()
    {
        Destroy(currentLevel);
        Destroy(currentPlanet);
        currentLevel = Instantiate(level02);
    }

	// spawn is used for planets and intro text
	public void Spawn(GameObject prefab, ref GameObject instance, Vector3 pos)
	{
		instance = Instantiate(prefab);
		instance.transform.position = pos;
	}

	// spawns random asteroid from array within the cambounds
	public void SpawnAsteroid() 
	{
		if (currentLevel.name.Contains(level01.name)) 
		{
			int ndx = Random.Range(0, prefabAsteroids.Length);
			GameObject go = Instantiate(prefabAsteroids[ndx]) as GameObject;
			Vector3 pos = Vector3.zero;
			float xMin = Utils.camBounds.min.x + asteroidSpawnPadding;
			float xMax = Utils.camBounds.max.x - asteroidSpawnPadding;
			pos.x = Random.Range(xMin, xMax);
			pos.y = Utils.camBounds.max.y + asteroidSpawnPadding;
			go.transform.position = pos;
			go.transform.parent = currentLevel.transform;
			asteroidSpawnRate = 3;

			Invoke("SpawnAsteroid", asteroidSpawnRate);
		}
	}
}
