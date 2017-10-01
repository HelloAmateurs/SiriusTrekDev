using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {

	public GameObject poi; // player ship
	public GameObject[] panels; // scrolling foregrounds
	public float scrollSpeed = 100;
	public float motionMult = 0.25f; // controls how much panels react to player movement
	private float panelHt; // height of each panel
	private float depth; // depth of panels, z


	void Start () {
		panelHt = panels[0].transform.localScale.y;
		depth = panels[0].transform.position.z;

		// set initial positions of panels
		panels[0].transform.position = new Vector3(0,0,depth);
		panels[1].transform.position = new Vector3(0, panelHt, depth);
	}
	
	// Update is called once per frame
	void Update () {
		float tY, tX = 0;
		tY = Time.time * scrollSpeed % panelHt + (panelHt*0.5f);

		if (poi != null) {
			tX = -poi.transform.position.x * motionMult;
		}

		// Position panels[0]
		panels[0].transform.position = new Vector3(tX, tY, depth);
		// then position panels[1] where needed to make continuous starfield
		if (tY >=0) {
			panels[1].transform.position = new Vector3 (tX, tY-panelHt, depth);
		} else {
			panels[1].transform.position = new Vector3 (tX, tY+panelHt, depth);
		}
	}
}
