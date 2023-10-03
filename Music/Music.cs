using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    AudioSource _source;

    bool _inIntro = true;

    [SerializeField] AudioClip _intro;
    [SerializeField] AudioClip _loop;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.clip = _intro;
        _source.Play();
        _source.loop = false;
    }

    private void Update()
    {
        if (_inIntro) 
        {
            if (!_source.isPlaying) 
            {
                _inIntro = false;
                _source.clip = _loop;
                _source.Play();
                _source.loop = true;
            }
        }
    }
}
