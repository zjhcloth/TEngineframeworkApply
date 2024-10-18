//=====================================================
// - FileName:     ScriptsTitleManager.cs
// - AuthorName:     #AuthorName#
// - CreateTime:      #CreateTime#
// - Description:   
// - 
//======================================================
using System.IO;
using Cysharp.Threading.Tasks;

public class ScriptsTitleManager : UnityEditor.AssetModificationProcessor
{
    private const string AuthorName = "ZhanJianhua";
    private const string DateFormat = "yyyy/MM/dd HH:mm:ss";
    private static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        if (path.EndsWith(".cs"))
        {
            ChangeFileContent(path);
        }
    }
 
    static async void ChangeFileContent(string path)
    {
        await UniTask.Yield();
        string allText = File.ReadAllText(path);
        allText = allText.Replace("#AuthorName#", AuthorName);
        allText = allText.Replace("#CreateTime#", System.DateTime.Now.ToString(DateFormat));
        File.WriteAllText(path, allText);
        UnityEditor.AssetDatabase.Refresh();
    }
}