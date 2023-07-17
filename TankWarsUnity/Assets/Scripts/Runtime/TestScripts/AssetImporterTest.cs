namespace TankWars.Runtime.Test
{
    using UnityEditor;
    using UnityEngine;

    public class AssetImporterTest : AssetPostprocessor
    {
        /*private void OnPreprocessModel()
        {
            ModelImporter modelImporter = assetImporter as ModelImporter;
            
            if (modelImporter == null || !assetPath.Contains("Tank"))
            {
                return; 
            }

                modelImporter.materialLocation = ModelImporterMaterialLocation.External;
                modelImporter.materialName = ModelImporterMaterialName.BasedOnModelNameAndMaterialName;
                modelImporter.materialSearch = ModelImporterMaterialSearch.Everywhere; 

            Debug.Log(assetPath); 
        }*/
    }
}
