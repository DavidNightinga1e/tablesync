using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private const float Speed = 600f;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(transform.forward * Speed, ForceMode.Force);
    }

    private void OnCollisionEnter(Collision other)
    {
        var otherGameObject = other.gameObject;
        if (otherGameObject.layer == LayerMask.NameToLayer("PlayerHitCollider"))
        {
            var playerBehaviour = otherGameObject.GetComponentInParent<PlayerBehaviour>();
            playerBehaviour.Hit();
        }
        Destroy(gameObject);
    }
}
