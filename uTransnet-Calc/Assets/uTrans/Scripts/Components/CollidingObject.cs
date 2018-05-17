namespace uTrans.Components
{
    using UnityEngine;

    public class CollidingObject : MonoBehaviour
    {
        public Collider NodeCollider { get; private set; }

        public Renderer NodeRenderer { get; private set; }

        public string[] usedTags;
        public string[] ignoredTags;

        [SerializeField]
        public bool Colliding
        {
            get
            {
                return collisions > 0;
            }
        }

        /// <summary>
        /// Count of collisions
        /// </summary>
        private int collisions;

        private OnMapObject uObject;

        [SerializeField]
        Color defaultColor;


        // Use this for initialization
        void Awake()
        {
            uObject = GetComponent<OnMapObject>();
            NodeCollider = GetComponent<Collider>();
            NodeRenderer = GetComponent<Renderer>();
            //defaultColor = NodeRenderer.material.color;
        }

        // Update is called once per frame
        void Update()
        {
            if (Colliding)
            {
                NodeRenderer.material.color = new Color(0.8f, 0, 0, 0.5f);
            }
            else
            {
                NodeRenderer.material.color = new Color(0, 0.8f, 0, 0.5f);
            }
        }

        public void Disable()
        {
            NodeRenderer.material.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, NodeRenderer.material.color.a);
            enabled = false;
        }

        void OnCollisionEnter(Collision collision)
        {
            if (Check(collision.gameObject.tag))
            {
                collisions++;
            }
        }

        void OnCollisionExit(Collision collision)
        {
            if (Check(collision.gameObject.tag))
            {
                collisions--;
            }
        }

        void OnTriggerEnter(Collider collider)
        {
            if (Check(collider.gameObject.tag))
            {
                collisions++;
            }
        }

        void OnTriggerExit(Collider collider)
        {
            if (Check(collider.gameObject.tag))
            {
                collisions--;
            }
        }

        bool Check(string needle)
        {
            if (usedTags.Length > 0)
            {
                foreach (string str in usedTags)
                {
                    if (str == needle)
                    {
                        return true;
                    }
                }
                return false;
            }

            if (ignoredTags.Length > 0)
            {
                foreach (string str in ignoredTags)
                {
                    if (str == needle)
                    {
                        return false;
                    }
                }
                return true;
            }

            return true;
        }


    }
}