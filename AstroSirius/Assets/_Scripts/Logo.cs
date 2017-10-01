using UnityEngine;
using System.Collections;

public class Logo : Planet
{
	public float scrollSpeed = 14f;

	void Update ()
	{
		Move();
	}

    public override void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= scrollSpeed * Time.deltaTime;
        pos = tempPos;
    }

}
