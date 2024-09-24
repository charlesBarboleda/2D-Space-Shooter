using UnityEngine;
using UnityEngine.Analytics;

public class ShieldBeamController : MonoBehaviour
{
    [SerializeField] Transform originPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] ParticleSystem _hitEffectInstance;
    [SerializeField] float ignoreDistanceThreshold = 5f;
    LineRenderer _lineRenderer;

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _layerMask = LayerMask.GetMask("BeamRaycast");
    }

    void Update()
    {
        if (originPoint != null && endPoint != null)
        {
            Vector3 direction = (endPoint.position - originPoint.position).normalized;
            float distance = Vector3.Distance(originPoint.position, endPoint.position);

            // Perform raycast to detect any colliders in the path of the beam
            RaycastHit2D hit = Physics2D.Raycast(originPoint.position, direction, distance, _layerMask);

            // Default to extending to the end point
            _lineRenderer.SetPosition(0, originPoint.position);
            _lineRenderer.SetPosition(1, endPoint.position);

            // Check if we hit something
            if (hit.collider != null)
            {
                float hitDistance = Vector2.Distance(originPoint.position, hit.point);

                // Ignore the collision if it's too close (within the threshold)
                if (hitDistance >= ignoreDistanceThreshold)
                {
                    // Set the end of the line to the hit point if it's a valid hit
                    _lineRenderer.SetPosition(1, hit.point);

                    // Play the hit effect at the collision point
                    if (_hitEffectInstance != null)
                    {
                        _hitEffectInstance.transform.position = hit.point;
                        _hitEffectInstance.Play();
                    }
                }
                else
                {
                    // Disable the hit effect if the hit is ignored
                    if (_hitEffectInstance != null)
                    {
                        _hitEffectInstance.Stop();
                    }
                }
            }
            else
            {
                // Disable the hit effect if there's no collision
                if (_hitEffectInstance != null)
                {
                    _hitEffectInstance.Stop();
                }
            }
        }
    }


    public Transform OriginPoint { get => originPoint; set => originPoint = value; }
    public Transform EndPoint { get => endPoint; set => endPoint = value; }
}
