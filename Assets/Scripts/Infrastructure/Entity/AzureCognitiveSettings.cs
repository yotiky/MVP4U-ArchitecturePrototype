using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArchitecturePrototype.Infrastructure.Entity
{
    [CreateAssetMenu(fileName = "AzureCognitiveSettings", menuName = "ScriptableObject/AzureCognitiveSettings", order = 0)]
    public class AzureCognitiveSettings : ScriptableObject
    {
        [SerializeField]
        private string customVisionUrl = "your api url";
        public string CustomVisionUrl
        {
            get { return customVisionUrl; }
#if UNITY_EDITOR
            set { customVisionUrl = value; }
#endif
        }

        [SerializeField]
        private string customVisionApiKey = "your api key";
        public string CustomVisionApiKey
        {
            get { return customVisionApiKey; }
#if UNITY_EDITOR
            set { customVisionApiKey = value; }
#endif
        }


        private const string PATH = "AzureCognitiveSettings";

        private static AzureCognitiveSettings instance;
        public static AzureCognitiveSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<AzureCognitiveSettings>(PATH);
                }
                return instance;
            }
        }
        private AzureCognitiveSettings() { }
    }
}
