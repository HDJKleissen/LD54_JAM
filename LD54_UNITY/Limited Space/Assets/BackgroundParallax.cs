using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    [SerializeField] Transform _background;
    [SerializeField] Transform _stars1;
    [SerializeField] Transform _stars2;
    [SerializeField] Transform _planet1;
    [SerializeField] Transform _planet2;

    [SerializeField] float _backgroundFactor;
    [SerializeField] float _stars1Factor;
    [SerializeField] float _stars2Factor;
    [SerializeField] float _planet1Factor;
    [SerializeField] float _planet2Factor;

    [SerializeField] Camera _camera;

    Vector2 _backgroundSize;
    Vector2 _stars1Size;
    Vector2 _stars2Size;
    Vector2 _planet1Size;
    Vector2 _planet2Size;

    Vector2 _backgroundStartPos;
    Vector2 _stars1StartPos;
    Vector2 _stars2StartPos;
    Vector2 _planet1StartPos;
    Vector2 _planet2StartPos;

    private void Awake()
    {
        if(_camera == null)
        {
            _camera = FindObjectOfType<Camera>();
        }
    }
    void Start()
    {
        _backgroundSize = _background.GetComponent<SpriteRenderer>().bounds.size;
        _stars1Size = _stars1.GetComponent<SpriteRenderer>().bounds.size;
        _stars2Size = _stars2.GetComponent<SpriteRenderer>().bounds.size;
        _planet1Size = _planet1.GetComponent<SpriteRenderer>().bounds.size;
        _planet2Size = _planet2.GetComponent<SpriteRenderer>().bounds.size;

        _backgroundStartPos = _background.transform.position;
        _stars1StartPos = _stars1.transform.position;
        _stars2StartPos = _stars2.transform.position;
        _planet1StartPos = _planet1.transform.position;
        _planet2StartPos = _planet2.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DoParallax(_background, _backgroundFactor, _backgroundSize, _backgroundStartPos);
        DoParallax(_stars1, _stars1Factor, _stars1Size, _stars1StartPos);
        DoParallax(_stars2, _stars2Factor, _stars2Size, _stars2StartPos);
        DoParallax(_planet1, _planet1Factor, _planet1Size, _planet1StartPos);
        DoParallax(_planet2, _planet2Factor, _planet2Size, _planet2StartPos);
    }

    void DoParallax(Transform layer, float factor, Vector2 layerSize, Vector2 startPos)
    {
        Vector3 cameraPos = _camera.transform.position;
        Vector2 temp = new Vector2(cameraPos.x * (1 - factor), cameraPos.y * (1 - factor));
        Vector2 distance = new Vector2(cameraPos.x * factor, cameraPos.y * factor);

        Vector3 newPosition = new Vector3(startPos.x + distance.x, startPos.y + distance.y, layer.position.z);

        layer.position = newPosition;

        if (temp.x > startPos.x + (layerSize.x / 2))
        {
            startPos += new Vector2(layerSize.x, 0);
        }
        else if (temp.x < startPos.x - (layerSize.x / 2))
        {
            startPos -= new Vector2(layerSize.x, 0);
        }

        if (temp.y > startPos.y + (layerSize.y / 2))
        {
            startPos += new Vector2(0,layerSize.y);
        }
        else if (temp.y < startPos.y - (layerSize.y / 2))
        {
            startPos -= new Vector2(0,layerSize.y);
        }
    }
}
