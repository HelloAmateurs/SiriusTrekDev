using UnityEngine;
using System.Collections;

public class SidewaysText : Planet {
	public float scrollSpeed;

	 public override void Move()
    {
        Vector3 tempPos = pos;
        tempPos.x += scrollSpeed * Time.deltaTime;
        pos = tempPos;
    }
}
