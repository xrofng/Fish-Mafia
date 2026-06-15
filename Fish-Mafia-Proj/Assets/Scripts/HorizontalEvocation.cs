using UnityEngine;
using Combat2D;

public class HorizontalEvocation : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;
    public Vector3 DirectionalMultiplier = Vector3.one;
    private Vector2 _direction;

    [Header("Lifetime")]
    public float lifeTime = 3f;
    private float _timer;
    

    [Header("HitBox")]
    public HitBox hitBox;

    [Header("Visual")]
    public GameObject VisualGroup;
    private Vector3 _scaleHeadRight;
    private Vector3 _scaleHeadLeft;

    public void Init(Vector2 dir, PlayerCombat owner)
    {
        _scaleHeadRight = VisualGroup.transform.localScale;
        _scaleHeadLeft = _scaleHeadRight;
        _scaleHeadLeft.x = -_scaleHeadLeft.x;

        _direction = dir.normalized;
        _direction *= DirectionalMultiplier;

        VisualGroup.transform.localScale = _direction.x >= 0 ? _scaleHeadRight : _scaleHeadLeft;
    }

    private void Update()
    {
        transform.position += (Vector3)(_direction * speed * Time.deltaTime);

        _timer += Time.deltaTime;
        if (_timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
