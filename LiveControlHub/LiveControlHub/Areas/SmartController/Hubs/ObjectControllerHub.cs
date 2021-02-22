using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace LiveControlHub.Areas.SmartController.Hubs
{
    public class ObjectControllerHub : Hub
    {
        private static Vector3 objectPosition = new Vector3();
        private static Vector3 objectRotation = new Vector3();
        public void SaveObjectPosition()
        {
            objectPosition = LiveControlHub.Program.ObjectController.GetObjectPosition("Cube");
        }

        public void MoveObjectPositionXYZ(float x, float y, float z)
        {
            var scale = 0.1f;
            
            var dx = -y * scale + objectPosition.X;
            var dy = -x * scale + objectPosition.Y;
            var dz = z * scale + objectPosition.Z;

            LiveControlHub.Program.ObjectController.MoveObjectSetPosition(dx, dy, dz, "Cube");
            return;
        }

        public void SaveObjectRotation()
        {
            objectRotation = LiveControlHub.Program.ObjectController.GetObjectRotation("Cube");
        }

        public void RotateObjectXYZ(float x, float y, float z, float scale, string objectName)
        {
            var sscale = 0.06f * scale;

            var dx = x * sscale;
            var dy = y * sscale;
            var dz = z * sscale;

            objectRotation.X -= dx;
            objectRotation.Y -= dy;
            objectRotation.Z -= dz;

            var xr = objectRotation.X;
            var yr = objectRotation.Y;
            var zr = objectRotation.Z;

           LiveControlHub.Program.ObjectController.RotateObjectSetRotation(xr, yr, zr, "Cube");
            return;
        }
    }
}
