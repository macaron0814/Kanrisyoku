#if UNITY_EDITOR
using UnityEditor;
using System.IO;
public class SetFirstFolder : EditorWindow
{
    [MenuItem("Custom/CreateFolder")]
    static void CreateEditorFolder()  //FolderCreate("フォルダ名");　で新しいフォルダ追加可能
    {

        BuildFolderCreate();

        if (AssetDatabase.IsValidFolder("Assets/_CommonFolder"))
        {
            return;
        }

        FolderCreate("Animations");
        FolderCreate("Audios");
        FolderCreate("AnimatorControllers");
        FolderCreate("Editors");
        FolderCreate("Textures");
        FolderCreate("Materials");
        FolderCreate("Prefabs");
        FolderCreate("Resources");
        FolderCreate("RenderTextures");
        FolderCreate("Scripts");
        FolderCreate("Scenes");
        FolderCreate("Shaders");
    }

    static void FolderCreate(string FolderCreate)
    {
        string path = "Assets/_CommonFolder/" + FolderCreate;

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        AssetDatabase.ImportAsset(path);
    }

    static void BuildFolderCreate()
    {
        string path = "Build/";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        AssetDatabase.ImportAsset(path);
    }
}
#endif