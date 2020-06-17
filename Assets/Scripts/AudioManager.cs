using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _laserSound;
    [SerializeField]
    private AudioClip _explosionSound;
    [SerializeField]
    private AudioClip _powerupSound;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.Log("The Audio Source in AudioManager is NULL.");
        }
    }

    public void PlayLaserSound()
    {
        _audioSource.PlayOneShot(_laserSound);
    }

    public void PlayExplosionSound()
    {
        _audioSource.PlayOneShot(_explosionSound);
    }

    public void PlayPowerupSound()
    {
        _audioSource.PlayOneShot(_powerupSound);
    }
}
