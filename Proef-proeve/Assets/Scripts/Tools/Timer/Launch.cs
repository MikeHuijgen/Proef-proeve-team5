using UnityEngine;

public class Launch : MonoBehaviour
{
    [SerializeField] float speed;

    [SerializeField] Transform startLocation;
    [SerializeField] Transform endLocation;
    [SerializeField] GameObject canon;

    [SerializeField] float travelTime;

    float planetDistance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        planetDistance = Vector3.Distance(startLocation.position, endLocation.position);
    }

    // Update is called once per frame
    void Update()
    {
        travelTime = speed * Time.deltaTime;

    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject == canon)
        {
            float journey = planetDistance / travelTime;
            transform.position = Vector3.Lerp(startLocation.position, endLocation.position, journey);
            Debug.Log("travel");
        }
    }
}
