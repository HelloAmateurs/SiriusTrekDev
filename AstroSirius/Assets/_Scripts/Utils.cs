using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BoundsTest
{
    center,  // is the center of the gameobject on screen?
    onScreen,  // are the bounds entirely on screen?
    offScreen  // are the bounds entirely off screen?
}


public class Utils : MonoBehaviour
{

    // ====================== Bounds Functions =======================\\

    // creates bounds that encapsulate two bounds passed in

    public static Bounds BoundsUnion(Bounds b0, Bounds b1)
    {
        // if size of one of the bounds is vector3.zero, ignore
        if (b0.size == Vector3.zero && b1.size != Vector3.zero)
        {
            return (b1);
        }
        else if (b0.size != Vector3.zero && b1.size == Vector3.zero)
        {
            return (b0);
        }
        else if (b0.size == Vector3.zero && b1.size == Vector3.zero)
        {
            return (b0);
        }
        // stretch b0 to include b1.min and max
        b0.Encapsulate(b1.min);
        b0.Encapsulate(b1.max);
        return (b0);

    }

    public static Bounds CombineBoundsOfChildren(GameObject go)
    {
        // create an empty Bounds b
        Bounds b = new Bounds(Vector3.zero, Vector3.zero);
        // if this game object has a renderer
        if (go.GetComponent<Renderer>() != null)
        {
            // expand b to contain renderer's bounds
            b = BoundsUnion(b, go.GetComponent<Renderer>().bounds);
        }
        // if this gameobject has a collider componenet
        if (go.GetComponent<Collider>() != null)
        {
            // expand b to contain collider's bounds
            b = BoundsUnion(b, go.GetComponent<Collider>().bounds);
        }
        // recursively iterate through each child of gameobject transform
        foreach (Transform t in go.transform)
        {
            // expand b to contain their bounds also
            b = BoundsUnion(b, CombineBoundsOfChildren(t.gameObject));
        }
        return (b);

    }

    // make a static read-only public property camBounds
    static public Bounds camBounds
    {
        get
        {
            // if _camBounds hasn't ben set yet
            if (_camBounds.size == Vector3.zero)
            {
                // setcamerabounds using defaut camera
                SetCameraBounds();
            }
            return (_camBounds);
        }
    }

    // this is the private static field that camBounds uses
    static private Bounds _camBounds;

    // this function is used by camboudns to set _cambounds and can be 
    // called directly
    public static void SetCameraBounds(Camera cam = null)
    {
        // if no camera passed in, use main camera
        if (cam == null) cam = Camera.main;
        // this assumes camera is orthographic and at rotation R:[0,0,0,]

        // make vector3s at topleft and bottom right of screen cords
        Vector3 topLeft = new Vector3(0, 0, 0);
        Vector3 bottomRight = new Vector3(Screen.width, Screen.height, 0);

        // convert these to world coordinates
        Vector3 boundTLN = cam.ScreenToWorldPoint(topLeft);
        Vector3 boundBRF = cam.ScreenToWorldPoint(bottomRight);

        // adjust zs to be at near and far camera clipping planes
        boundTLN.z += cam.nearClipPlane;
        boundBRF.z += cam.farClipPlane;

        // find center of the bounds
        Vector3 center = (boundTLN + boundBRF) / 2f;
        _camBounds = new Bounds(center, Vector3.zero);
        // expand _cambounds to encapsulate the extents
        _camBounds.Encapsulate(boundTLN);
        _camBounds.Encapsulate(boundBRF);
    }

    // checks to see whether bounds bnd are within cambounds
    public static Vector3 ScreenBoundsCheck(Bounds bnd, BoundsTest test = BoundsTest.center)
    {
        return (BoundsInBoundsCheck(camBounds, bnd, test));
    }

