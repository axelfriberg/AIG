﻿using System.Collections.Generic;
using Assets.Scripts.IAJ.Unity.Utils;
using RAIN.Navigation.Graph;
using UnityEngine;
using System;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.Path {
    public class GlobalPath : Path {
        public List<NavigationGraphNode> PathNodes { get; protected set; }
        public List<Vector3> PathPositions { get; protected set; }
        public bool IsPartial { get; set; }
        public float Length { get; set; }
        public List<LocalPath> LocalPaths { get; protected set; }


        public GlobalPath() {
            this.PathNodes = new List<NavigationGraphNode>();
            this.PathPositions = new List<Vector3>();
            this.LocalPaths = new List<LocalPath>();
        }

        public void CalculateLocalPathsFromPathPositions(Vector3 initialPosition) {
            Vector3 previousPosition = initialPosition;
            for (int i = 0; i < this.PathPositions.Count; i++) {
                var sqrDistance = (this.PathPositions[i] - previousPosition).sqrMagnitude;
                if (sqrDistance >= 2.0f) {
                    this.LocalPaths.Add(new LineSegmentPath(previousPosition, this.PathPositions[i]));
                    previousPosition = this.PathPositions[i];
                }
            }
        }

        public override float GetParam(Vector3 position, float previousParam) {
            var localPathIndex = (int)previousParam;

            if (localPathIndex >= this.LocalPaths.Count) {
                return this.LocalPaths.Count;
            }

            var localPath = this.LocalPaths[localPathIndex];
            var localParam = localPath.GetParam(position, previousParam - localPathIndex);
            //if we are at the end of the current local path, try the next one
            if (localPath.PathEnd(localParam)) {
                if (localPathIndex < this.LocalPaths.Count - 1) {
                    localPathIndex++;
                    localPath = this.LocalPaths[localPathIndex];
                    localParam = localPath.GetParam(position, 0.0f);
                }
            }
            return localPathIndex + localParam;
        }

        public override Vector3 GetPosition(float param) {
            var localPathIndex = (int)param;
            if (localPathIndex >= this.LocalPaths.Count) {
                return this.LocalPaths[this.LocalPaths.Count - 1].GetPosition(1.0f);
            }

            var localPath = this.LocalPaths[localPathIndex];

            return localPath.GetPosition(param - localPathIndex);
        }

        public override bool PathEnd(float param) {
            return param > LocalPaths.Count - 0.05;
        }
    }
}
