using System;
using UnityEngine;

namespace Assets
{
    public struct Vector3KeyFrame
    {
        public Vector3 point;
        public float time;
        public Vector3KeyFrame(float time, Vector3 point)
        {
            this.time = time;
            this.point = point;
        }
    };
    [Serializable]
    public class Vector3Curve
    {
        [SerializeField]
        private AnimationCurve X;
        [SerializeField]
        private AnimationCurve Y;
        [SerializeField]
        private AnimationCurve Z;

        public Vector3Curve(params Vector3KeyFrame[] keyFrames3D)
        {
            var keyframeX = new Keyframe[keyFrames3D.Length];
            var keyframeY = new Keyframe[keyFrames3D.Length];
            var keyframeZ = new Keyframe[keyFrames3D.Length];
            for (int i = 0; i < keyFrames3D.Length; i++)
            {
                keyframeX[i] = new Keyframe(keyFrames3D[i].time, keyFrames3D[i].point.x);
                keyframeY[i] = new Keyframe(keyFrames3D[i].time, keyFrames3D[i].point.y);
                keyframeZ[i] = new Keyframe(keyFrames3D[i].time, keyFrames3D[i].point.z);
            }
            MakeCurveLinear(keyframeX);
            MakeCurveLinear(keyframeY);
            MakeCurveLinear(keyframeZ);
            X = new AnimationCurve(keyframeX);
            Y = new AnimationCurve(keyframeY);
            Z = new AnimationCurve(keyframeZ);
        }

        private void MakeCurveLinear(Keyframe[] keyframes)
        {
            for (int i = 0; i < keyframes.Length; i++)
            {
                keyframes[i].tangentMode = 1;
                if (i < keyframes.Length - 1)
                {
                    keyframes[i].outTangent = (keyframes[i + 1].value - keyframes[i].value) / (keyframes[i + 1].time - keyframes[i].time);
                }
                if (i > 0)
                {
                    keyframes[i].inTangent = (keyframes[i].value - keyframes[i - 1].value) / (keyframes[i].time - keyframes[i - 1].time);
                }
            }
        }

        public Vector3 Evaluate(float time)
        {
            return new Vector3(X.Evaluate(time), Y.Evaluate(time), Z.Evaluate(time));
        }
    }
}
