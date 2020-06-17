using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    private float _speed = -8.0f;

    private Player _player;
    private AudioManager _audioManager;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player in EnemyLaser is NULL.");
        }

        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();
        if (_audioManager == null)
        {
            Debug.LogError("The Audio Manager in EnemyLaser is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 1, 0) * _speed * Time.deltaTime);

        if (transform.position.y <= -5)
        {
            Destroy(this.gameObject);
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_player != null)
            {
                _player.Damage();                
            }

            _audioManager.PlayExplosionSound();
            Destroy(this.gameObject);
        }
    }
}
