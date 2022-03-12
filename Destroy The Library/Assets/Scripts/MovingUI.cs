using UnityEngine;

public class MovingUI : MonoBehaviour
{
    [Tooltip("How far away it can move randomly")]
    [SerializeField] private int step;
    private new RectTransform transform;

    private void Awake()
    {
        transform = this.GetComponent<RectTransform>();
    }

    private void Update()
    {
        int movementX = Random.Range(-5, 5) * step;
        int movementY = Random.Range(-5, 5) * step;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(movementX, movementY, transform.position.z), Time.deltaTime * step);
    }
}
