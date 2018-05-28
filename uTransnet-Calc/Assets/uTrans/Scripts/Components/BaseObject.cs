using UnityEngine.UI;

namespace uTrans.Components
{
    using UnityEngine;

    public class BaseObject : MonoBehaviour
    {
        [SerializeField]
        public CollidingObject collidingObject;
        [SerializeField]
        public Renderer objectRenderer;
        [SerializeField]
        public Id id;
        [SerializeField]
        public Text debugText;

        [SerializeField]
        float alfa = 0.5f;

        [SerializeField]
        bool canBeSelected = true;

        public Project project;

        private bool _editable;

        [SerializeField]
        public bool Editable
        {
            get
            {
                return _editable;
            }
            set
            {
                _editable = value;
                Color oldColor = objectRenderer.material.color;
                objectRenderer.material.color = new Color(oldColor.r, oldColor.g, oldColor.b, _editable ? alfa : 1);
                if (!value)
                {
                    Active = false;
                }
            }
        }

        private bool _active;

        [SerializeField]
        virtual public bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;
                if (collidingObject != null)
                {
                    if (!value)
                    {
                        collidingObject.Disable();
                    }
                    else
                    {
                        collidingObject.enabled = value;
                    }
                }
            }
        }
    }
}
