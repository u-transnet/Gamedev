using UnityEngine;

public enum PointType
{
    Default
}

public class PointProps : MonoBehaviour
{

    [SerializeField]
    PointType pointType;
}