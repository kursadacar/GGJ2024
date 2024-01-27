using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class CharacterAnimationController : MonoBehaviour
{
    #region Debug Properties

    [SerializeField]
    private Vector3 _velocityPerSecond;

    #endregion

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private Vector3 _lastPosition;
    private Vector3 currentPosition;
    private bool _isFlipped;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = transform.position;

        var velocity = currentPosition - _lastPosition;
        var velocityPerSecond = velocity * (1f / Time.deltaTime);
        _velocityPerSecond = velocityPerSecond;

        _animator.SetHorizontalVelocity(Mathf.Abs(velocityPerSecond.x));

        bool shouldFlipHorizontal = velocityPerSecond.x < 0 || (Mathf.Abs(velocityPerSecond.x) < 0.01f && _isFlipped);
        SetFlipped(shouldFlipHorizontal);
    }

    private void LateUpdate()
    {
        _lastPosition = currentPosition;
    }

    private void SetFlipped(bool isFlipped)
    {
        _isFlipped = isFlipped;

        var localScale = transform.localScale;
        localScale.x = isFlipped ? -1f : 1f;
        transform.localScale = localScale;
        _isFlipped = isFlipped;
    }
}
