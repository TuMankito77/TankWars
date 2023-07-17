namespace TankWars.Editor.Tools
{
    using System.IO;
    using System.Linq; 
    using UnityEditor;
    using UnityEngine;

    public class TankMaterialAndModelRefresher 
    {
        private const string TANK_SHADER_PATH_AND_NAME = "Assets/Art/Shaders/TankShader.shadergraph";
        private const string TANK_MATERIAL_SAVE_PATH = "Assets/Art/Materials";
        private const string TANK_MATERIAL_NAME_SUFFIX = "-TiledTexture"; 
        private const string TEXTURE_FOLDERS_PATH = "Art/Textures";
        private const string TEXTURE_AMBIENT_OCLUSSION_SUFFIX = "_AmbientOclusion";
        private const string TEXTURE_BASE_COLOR_SUFFIX = "_BaseColor"; 
        private const string TEXTURE_COLOR_MASK_SUFFIX = "_ColorMask";
        private const string TEXTURE_EMISSIVE_SUFFIX = "_Emissive"; 
        private const string TEXTURE_METALLIC_SUFFIX = "_Metallic";
        private const string TEXTURE_NORMAL_SUFFIX = "_Normal";
        private const string TEXTURE_ROUGHNESS_SUFFIX = "_Roughness";
        private const string TEXTURE_FILE_EXTENSION = ".png";
        private const string MATERIAL_BASE_COLOR_PROPERTY_NAME = "_Base_Color";
        private const string MATERIAL_COLOR_MASK_PROPERTY_NAME = "_Color_Mask";
        private const string MATERIAL_NORMAL_PROPERTY_NAME = "_Normal";
        private const string MATERIAL_METALLIC_PROPERTY_NAME = "_Metallic";
        private const string MATERIAL_ROUGHNESS_PROPERTY_NAME = "_Roughness";
        private const string MATERIAL_EMISSION_PROPERTY_NAME = "_Emission";
        private const string MATERIAL_AMBIENT_OCLUSION_PROPERTY_NAME = "_Ambient_Oclusion";
        private const string MATERIAL_EMISSION_STRENGTH_PROPERTY_NAME = "_Emission_Strength";
        private const string MATERIAL_FILE_EXTENSION = ".mat"; 
        private const string TANK_STATIC_MESHES_PATH = "Assets/Art/Models/StaticTanks";
        private const string TANK_MOVABLE_MESHES_PATH = "Assets/Art/Models/MovableTanks";
        private const string TANK_MODEL_FILE_EXTENTION = ".fbx"; 

        [MenuItem("TankWars/Update Tank Materials and 3D Models")]
        private static void UpdateTankMaterialsAndModels() 
        {
            string texturesFullPath = Path.Combine(Application.dataPath, TEXTURE_FOLDERS_PATH);
            string[] tankTextureDirectories = Directory.GetDirectories(texturesFullPath).Select(directory => new DirectoryInfo(directory).Name).ToArray();

            GenerateTankMaterials(tankTextureDirectories);
            UpdateTankMeshesMaterial(TANK_STATIC_MESHES_PATH, tankTextureDirectories);
            UpdateTankMeshesMaterial(TANK_MOVABLE_MESHES_PATH, tankTextureDirectories);
        }

        private static void UpdateTankMeshesMaterial(string tankModelsPath, string[] tankUniqueNames)
        {
            foreach(string name in tankUniqueNames)
            {
                ModelImporter modelImporter = AssetImporter.GetAtPath(tankModelsPath + "/" + name + TANK_MODEL_FILE_EXTENTION) as ModelImporter;
                
                if (modelImporter == null)
                {
                    Debug.LogWarning($"The mesh for the textures of the {name} is missing inside the path {tankModelsPath}.");
                    continue; 
                }

                modelImporter.materialLocation = ModelImporterMaterialLocation.External;
                modelImporter.materialName = ModelImporterMaterialName.BasedOnModelNameAndMaterialName;
                modelImporter.materialSearch = ModelImporterMaterialSearch.Everywhere;
                modelImporter.SaveAndReimport();
            }
        }

        private static void GenerateTankMaterials(string[] textureDirectoryNames)
        {
            Shader tankShader = AssetDatabase.LoadAssetAtPath<Shader>(TANK_SHADER_PATH_AND_NAME);

            foreach (string directoryName in textureDirectoryNames)
            {
                Texture ambienOclusionTexture = GetTexture(directoryName, TEXTURE_AMBIENT_OCLUSSION_SUFFIX);
                Texture baseColorTexture = GetTexture(directoryName, TEXTURE_BASE_COLOR_SUFFIX);
                Texture colorMaskTexture = GetTexture(directoryName, TEXTURE_COLOR_MASK_SUFFIX);
                Texture emissiveTexture = GetTexture(directoryName, TEXTURE_EMISSIVE_SUFFIX);
                Texture metallicTexture = GetTexture(directoryName, TEXTURE_METALLIC_SUFFIX);
                Texture normalTexture = GetTexture(directoryName, TEXTURE_NORMAL_SUFFIX);
                Texture roughnessTexture = GetTexture(directoryName, TEXTURE_ROUGHNESS_SUFFIX);

                Material tankMaterial = new Material(tankShader);
                tankMaterial.SetTexture(MATERIAL_AMBIENT_OCLUSION_PROPERTY_NAME, ambienOclusionTexture);
                tankMaterial.SetTexture(MATERIAL_BASE_COLOR_PROPERTY_NAME, baseColorTexture);
                tankMaterial.SetTexture(MATERIAL_COLOR_MASK_PROPERTY_NAME, colorMaskTexture);
                tankMaterial.SetTexture(MATERIAL_METALLIC_PROPERTY_NAME, metallicTexture);
                tankMaterial.SetTexture(MATERIAL_NORMAL_PROPERTY_NAME, normalTexture);
                tankMaterial.SetTexture(MATERIAL_ROUGHNESS_PROPERTY_NAME, roughnessTexture);

                if (emissiveTexture)
                {
                    tankMaterial.SetTexture(MATERIAL_EMISSION_PROPERTY_NAME, emissiveTexture);
                    tankMaterial.SetFloat(MATERIAL_EMISSION_STRENGTH_PROPERTY_NAME, 1);
                }
                else
                {
                    tankMaterial.SetFloat(MATERIAL_EMISSION_STRENGTH_PROPERTY_NAME, 0);
                }

                string materialNamePrefix = directoryName;
                AssetDatabase.CreateAsset(tankMaterial, TANK_MATERIAL_SAVE_PATH + "/" + materialNamePrefix + TANK_MATERIAL_NAME_SUFFIX + MATERIAL_FILE_EXTENSION);
            }
        }

        private static Texture GetTexture(string directoryName, string textureSuffix)
        {
            string texturePath = "Assets/Art/Textures/" + directoryName;
            string textureNamePrefix = directoryName;
            string ambientOclusionTextureName = textureNamePrefix + textureSuffix + TEXTURE_FILE_EXTENSION;
            return AssetDatabase.LoadAssetAtPath<Texture>(texturePath + "/" + ambientOclusionTextureName);
        }
    }
}
