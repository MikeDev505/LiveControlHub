using LiveControl;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientForBlender
{
    public class ObjectController : IObjectController
    {
        private BlenderRemoteScriptServer server = new BlenderRemoteScriptServer();

        public void Start()
        {
            server.Start();
        }

        private float floatParse(string strfloat)
        {
            var nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            return float.Parse(strfloat, nfi);
        }

        private string floatToString(float v)
        {
            var nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            return v.ToString(nfi);
        }

        public Vector3 GetObjectPosition(string objectName)
        {           

            var script = $@"ob = bpy.data.objects['{objectName}']
scriptResult = repr(ob.location)";

            var response = server.RunScript(script);
            response = response.Replace("Vector((", "").Replace("))", "");
            var sv = response.Split(',');
            var x = floatParse(sv[0]);
            var y = floatParse(sv[1]);
            var z = floatParse(sv[2]);

            var v = new Vector3(x, y, z);
            return v;
        }                

        public void MoveObjectSetPosition(float x, float y, float z, string objectName)
        {            
            var scale = 1.0f;
            var sx = floatToString(x * scale);
            var sy = floatToString(y * scale);
            var sz = floatToString(z * scale);
            var script = $@"ob = bpy.data.objects['{objectName}']
ob.location = mathutils.Vector(({sx}, {sy}, {sz}))
scriptResult = 'ok'";

            var response = server.RunScript(script);
        }

        public void MoveObjectAddPosition(float x, float y, float z, string objectName)
        {            
            var scale = 0.01f;
            var sx = floatToString(-y * scale);
            var sy = floatToString(-x * scale);
            var sz = floatToString(z * scale);
             var script = $@"ob = bpy.data.objects['{objectName}']
ob.location = mathutils.Vector(({sx}, {sy}, {sz}))
scriptResult = 'ok'";

            var response = server.RunScript(script);
        }


        public Vector3 GetObjectRotation(string objectName)
        {
            var script = $@"ob = bpy.data.objects['{objectName}']
scriptResult = repr(ob.rotation_euler)";

            var response = server.RunScript(script);
            response = response.Replace("Euler((", "")                
                .Replace("), 'XYZ')", "");
            var sv = response.Split(',');
            var x = floatParse(sv[0]);
            var y = floatParse(sv[1]);
            var z = floatParse(sv[2]);

            var v = new Vector3(x, y, z);
            return v;
        }

        public void RotateObjectSetRotation(float x, float y, float z, string objectName)
        {
            var scale = 1.0f;
            var sx = floatToString(x * scale);
            var sy = floatToString(y * scale);
            var sz = floatToString(z * scale);
            var script = $@"ob = bpy.data.objects['{objectName}']
ob.rotation_euler = ({sx}, {sy}, {sz})
scriptResult = 'ok'";

            var response = server.RunScript(script);
        }
    }
}
