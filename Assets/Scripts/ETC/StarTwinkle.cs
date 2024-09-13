using UnityEngine;

public class StarTwinkle : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    float _opacitySpeed = 1f;
    float _targetOpacity;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _targetOpacity = Random.Range(0.5f, 1f);
    }

    void Update()
    {
        float opacity = Mathf.PingPong(Time.time * _opacitySpeed, _targetOpacity);
        Color color = _spriteRenderer.color;
        color.a = opacity;
        _spriteRenderer.color = color;
    }
}
