using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    float _canFire = -1f;

    [SerializeField]
    private GameObject _laserPrefab = null;

    private Player _player;
    private Animator _explosionAnim;

    private AudioManager _audioManager;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player is NULL.");
        }

        _explosionAnim = GetComponent<Animator>();

        if (_explosionAnim == null)
        {
            Debug.LogError("Explosion Animation is NULL.");
        }

        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();

        if (_audioManager == null)
        {
            Debug.LogError("The Enemy AudioManager is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
            FireEnemyLaser();
        }        
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5f && this.GetComponent<Collider2D>() != null)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
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

            Destroy(this.GetComponent<Collider2D>());
            _explosionAnim.SetTrigger("OnEnemyDeath");
            Destroy(this.gameObject, 2.8f);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(10);
            }
            Destroy(this.GetComponent<Collider2D>());
            _explosionAnim.SetTrigger("OnEnemyDeath");
            Destroy(this.gameObject, 2.8f);
        }

        _audioManager.PlayExplosionSound();
    }

    private void FireEnemyLaser()
    {
        _canFire = Time.time + Random.Range(3, 8);

        Instantiate(_laserPrefab, transform.position + new Vector3(0.18f, -1.4f, 0), Quaternion.identity);
        Instantiate(_laserPrefab, transform.position + new Vector3(-0.18f, -1.4f, 0), Quaternion.identity);

        _audioManager.PlayLaserSound();
    }
}
