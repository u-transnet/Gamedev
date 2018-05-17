using UnityEngine;

namespace uTrans.Components
{

    public class Id : MonoBehaviour
    {
        [SerializeField]
        private int _value;

        public int Value
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