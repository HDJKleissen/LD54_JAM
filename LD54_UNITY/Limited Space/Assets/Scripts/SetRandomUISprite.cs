using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetRandomUISprite : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private Image image;
    // Start is called before the first frame update
    void Start()
    {
        image.sprite = _sprites[Random.Range(0, _sprites.Length)];
    }

    // Update is called once per frame
    void Update()
    {

    }
}
