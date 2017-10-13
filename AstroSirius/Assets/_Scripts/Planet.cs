using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour
{

    public float speed = 10f;
    public Bounds bounds;
    public Vector3 boundsCenterOffset;
    public Main mainRef;


    void Awake()
    {
        InvokeRepeating("CheckOffscreen", 0f, 2f);
    }
    void Update()
    {
        Move();
    }

    public float moveTimer = 0.0f;
    public float duration = 0.5f;
    // planet moves down screen in a wide cosine path
    public virtual void Move()
    {
        moveTimer += Time.deltaTime;
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        tempPos.x = Mathf.Cos(moveTimer / duration) * 10f;
        pos = tempPos;
    }

    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }

    protected virtual void CheckOffscreen()
    {
        if (bounds.size == Vector3.zero)
        {
            bounds = Utils.CombineBoundsOfChildren(this.gameObject);
            boundsCenterOffset = bounds.center - transform.position;
        }

        bounds.center = transform.position + boundsCenterOffset;
        Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.offScreen);
        if (off != Vector3.zero && this.tag == "Planet")
        {
            if (off.y < 0)
            {
                this.transform.position = new Vector3(0, 100, -5);
            }  
        }
    }
}
