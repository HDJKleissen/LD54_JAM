using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRandomSprite : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer.sprite = _sprites[Random.Range(0, _sprites.Length-1)];
    }

    // Update is called once per frame
    void Update()
    {

    }
}
