using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;

    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Player") == true)
        {
            _spriteRenderer.color = new Color(0, 1, 1, 1);
        }

        transform.Translate(new Vector3(0, 1, 0) * _speed * Time.deltaTime);

        if (transform.position.y >= 8f)
        {
            Destroy(this.gameObject);
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
        }
        
    }
}
