using UnityEngine;

public class chainGen : MonoBehaviour
{
    public GameObject chainLinkPrefab;
    public float chainLength = 0.1f;
    public int chainNumber = 10;
    void Start()
    {
        GenerateChain(chainNumber);
    }
    void GenerateChain(int chainNumber)
    {
        Vector3 previousLinkPosition = transform.position;
        Quaternion previousLinkRotation = transform.rotation;
        Rigidbody previousRigidbody = GetComponent<Rigidbody>();
        for (int i = 0; i < chainNumber; i++)
        {
            GameObject newLink = Instantiate(chainLinkPrefab, previousLinkPosition, previousLinkRotation);
            SpringJoint sj = newLink.GetComponent<SpringJoint>();
            newLink.transform.parent = transform.parent;

            if (previousRigidbody != null)
            {
                sj.connectedBody = previousRigidbody;
            }

            previousRigidbody = newLink.GetComponent<Rigidbody>();

            // Update position and rotation for the next link
            previousLinkPosition += -newLink.transform.up * chainLength;
            previousLinkRotation = newLink.transform.rotation;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
