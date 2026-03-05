using UnityEngine;

public abstract class Bubbles : MonoBehaviour
{
    //classe madre per far spinnare tutte le bolle!
    public virtual void FixedUpdate()
    {
        SpinBubble();
    }

    private void SpinBubble()
    {
        transform.Rotate( Vector3.down * 90 * Time.fixedDeltaTime);
    }
}
