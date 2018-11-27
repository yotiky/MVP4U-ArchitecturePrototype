using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchitecturePrototype.Model.ServiceAgent.Azure
{
    [Serializable]
    public class CustomVisionResponse
    {
        public string id;
        public string project;
        public string iteration;
        public DateTime created;
        public Prediction[] predictions;
    }

    [Serializable]
    public class Prediction
    {
        public float probability;
        public string tagId;
        public string tagName;
        public Boundingbox boundingBox;
    }

    [Serializable]
    public class Boundingbox
    {
        public float left;
        public float top;
        public float width;
        public float height;
    }
}
