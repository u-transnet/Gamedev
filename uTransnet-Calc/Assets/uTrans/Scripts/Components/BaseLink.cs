using System;
using Mapbox.Utils;
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

        void Awake()
        {
            stretchy.OnTargetChange += (oldTransform, newTransform) => {
                if(oldTransform != null)
                {
                    oldTransform.GetComponent<BasePoint>().onMapObject.OnLocationUpdate -= UpdateTextPosition;
                }
                if(newTransform != null)
                {
                    newTransform.GetComponent<BasePoint>().onMapObject.OnLocationUpdate += UpdateTextPosition;
                }
            };
        }

        private void UpdateTextPosition(Vector2d vector2d)
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
    }
}