﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenPose.Example
{
    public class HumanController2D : MonoBehaviour {

        public int PoseKeypointsCount = 25;
        public int HandKeypointsCount = 21;
        public int FaceKeypointsCount = 70;
        public float ScoreThres = 0.2f;

        [SerializeField] RectTransform PoseParent;
        [SerializeField] RectTransform LHandParent;
        [SerializeField] RectTransform RHandParent;
        [SerializeField] RectTransform FaceParent;
        //[SerializeField] RectTransform LHandRectangle; // TODO
        //[SerializeField] RectTransform RHandRectangle; // TODO
        [SerializeField] RectTransform FaceRectangle;
        private List<RectTransform> poseJoints = new List<RectTransform>();
        private List<RectTransform> lHandJoints = new List<RectTransform>();
        private List<RectTransform> rHandJoints = new List<RectTransform>();
        private List<RectTransform> faceJoints = new List<RectTransform>();

        public void DrawHuman(ref OPDatum datum, int bodyIndex){
            DrawBody(ref datum, bodyIndex);
            DrawHand(ref datum, bodyIndex);
            DrawFace(ref datum, bodyIndex);
        }

        public void ClearHuman(){
            PoseParent.gameObject.SetActive(false);
            LHandParent.gameObject.SetActive(false);
            RHandParent.gameObject.SetActive(false);
            FaceParent.gameObject.SetActive(false);
        }

        private void DrawBody(ref OPDatum datum, int bodyIndex){
            if (datum.poseKeypoints == null || bodyIndex >= datum.poseKeypoints.GetSize(0)) {
                PoseParent.gameObject.SetActive(false);
                return;
            } else {
                PoseParent.gameObject.SetActive(true);
            }
            // Pose
            for (int part = 0; part < poseJoints.Count; part++) {
                // Joints overflow
                if (part >= datum.poseKeypoints.GetSize(1)) {
                    poseJoints[part].gameObject.SetActive(false);
                    continue;
                }
                // Compare score
                if (datum.poseKeypoints.Get(bodyIndex, part, 2) < ScoreThres) {
                    poseJoints[part].gameObject.SetActive(false);
                } else {
                    poseJoints[part].gameObject.SetActive(true);
                    Vector3 pos = new Vector3(datum.poseKeypoints.Get(bodyIndex, part, 0), datum.poseKeypoints.Get(bodyIndex, part, 1), 0f);
                    poseJoints[part].localPosition = pos;
                }
            }
        }

        private void DrawHand(ref OPDatum datum, int bodyIndex) {
            // Left
            if (datum.handKeypoints == null || bodyIndex >= datum.handKeypoints.left.GetSize(0)){
                LHandParent.gameObject.SetActive(false);
            } else {
                LHandParent.gameObject.SetActive(true);
                for (int part = 0; part < lHandJoints.Count; part++) {
                    // Joints overflow
                    if (part >= datum.handKeypoints.left.GetSize(1)) {
                        lHandJoints[part].gameObject.SetActive(false);
                        continue;
                    }
                    // Compare score
                    if (datum.handKeypoints.left.Get(bodyIndex, part, 2) < ScoreThres) {
                        lHandJoints[part].gameObject.SetActive(false);
                    } else {
                        lHandJoints[part].gameObject.SetActive(true);
                        Vector3 pos = new Vector3(datum.handKeypoints.left.Get(bodyIndex, part, 0), datum.handKeypoints.left.Get(bodyIndex, part, 1), 0f);
                        lHandJoints[part].localPosition = pos;
                    }
                }
            }
            // Right
            if (datum.handKeypoints == null || bodyIndex >= datum.handKeypoints.right.GetSize(0)){
                RHandParent.gameObject.SetActive(false);
            } else {
                RHandParent.gameObject.SetActive(true);
                for (int part = 0; part < rHandJoints.Count; part++) {
                    // Joints overflow
                    if (part >= datum.handKeypoints.right.GetSize(1)) {
                        rHandJoints[part].gameObject.SetActive(false);
                        continue;
                    }
                    // Compare score
                    if (datum.handKeypoints.right.Get(bodyIndex, part, 2) < ScoreThres) {
                        rHandJoints[part].gameObject.SetActive(false);
                    } else {
                        rHandJoints[part].gameObject.SetActive(true);
                        Vector3 pos = new Vector3(datum.handKeypoints.right.Get(bodyIndex, part, 0), datum.handKeypoints.right.Get(bodyIndex, part, 1), 0f);
                        rHandJoints[part].localPosition = pos;
                    }
                }
            }
        }

        private void DrawFace(ref OPDatum datum, int bodyIndex){            
            // Face
            if (datum.faceKeypoints == null || bodyIndex >= datum.faceKeypoints.GetSize(0)) {
                FaceParent.gameObject.SetActive(false);
            } else {
                FaceParent.gameObject.SetActive(true);

                for (int part = 0; part < faceJoints.Count; part++) {
                    // Joints overflow
                    if (part >= datum.faceKeypoints.GetSize(1)) {
                        faceJoints[part].gameObject.SetActive(false);
                        continue;
                    }
                    // Compare score
                    if (datum.faceKeypoints.Get(bodyIndex, part, 2) < ScoreThres) {
                        faceJoints[part].gameObject.SetActive(false);
                    } else {
                        faceJoints[part].gameObject.SetActive(true);
                        Vector3 pos = new Vector3(datum.faceKeypoints.Get(bodyIndex, part, 0), datum.faceKeypoints.Get(bodyIndex, part, 1), 0f);
                        faceJoints[part].localPosition = pos;
                    }
                }
            }           

            // Face rect
            if (datum.faceRectangles == null || bodyIndex >= datum.faceRectangles.Count){
                FaceRectangle.gameObject.SetActive(false);
            } else {
                FaceRectangle.gameObject.SetActive(true);
                FaceRectangle.localPosition = datum.faceRectangles[bodyIndex].center;
                FaceRectangle.sizeDelta = datum.faceRectangles[bodyIndex].size;
            }
        }
        
        // Use this for initialization
        void Start() {
            InitJoints();
        }

        private void InitJoints() {
            // Pose
            if (PoseParent) {
                Debug.Assert(PoseParent.childCount == PoseKeypointsCount, "Pose joint count not match");
                for (int i = 0; i < PoseKeypointsCount; i++) {
                    poseJoints.Add(PoseParent.GetChild(i) as RectTransform);
                }
            }
            // LHand
            if (LHandParent) {
                Debug.Assert(LHandParent.childCount == HandKeypointsCount, "LHand joint count not match");
                //LHandRectangle = LHandParent.GetChild(0) as RectTransform;
                for (int i = 0; i < HandKeypointsCount; i++) {
                    lHandJoints.Add(LHandParent.GetChild(i) as RectTransform);
                }
            }
            // RHand
            if (RHandParent) {
                Debug.Assert(RHandParent.childCount == HandKeypointsCount, "RHand joint count not match");
                //RHandRectangle = RHandParent.GetChild(0) as RectTransform;
                for (int i = 0; i < HandKeypointsCount; i++) {
                    rHandJoints.Add(RHandParent.GetChild(i) as RectTransform);
                }
            }
            // Face
            if (FaceParent){
                Debug.Assert(FaceParent.childCount == FaceKeypointsCount, "Face joint count not match");
                //FaceRectangle = FaceParent.GetChild(0) as RectTransform;
                for (int i = 0; i < FaceKeypointsCount; i++){
                    faceJoints.Add(FaceParent.GetChild(i) as RectTransform);
                }
            }
        }
    }
}

