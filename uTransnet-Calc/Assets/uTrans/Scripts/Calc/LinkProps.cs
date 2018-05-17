using UnityEngine;

public enum LinkType
{
    Default
}

public class LinkProps : MonoBehaviour
{

    [SerializeField]
    LinkType linkType;
}