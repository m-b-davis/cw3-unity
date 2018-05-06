using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PickupAudio : MonoBehaviour {

    private AudioSource source;

    IEnumerator Start () {
        source = GetComponent<AudioSource>();
        source.Play();
        yield return new WaitForSeconds(source.clip.length);
        Destroy(gameObject);
    }

}