    // checks to see whether bounds lilb are within bounds bigb
    public static Vector3 BoundsInBoundsCheck(Bounds bigB, Bounds lilB, BoundsTest test = BoundsTest.onScreen)
    {
        // behavior of this fn differs based on selected boundtest

        // get of lilb
        Vector3 pos = lilB.center;
        // initialize offset at 000
        Vector3 off = Vector3.zero;

        switch (test)
        {
            // center test determines what offset is needed to move lilb center inside bigb
            case BoundsTest.center:
                if (bigB.Contains(pos))
                {
                    return (Vector3.zero);
                }
                if (pos.x > bigB.max.x)
                {
                    off.x = pos.x - bigB.max.x;
                }
                else if (pos.x < bigB.min.x)
                {
                    off.x = pos.x - bigB.min.x;
                }
                if (pos.y > bigB.max.y)
                {
                    off.y = pos.y - bigB.max.y;
                }
                else if (pos.y < bigB.min.y)
                {
                    off.y = pos.y - bigB.min.y;
                }
                if (pos.z > bigB.max.z)
                {
                    off.z = pos.z - bigB.max.z;
                }
                else if (pos.z < bigB.min.z)
                {
                    off.z = pos.z - bigB.min.z;
                }
                return (off);


            // onscreen test determines offset to keep all lilb in bigb
            case BoundsTest.onScreen:
                if (bigB.Contains(lilB.min) && bigB.Contains(lilB.max))
                {
                    return (Vector3.zero);
                }

                if (lilB.max.x > bigB.max.x)
                {
                    off.x = lilB.max.x - bigB.max.x;
                }
                else if (lilB.min.x < bigB.min.x)
                {
                    off.x = lilB.min.x - bigB.min.x;
                }
                if (lilB.max.y > bigB.max.y)
                {
                    off.y = lilB.max.y - bigB.max.y;
                }
                else if (lilB.min.y < bigB.min.y)
                {
                    off.y = lilB.min.y - bigB.min.y;
                }
                if (lilB.max.z > bigB.max.z)
                {
                    off.z = lilB.max.z - bigB.max.z;
                }
                else if (lilB.min.z < bigB.min.z)
                {
                    off.z = lilB.min.z - bigB.min.z;
                }
                return (off);

            // offscreen test determines offset to move any tiny part of lilb into bigb
            case BoundsTest.offScreen:
                bool cMin = bigB.Contains(lilB.min);
                bool cMax = bigB.Contains(lilB.max);
                if (cMin || cMax)
                {
                    return (Vector3.zero);
                }
                if (lilB.min.x > bigB.max.x)
                {
                    off.x = lilB.min.x - bigB.max.x;
                }
                else if (lilB.max.x < bigB.min.x)
                {
                    off.x = lilB.max.x - bigB.min.x;
                }
                if (lilB.min.y > bigB.max.y)
                {
                    off.y = lilB.min.y - bigB.max.y;
                }
                else if (lilB.max.y < bigB.min.y)
                {
                    off.y = lilB.max.y - bigB.min.y;
                }
                if (lilB.min.z > bigB.max.z)
                {
                    off.z = lilB.min.z - bigB.max.z;
                }
                else if (lilB.max.z < bigB.min.z)
                {
                    off.z = lilB.max.z - bigB.min.z;
                }
                return (off);
        }
        return (Vector3.zero);
    }

    // ====================== Transform Functions =======================\\

    // This fn iteratively climbs transform.parent tree until finding
    // parent with a tag or no parent

    public static GameObject FindTaggedParent(GameObject go)
    {
        if (go.tag != "Untagged")
        {  // if gameobject has a tag, return it
            return (go);
        }
        if (go.transform.parent == null)
        { //we are at top of hierarchy and no tags
            return (null);
        }
        return (FindTaggedParent(go.transform.parent.gameObject));
    }

    // this version handles things if a transform is passed in
    public static GameObject FindTaggedParent(Transform t)
    {
        return (FindTaggedParent(t.gameObject));
    }




    // ====================== Transform Functions =======================\\

    // returns list of all materials on this gameobject or children
    static public Material[] GetAllMaterials(GameObject go)
    {
        List<Material> mats = new List<Material>();
        if (go.GetComponent<Renderer>() != null)
        {
            mats.Add(go.GetComponent<Renderer>().material);
        }
        foreach (Transform t in go.transform)
        {
            mats.AddRange(GetAllMaterials(t.gameObject));
        }
        return (mats.ToArray());
    }


}
