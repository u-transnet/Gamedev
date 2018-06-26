using System;
using System.Collections.Generic;
using UnityEngine;

namespace uTrans.Components
{
    public class BaseLink : BaseObject
    {
        [SerializeField]
        public LinkProps linkProps;


        [SerializeField]
        public StretchyTethered stretchy;

        public BasePoint FirstPoint
        {
            get
            {
                if(stretchy.FirstTarget != null)
                {
                    return stretchy.FirstTarget.GetComponent<BasePoint>();
                }
                else
                {
                    return null;
                }
            }
        }

        public BasePoint SecondPoint
        {
            get
            {
                if(stretchy.SecondTarget != null)
                {
                    return stretchy.SecondTarget.GetComponent<BasePoint>();
                }
                else
                {
                    return null;
                }

            }
        }

        private Dictionary<Transform, Action> updateTargetOffsetCallbacks = new Dictionary<Transform, Action>();

        void Awake()
        {
            stretchy.OnTargetChange += (index, oldTransform, newTransform) => {
                if(oldTransform != null)
                {
                    oldTransform.GetComponent<BasePoint>().onMapObject.OnLocationUpdate -= UpdateTextPosition;
                    oldTransform.GetComponent<BasePoint>().objectWithHeight.OnHeightChanged -= UpdateTextPosition;

                    Action tmp = updateTargetOffsetCallbacks[oldTransform];
                    oldTransform.GetComponent<BasePoint>().objectWithHeight.OnHeightChanged -= tmp;
                    updateTargetOffsetCallbacks.Remove(oldTransform);
                }
                if(newTransform != null)
                {
                    newTransform.GetComponent<BasePoint>().onMapObject.OnLocationUpdate += UpdateTextPosition;
                    newTransform.GetComponent<BasePoint>().objectWithHeight.OnHeightChanged += UpdateTextPosition;

                    Action tmp = () => {
                        UpdateOffset(index, newTransform);
                    };
                    updateTargetOffsetCallbacks.Add(newTransform, tmp);
                    newTransform.GetComponent<BasePoint>().objectWithHeight.OnHeightChanged += tmp;
                }
            };
        }

        private void UpdateTextPosition()
        {
            if(FirstPoint != null && SecondPoint != null)
            {
                var position1 = FirstPoint.transform.position;
                var position2 = SecondPoint.transform.position;
                var textPos = (position1 + position2) / 2 + new Vector3(0, 10, 0);
                debugText.transform.position = textPos;

                debugText.text = String.Format("Length: {0:0.0}m" +
                    "\nSlope: {1:0}%", linkProps.Length, linkProps.Slope);
            }
        }

        private void UpdateOffset(int index, Transform target)
        {
            float height = target.GetComponent<BasePoint>().objectWithHeight.UnityHeight;
            stretchy.targetWorldOffset[index] = new Vector3(0, height - 4, 0);

        }
    }
}