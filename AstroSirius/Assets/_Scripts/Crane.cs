using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Crane : MonoBehaviour
{
    static public bool giraffeHit = false;
    public GameObject giraffeGO;
    public GameObject baseGO;
    bool canBeHooked = true;
    public Animator animationController;
    public Text giraffeMessage;

	public GameObject DiscoLights;
	public Animation DiscoPrepare;
	public ParticleSystem DiscoParticles;
	public GameObject DiscoCanvas;
	public AudioClip woohoo;
    public AudioClip snap;
    AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Giraffe" && canBeHooked == true)
        {
            Crane.giraffeHit = true;
            animationController.SetBool("giraffeHit", giraffeHit);
            Debug.Log("giraffeHit= " + Crane.giraffeHit);
            giraffeGO.transform.SetParent(this.gameObject.transform);
            audioSource.PlayOneShot(snap);
        }

        if (other.gameObject.tag == "Base" && giraffeGO.transform.parent.tag == "CraneWeight")
        {
            giraffeGO.transform.position = baseGO.transform.position;
            giraffeGO.transform.SetParent(this.transform.parent);
            giraffeGO.transform.localScale = new Vector3(6, 6, 6);
            giraffeGO.transform.position = new Vector3(-22.5f, 8.7f, -8);
            canBeHooked = false;

            WinGame();
        }
    }

    void WinGame()
    {
        giraffeMessage.text = "THANK YOU";
        audioSource.PlayOneShot(woohoo);
		DiscoPrepare.Play();
		DiscoLights.SetActive(true);
		DiscoParticles.gameObject.SetActive(true);
		DiscoParticles.Play();
		DiscoCanvas.gameObject.SetActive(true);
    }
}
