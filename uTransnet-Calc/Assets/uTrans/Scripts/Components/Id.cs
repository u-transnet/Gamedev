using UnityEngine;

namespace uTrans.Components
{

    public class Id : MonoBehaviour
    {
        [SerializeField]
        private long _value;

        public long Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
    }
}