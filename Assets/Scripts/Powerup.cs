using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField] //0 = Triple Shot 1 = Speed 2 = Shields
    private int powerupID;

    private AudioManager _audioManager;

    // Start is called before the first frame update
    void Start()
    {
        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();
        
        if (_audioManager == null)
        {
            Debug.LogError("The Audio Manager is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                _audioManager.PlayPowerupSound();

                switch (powerupID)
                {
                    case 0:
                        player.TripleShotActivate();
                        break;
                    case 1:
                        player.SpeedPowerupActivate();
                        break;
                    case 2:
                        player.ShieldPowerupActivate();
                        break;
                    default:
                        Debug.Log("Default Value on powerup spawn. Oops?");
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
