using UnityEngine;
using System.Collections;

public class Sirius : MonoBehaviour
{
    static public Sirius S;
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 10;
    public Bounds bounds;
    public LineRenderer craneLine;
    public Transform craneWeightTransform;
    AudioSource audioSource;
    public AudioClip crash;
    public AudioClip meow;
    public AudioClip landing;

    bool isControllable = true;

    void Awake()
    {
        S = this;
        bounds = Utils.CombineBoundsOfChildren(this.gameObject);

    }

    void Start ()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (!isControllable)
        {
            return;
        }

        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;

        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;
        bounds.center = transform.position;

        Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.onScreen);
        if (off != Vector3.zero)
        {
            pos -= off;
            transform.position = pos;
        }
        transform.rotation = Quaternion.Euler(0, 0, xAxis * pitchMult);
        if (craneLine != null)
        {
            updateLine();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // changing levels when hitting planet from starfield
        print("Triggered: " + other.gameObject.name);
        if (other.gameObject.tag == "Planet")
        {
            audioSource.PlayOneShot(landing);
            isControllable = false;
            this.transform.parent = other.gameObject.transform;

            Invoke("loadLevel", 1.5f);
        }
        // crashing into asteriods
        if (other.gameObject.tag == "Asteroid")
        {
            StartCoroutine(soundRoutine());
            other.GetComponent<ParticleSystem>().Play();
            
        }
        
    }

    void loadLevel()
    {
        Main.S.loadLevel();
    }

    IEnumerator soundRoutine (){
        audioSource.PlayOneShot(crash);
        yield return new WaitForSeconds(.25f);
        audioSource.PlayOneShot(meow);
    }
    // the crane in desert world
    void updateLine()
    {
		craneLine.SetPosition(0, this.transform.position - new Vector3(0f, 7f, 0f));
		craneLine.SetPosition(1, craneWeightTransform.position);
    }
}
