using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
using System;
namespace CustomPlugins.ToolEditor
{
    using ToolWindow;
    public class ReNameTool : UnityEditor.Editor
    {
        public string value1;
        
        
        
        
        
        private delegate string NamingNotationsDelegate(string name, string addPrefix = null);
        private static NamingNotationsDelegate namingNotationsDelegate;
        /*
        [MenuItem("Assets/特效命名规范/一键规范化", false, 100)]
        public static void NameNormalize()
        {
            namingNotationsDelegate = Criterion;
            ReName(namingNotationsDelegate);
        }
        [MenuItem("Assets/特效命名规范/加Fx前缀", false, 101)]
        public static void AddFXPrefix()
        {
            namingNotationsDelegate = AddPrefix;
            ReName(namingNotationsDelegate, "Fx");
        }
        [MenuItem("Assets/特效命名规范/加UI前缀", false, 102)]
        public static void AddUIPrefix()
        {
            namingNotationsDelegate = AddPrefix;
            ReName(namingNotationsDelegate, "UI");
        }
        */
        [MenuItem("Assets/资产批量命名工具", false, 103)]
        public static void AddCustomPrefix()
        {
            ReNameWindow.ShowWindow();
        }
        public static void AddCustomPrefix(string pName)
        {
            namingNotationsDelegate = AddPrefix;
            ReName(namingNotationsDelegate, pName);
        }
        public static void AddCustomPostfix(string pName)
        {
            namingNotationsDelegate = AddPostfix;
            ReName(namingNotationsDelegate, pName);
        }
        public static void Remove(string pName)
        {
            namingNotationsDelegate = ReMoveString;
            ReName(namingNotationsDelegate, pName);
        }
        public static void AddCustomPostfixNumber(int addCount)
        {
            ReName(addCount);
        }
        
        
        
        
        
        
        
        
        
        
        /// <summary>
        /// 匹配修改
        /// </summary>
        /// <param name="name"></param>
        /// <param name="removeString"></param>
        /// <returns></returns>
        public static void MatchingModifyString(string Matching, string Modifty)
        {
            namingNotationsDelegate = delegate (string s, string s1)
            {
                string[] matchingModify = s1.Split('/');
                return s.Replace(matchingModify[0], matchingModify[1]);
            };
            ReName(namingNotationsDelegate, Matching + "/" + Modifty);
        }
        public static void ReDefine(string reString, int count)
        {
            UnityEngine.Object[] arr = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Unfiltered);
            for (int index = 0; index < arr.Length; index++)
            {
                string filePath = AssetDatabase.GetAssetPath(arr[index]);
                FileInfo fileInfo = new FileInfo(filePath);
                string fileName = filePath.Split('.')[0];
                string[] fileNamePath = fileName.Split('/');
                AssetDatabase.RenameAsset(filePath, "Name_"+ index);
            }
            for (int index = 0; index < arr.Length; index++)
            {
                string filePath = AssetDatabase.GetAssetPath(arr[index]);
                FileInfo fileInfo = new FileInfo(filePath);
                string fileName = filePath.Split('.')[0];
                string[] fileNamePath = fileName.Split('/');
                AssetDatabase.RenameAsset(filePath, reString + (count+ index));
            }
            AssetDatabase.Refresh();
        }
        private static void ReName(NamingNotationsDelegate fun,string s = null)
        {
            UnityEngine.Object[] arr = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Unfiltered);
            for (int index = 0; index < arr.Length; index++)
            {
                string filePath = AssetDatabase.GetAssetPath(arr[index]);
                FileInfo fileInfo = new FileInfo(filePath);
                string fileName = filePath.Split('.')[0];
                string[] fileNamePath = fileName.Split('/');
                //fileNamePath[fileNamePath.Length - 1] = fun(fileNamePath[fileNamePath.Length - 1]);//更改的名字
                //string destFileName = ArrayToString(fileNamePath) + fileInfo.Extension;
                ////System.IO.File.Move(filePath, destFileName);
                AssetDatabase.RenameAsset(filePath, fun(fileNamePath[fileNamePath.Length - 1], s));
            }
            AssetDatabase.Refresh();
        }
        private static void ReName(int addCount)
        {
            UnityEngine.Object[] arr = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Unfiltered);
            for (int index = 0; index < arr.Length; index++)
            {
                string filePath = AssetDatabase.GetAssetPath(arr[index]);
                FileInfo fileInfo = new FileInfo(filePath);
                string fileName = filePath.Split('.')[0];
                string[] fileNamePath = fileName.Split('/');
                AssetDatabase.RenameAsset(filePath, fileNamePath[fileNamePath.Length - 1] + (addCount+1+ index));
            }
            AssetDatabase.Refresh();
        }
        
        
        
        
        
        
        
        
        
        
        
        
        /// <summary>
        /// 添加Fx前缀
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string AddPrefix(string name, string addPrefix)
        {
            if (name.Length < 2)
            {
                return addPrefix + name;
            }
            string prefix = name.Substring(0, 2);
            if (prefix.Equals(addPrefix, StringComparison.InvariantCultureIgnoreCase))//已存在Fx前缀
            {
                name = char.ToUpper(name[0]) + name.Substring(1);//首字母大写
            }
            else
            {
                name = addPrefix + name;
            }
            return name;
        }
        /// <summary>
        /// 添加后缀
        /// </summary>
        /// <param name="name"></param>
        /// <param name="addPrefix"></param>
        /// <returns></returns>
        private static string AddPostfix(string name, string addPrefix)
        {
            return name + addPrefix;
        }
        /// <summary>
        /// 移除对应字符串
        /// </summary>
        /// <param name="name"></param>
        /// <param name="removeString"></param>
        /// <returns></returns>
        private static string ReMoveString(string name, string removeString)
        {
            return name.Replace(removeString, "");//去除空格
        }
    
        /// <summary>
        /// 标准化
        /// </summary>
        private static string Criterion(string name , string addPrefix = null)
        {
            string criterionName = name.Replace(" ", "");//去除空格
            criterionName = Regex.Replace(criterionName, @"[^a-zA-Z0-9\u4e00-\u9fa5\s]", "");//去除符号
            foreach (var c in criterionName)//移除前面为数字的情况
            {
                if (c >= '0' && c <= '9')
                {
                    criterionName = criterionName.Substring(1);
                }
                else
                {
                    break;
                }
            }
            criterionName = char.ToUpper(criterionName[0]) + criterionName.Substring(1);//首字母大写
            return criterionName;
        }

    }

}