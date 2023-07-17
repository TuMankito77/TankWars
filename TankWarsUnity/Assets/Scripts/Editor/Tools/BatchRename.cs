namespace TankWars.Editor.Tools
{
    using UnityEditor; 
    using UnityEngine;

    public class BatchRename : ScriptableWizard
    {
        [SerializeField]
        private string baseName = "MyObject_";

        [SerializeField]
        private int startNumber = 0;

        [SerializeField]
        private int increment = 1;

        #region Unity Methods

        private void OnEnable()
        {
            UpdateSelectionHelper(); 
        }

        private void OnSelectionChange()
        {
            UpdateSelectionHelper(); 
        }

        private void OnWizardCreate()
        {
            if(Selection.objects == null)
            {
                return; 
            }

            int index = startNumber; 

            foreach(Object obj in Selection.objects)
            {
                obj.name = baseName + index;
                index += increment; 
            }
        }

        #endregion

        [MenuItem("Edit/Batch Rename...")]
        private static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard("Batch Rename", typeof(BatchRename), "Rename");
        }


        private void UpdateSelectionHelper()
        {
            helpString = "";

            if (Selection.objects != null)
            {
                helpString = $"Numbeer of objects selected: {Selection.objects.Length}";
            }
        }
    }
}
