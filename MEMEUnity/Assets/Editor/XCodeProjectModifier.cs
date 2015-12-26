using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.Collections;
using System.IO;

public class XCodeProjectModifier {

	internal static void CopyAndReplaceDirectory(string srcPath, string dstPath)
	{
		if (Directory.Exists (dstPath)) {
			Directory.Delete (dstPath);
		}
		if (File.Exists (dstPath)) {
			File.Delete (dstPath);
		}

		Directory.CreateDirectory(dstPath);

		foreach (var file in Directory.GetFiles(srcPath)) {
			File.Copy (file, Path.Combine (dstPath, Path.GetFileName (file)));
		}

		foreach (var dir in Directory.GetDirectories(srcPath)) {
			CopyAndReplaceDirectory (dir, Path.Combine (dstPath, Path.GetFileName (dir)));
		}
	}

	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
	{
		if (buildTarget == BuildTarget.iOS)
		{
			string projPath = PBXProject.GetPBXProjectPath(path);
			PBXProject proj = new PBXProject();

			proj.ReadFromString(File.ReadAllText(projPath));
			string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());

			// CoreBluetoothのフレームワークを追加
			proj.AddFrameworkToProject(target, "CoreBluetooth.framework", false);

			// MEMEのフレームワークを追加
			var memeFrameworkPath = "Assets/Editor/Files/MEMELib.framework";
			CopyAndReplaceDirectory(memeFrameworkPath, Path.Combine(path, "Frameworks/MEMELib.framework"));
			proj.AddFileToBuild(target, proj.AddFile("Frameworks/MEMELib.framework", "Frameworks/MEMELib.framework", PBXSourceTree.Source));

			// フレームワークの検索パスを設定・追加
			proj.SetBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(inherited)");
			proj.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(PROJECT_DIR)/Frameworks");

			proj.SetBuildProperty(target, "LD_RUNPATH_SEARCH_PATHS", "$(inherited)");
			proj.AddBuildProperty(target, "LD_RUNPATH_SEARCH_PATHS", "@executable_path/Frameworks");

			// 書き出し
			File.WriteAllText(projPath, proj.WriteToString());


			// plistにCoreBluetoothの設定を追加
			var plistPath = Path.Combine(path, "Info.plist");
			var plist = new PlistDocument ();
			plist.ReadFromFile (plistPath);
			var backgroundModes = plist.root.CreateArray ("UIBackgroundModes");
			backgroundModes.AddString ("bluetooth-central");
			plist.WriteToFile (plistPath);
		}
	}
}
