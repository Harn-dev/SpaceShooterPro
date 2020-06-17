using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Player : MonoBehaviour
{
    //Private variables are written as _variable.
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab = null;
    [SerializeField]
    private GameObject _tripleShotPrefab = null;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isSpeedPowerupActive = false;
    [SerializeField]
    private bool _isShieldPowerupActive = false;

    [SerializeField]
    private GameObject _shieldVisual = null;
    [SerializeField]
    private GameObject _right_Engine = null;
    [SerializeField]
    private GameObject _left_Engine = null;

    [SerializeField]
    private int _score = 0;

    private UIManager _uiManager;

    private AudioManager _audioManager;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, -4, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        if (_audioManager == null)
        {
            Debug.LogError("The Audio Manager is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);

        //restrain player movement along y axis. Hover over Clamp for def.
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        //Wrapping player from left or right to opposite side.
        if (transform.position.x >= 11.2 || transform.position.x <= -11.2)
        {
            transform.position = new Vector3(transform.position.x * -1, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        _audioManager.PlayLaserSound();
    }

    public void Damage()
    {
        if (_isShieldPowerupActive == true)
        {
            _isShieldPowerupActive = false;
            _shieldVisual.SetActive(false);
            return; //eat shield up, done.
        }

        _lives--;

        if (_lives == 2)
        {
            int randomEngineDamage = UnityEngine.Random.Range(0, 2);

            switch (randomEngineDamage)
            {
                case 0:
                    _right_Engine.SetActive(true);
                    break;
                case 1:
                    _left_Engine.SetActive(true);
                    break;
                default:
                    Debug.Log("Default Value on random engine damage. Oops?");
                    break;
            }
            
        }
        else if (_lives == 1)
        {
            _right_Engine.SetActive(true);
            _left_Engine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            _audioManager.PlayExplosionSound();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActivate()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    private IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;        
    }

    public void SpeedPowerupActivate()
    {
        _isSpeedPowerupActive = true;
        _speed *= 2;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    private IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _speed /= 2;
        _isSpeedPowerupActive = false;
    }

    public void ShieldPowerupActivate()
    {
        _isShieldPowerupActive = true; //lasts until hit
        _shieldVisual.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
