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
            var objName = getName(objectName);

            var script = $@"{objName.getBlenderObjectScript()}
scriptResult = repr(ob.location)";

            if(objName.Type == EnumObjectType.Bone)
            {
                script = $@"{objName.getBlenderObjectScript()}
mw = obj.convert_space(pose_bone=pb, 
            matrix=pb.matrix, 
            from_space='POSE', 
            to_space='WORLD')
scriptResult = repr(mw.translation)";
            }

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

            var objName = getName(objectName);

            var script = $@"{objName.getBlenderObjectScript()}
ob.location = mathutils.Vector(({sx}, {sy}, {sz}))
scriptResult = 'ok'";

            if (objName.Type == EnumObjectType.Bone)
            {
                script = $@"{objName.getBlenderObjectScript()}
mw = obj.convert_space(pose_bone=pb, 
            matrix=pb.matrix, 
            from_space='POSE', 
            to_space='WORLD')
mw.translation = ({sx}, {sy}, {sz})

pb.matrix = obj.convert_space(pose_bone=pb, 
        matrix=mw, 
        from_space='WORLD', 
        to_space='POSE')
scriptResult = 'ok'";
            }

            var response = server.RunScript(script);
        }

        public void MoveObjectAddPosition(float x, float y, float z, string objectName)
        {            
            var scale = 0.01f;
            var sx = floatToString(-y * scale);
            var sy = floatToString(-x * scale);
            var sz = floatToString(z * scale);

            var objName = getName(objectName);

            var script = $@"{objName.getBlenderObjectScript()}
ob.location = mathutils.Vector(({sx}, {sy}, {sz}))
scriptResult = 'ok'";

            var response = server.RunScript(script);
        }

       

        public Vector3 GetObjectRotation(string objectName)
        {
            var objName = getName(objectName);

            var script = $@"{objName.getBlenderObjectScript()}
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

            var objName = getName(objectName);

            var script = $@"{objName.getBlenderObjectScript()}
ob.rotation_euler = ({sx}, {sy}, {sz})
scriptResult = 'ok'";            

            var response = server.RunScript(script);
        }




        #region pomocnicze
        enum EnumObjectType { Object, Bone };
        class NameContainer
        {
            public EnumObjectType Type { get; set; }
            public string ObjectName { get; set; }
            public string BoneName { get; set; }

            public string getBlenderObjectScript()
            {
                var script = $@"obj = bpy.data.objects['{ObjectName}']
ob=obj";
                if (Type == EnumObjectType.Bone)
                {
                    script = $@"obj = bpy.data.objects['{ObjectName}']
pb = obj.pose.bones.get('{BoneName}')
ob = pb";
                }

                return script;
            }
        }

        private NameContainer getName(string objectName)
        {
            if (objectName.StartsWith("Object___"))
            {
                return new NameContainer()
                {
                    Type = EnumObjectType.Object,
                    ObjectName = objectName.Replace("Object___", "")
                };
            }
            else if (objectName.StartsWith("Bone___"))
            {
                var oName = objectName.Replace("Bone___", "");
                var names = oName.Split(new string[] { "___" }, StringSplitOptions.RemoveEmptyEntries);
                return new NameContainer()
                {
                    Type = EnumObjectType.Bone,
                    ObjectName = names[0],
                    BoneName = names[1]
                };
            }
            else
            {
                throw new Exception("Incorect object name: chould be 'Object___Cube' or 'Bone___ArmatureName___BonaName'");
            }
        }
        #endregion
    }
}
