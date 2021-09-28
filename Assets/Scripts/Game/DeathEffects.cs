using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffects : MonoBehaviour
{
    [SerializeField]
    private GameObject particles;
    [SerializeField]
    private AudioClip deathClip;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void PlayDeathEffects()
    {
        GameObject p = Instantiate(particles, transform);
        p.transform.position = transform.position;
        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
            audioSource.PlayOneShot(deathClip);
        StartCoroutine(Dispose(deathClip.length));
    }

    private IEnumerator Dispose(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
