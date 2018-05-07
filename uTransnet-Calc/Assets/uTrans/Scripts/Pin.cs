using UnityEngine;

public class Pin : MonoBehaviour
{
    [SerializeField]
    GameObject target;

    float scaleAtBeginning;

    void Start() {
        scaleAtBeginning = transform.localPosition.z;
    }


    // Update is called once per frame
    void Update()
    {
        var parentScale = target.transform.localScale;
        transform.localScale = new Vector3(1 / parentScale.x, 1 / parentScale.y, transform.localScale.z);

        float height = GetComponent<SpriteRenderer>().bounds.size.z;
        var originPosition = transform.localPosition;
        transform.localPosition = new Vector3(originPosition.x, originPosition.y, 12 / parentScale.x);
    }
}
