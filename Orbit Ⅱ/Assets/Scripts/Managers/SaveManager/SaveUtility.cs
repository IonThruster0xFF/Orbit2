/// <summary>
/// Save utility.
/// Utility class for saving/loading data files to/from disk
/// largely based on:
/// https://unity3d.com/learn/tutorials/modules/beginner/live-training-archive/persistence-data-saving-loading
/// 函数参数中的 filePath 要输入完整的路径, 包括文件后缀
/// </summary>
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
namespace Manager
{
    public static class SaveUtility 
    {
        #region Save | Load | Delete  

        /// <summary>
        /// 序列化saveData并且把数据写入硬盘filePath.dat文件
        /// </summary>
        public static void Save<T>(T saveData, string filePath) 
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = GetFileStream(filePath);
            bf.Serialize(file, saveData);
            file.Close();
        }

        /// <summary>
        /// 从硬盘载入文件并返回反序列化后的数据
        /// </summary>
        public static T Load<T>(string filePath) 
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = GetFileStream(filePath);
            if (file.Length == 0) {
                file.Close();
                return default(T);//根据T的类型返回默认的0或者NULL
            }
            T saveData = (T) bf.Deserialize(file);
            file.Close();
            return saveData;
        }
        /// <summary>
        /// 从硬盘删除存档
        /// </summary>
        public static void Delete(string filePath) 
        {
            if (!File.Exists(filePath)) 
                return;
            File.Delete(filePath);
        }
        #endregion

        /// <summary>
        /// 从硬盘打开一个路径下的文件
        /// 如果目录不存在, 创建之
        /// 如果文件不存在, 创建之
        /// </summary>
        private static FileStream GetFileStream(string filePath) 
        {
            string dirName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dirName))
            {
                Debug.Log("Create Dir "+dirName);
                Directory.CreateDirectory(dirName);
            }
            if (!File.Exists(filePath)) 
            {
                Debug.Log("Create New File "+filePath);
                return File.Create(filePath);
            }
            return File.Open(filePath, FileMode.Open);
        }
    }
}