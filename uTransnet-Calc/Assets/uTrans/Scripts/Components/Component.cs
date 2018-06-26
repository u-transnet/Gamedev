using UnityEngine;

namespace uTrans.Components
{
    public class Component : MonoBehaviour
    {
        [SerializeField]
        private BaseObject baseObject;

        public BaseObject BaseObject
        {
            get
            {
                return baseObject;
            }
        }
    }
}