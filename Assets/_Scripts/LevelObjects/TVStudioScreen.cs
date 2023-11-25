using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TVStudioScreen : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] private GameObject _spriteRendererPrefab;
    [SerializeField] private float _speed;
    private List<GameObject> _spriteRenderersObjects = new List<GameObject>();
    private int _currentSpriteIndex;
    private float _lastFrameChangeTime;
    
    private const float FrameSize = 2.56f;

    private void Start()
    {
        SetupSprites();
    }
    
    private void SetupSprites()
    {
        for(int i = 0; i < _sprites.Count; i++)
        {
            var spriteRenderer = Instantiate(_spriteRendererPrefab, transform);
            spriteRenderer.GetComponent<SpriteRenderer>().sprite = _sprites[i];
            spriteRenderer.transform.localPosition = new Vector3(FrameSize * i, 0, 0);
            _spriteRenderersObjects.Add(spriteRenderer);
        }
    }
        
    private void Update()
    {
        SpriteChangeCycle();
    }

    private void SpriteChangeCycle()
    {
        foreach (var spriteRenderersObject in _spriteRenderersObjects )
        {
            MoveRenderer(spriteRenderersObject, _speed);
            RendererReachedEnd(spriteRenderersObject);
        }
    }

    private void MoveRenderer(GameObject rendererGameObject, float speed)
    {
        var position = rendererGameObject.transform.localPosition;
        position = new Vector3(position.x - Time.deltaTime * speed, position.y, position.z);
        rendererGameObject.transform.localPosition = position;
    }

    private void RendererReachedEnd(GameObject rendererGameObject)
    {
        if (rendererGameObject.transform.localPosition.x < -FrameSize)
        {
            rendererGameObject.transform.localPosition = new Vector3(FrameSize * (_sprites.Count - 1), 0, 0);
        }
    }
}
