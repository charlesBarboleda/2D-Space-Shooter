using UnityEngine;

public class ShieldBeamController : MonoBehaviour
{
    [SerializeField] Transform originPoint;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] ParticleSystem _hitEffectInstanceEnd;
    [SerializeField] ParticleSystem _hitEffectInstanceStart;

    [SerializeField] float ignoreDistanceThreshold = 5f;

    private GameObject _target;
    private LineRenderer _lineRenderer;

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _layerMask = LayerMask.GetMask("BeamRaycast");
    }

    void Update()
    {
        if (originPoint != null && _target != null)
        {
            Vector3 direction = (_target.transform.position - originPoint.position).normalized;
            float distance = Vector3.Distance(originPoint.position, _target.transform.position);

            // Perform raycast to detect any colliders in the path of the beam
            RaycastHit2D hit = Physics2D.Raycast(originPoint.position, direction, distance, _layerMask);

            _lineRenderer.SetPosition(0, originPoint.position);
            _lineRenderer.SetPosition(1, _target.transform.position);
            Debug.Log("Beam positions set");

            if (hit.collider != null)
            {
                Debug.Log("Hit detected " + hit.collider.name);
                float hitDistance = Vector2.Distance(originPoint.position, hit.point);
                if (hitDistance >= ignoreDistanceThreshold)
                {
                    _lineRenderer.SetPosition(1, hit.point);
                    Debug.Log("Hit distance greater than threshold");
                    if (_hitEffectInstanceStart != null)
                    {
                        Debug.Log("Playing hit effect start");
                        _hitEffectInstanceStart.transform.position = originPoint.position;
                        _hitEffectInstanceStart.Play();
                        Debug.Log("Hit Effect Played start");
                    }
                    if (_hitEffectInstanceEnd != null)
                    {
                        Debug.Log("Playing hit effect");
                        _hitEffectInstanceEnd.transform.position = hit.point;
                        _hitEffectInstanceEnd.Play();
                        Debug.Log("Hit Effect Played");
                    }
                }
            }
        }
    }

    public Transform OriginPoint { get => originPoint; set => originPoint = value; }
    public GameObject Target { get => _target; set => _target = value; }
}
