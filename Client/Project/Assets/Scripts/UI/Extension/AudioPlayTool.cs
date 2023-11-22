using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayTool : MonoBehaviour
{
    [SerializeField]
    public AudioClip clip;

    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = this.gameObject.AddComponent<AudioSource>();
        if (clip != null)
            source.clip = clip;
    }

    public void PlayClick()
    {
        if (clip)
        {
            source.Play();
        }
    }

}
