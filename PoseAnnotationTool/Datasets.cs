using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoseAnnotationTool
{
    class Datasets
    {
        //const string DefaultDataDir = @"D:\workspace\human_datasets\train_data\anime\images";

        public string DataDir { get; set; }
        public List<string> FileList = new List<string>();

        public Datasets(string dataDir = null)
        {
            string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
            
            string DefaultDataDir = System.IO.Path.Combine(strWorkPath, "images");
            DataDir = DefaultDataDir;
            dataDir = DefaultDataDir;
            FileList.AddRange(System.IO.Directory.GetFiles(dataDir, "*.png", System.IO.SearchOption.AllDirectories).ToList());
            FileList.AddRange(System.IO.Directory.GetFiles(dataDir, "*.jpg", System.IO.SearchOption.AllDirectories).ToList());

            FileList.Sort();
        }
    }
}
