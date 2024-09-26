using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    public float BulletSpeed { get; private set; }
    public float BulletDamage { get; private set; }
    public float BulletLifetime { get; set; } = 5f;
    GameObject _bulletOnHitEffect;
    SpriteRenderer _spriteRenderer;
    BoxCollider2D _boxCollider2D;

    Rigidbody2D _rb;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        if (_bulletOnHitEffect != null) _bulletOnHitEffect.SetActive(false);
        if (_boxCollider2D != null) _boxCollider2D.enabled = true;
        if (_spriteRenderer != null) _spriteRenderer.enabled = true;
        if (_rb != null)
        {
            _rb.simulated = true;
            _rb.velocity = Vector2.zero;
        }

    }

    public void Initialize(float speed, float damage, float lifetime, Vector3 direction)
    {
        BulletSpeed = speed;
        BulletDamage = damage;
        BulletLifetime = lifetime;

        _rb.velocity = direction.normalized * BulletSpeed;

        if (BulletLifetime > 0)
        {
            Invoke(nameof(Deactivate), BulletLifetime);
        }
    }

    private void Deactivate()
    {
        _rb.velocity = Vector2.zero;
        _rb.simulated = false;
        if (_boxCollider2D != null) _boxCollider2D.enabled = false;
        if (_spriteRenderer != null) _spriteRenderer.enabled = false;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("EnemyDestroyable") || other.CompareTag("CrimsonFleet") || other.CompareTag("ThraxArmada") || other.CompareTag("Syndicates"))
        {

            StartCoroutine(BulletOnHitEffect());
            other.GetComponent<IDamageable>()?.TakeDamage(BulletDamage);

        }
    }

    IEnumerator BulletOnHitEffect()
    {
        // Get screen position from world position
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        // Spawn damage text and convert its position to canvas space
        GameObject damageText = ObjectPooler.Instance.SpawnFromPool("DamageText", transform.position, Quaternion.identity);
        Debug.Log("Damage text spawned");

        // Set the parent to the world canvas and convert the screen point to the canvas's world position
        damageText.transform.SetParent(UIManager.Instance.worldCanvas.transform, false);

        // Convert screen position to local position in the canvas
        RectTransform canvasRect = UIManager.Instance.worldCanvas.GetComponent<RectTransform>();
        RectTransform damageTextRect = damageText.GetComponent<RectTransform>();

        Vector2 localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPosition, Camera.main, out localPosition);
        damageTextRect.localPosition = localPosition;

        Debug.Log("Damage text parented and positioned");

        // Set damage text content
        damageText.GetComponent<TextMeshProUGUI>().text = Mathf.Round(BulletDamage).ToString();

        _spriteRenderer.enabled = false;
        _boxCollider2D.enabled = false;
        _bulletOnHitEffect = ObjectPooler.Instance.SpawnFromPool("BulletHitEffect", transform.position, Quaternion.identity);

        yield return StartCoroutine(MoveUpAndFadeOut(damageText, 1f));

        _bulletOnHitEffect.SetActive(false);
        Deactivate();
    }

    IEnumerator MoveUpAndFadeOut(GameObject damageTextObject, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            damageTextObject.transform.position += Vector3.up * Time.deltaTime * 5f;
            damageTextObject.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1 - time / duration);
            yield return null;
        }
        damageTextObject.SetActive(false);
    }



}

