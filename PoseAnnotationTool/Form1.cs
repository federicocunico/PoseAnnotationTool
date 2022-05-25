using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoseAnnotationTool
{
    public partial class Form1 : Form
    {
        class Joint
        {
            public bool Use { get; set; }
            public PointF Position { get; set; }
            public string ParentName { get; set; }
            public int ParentIndex { get; set; }
            public string Name { get; set; }
            public Joint(float x, float y, string name, string parentName, bool use = true)
            {
                Position = new PointF(x, y);
                ParentName = parentName;
                Use = use;
                Name = name;
            }

            public Joint(float x, float y, int parentIndex, string name, bool use = true)
            {
                Position = new PointF(x, y);
                ParentIndex = parentIndex;
                Use = use;
                Name = name;
            }
        }


        Datasets datasets = null;

        string ImagePath = "";
        Bitmap Img = null;

        CheckBox[] checkboxes = new CheckBox[17];
        Joint[] kps = new Joint[17];
        Color[] jointColors = new Color[17];
        Color[] boneColors = new Color[17];

        private static CultureInfo EnUs = new CultureInfo("en-US");
        public Form1()
        {
            Thread.CurrentThread.CurrentCulture = EnUs;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            checkboxes[0] = checkBox1;
            checkboxes[1] = checkBox2;
            checkboxes[2] = checkBox3;
            checkboxes[3] = checkBox4;
            checkboxes[4] = checkBox5;
            checkboxes[5] = checkBox6;
            checkboxes[6] = checkBox7;
            checkboxes[7] = checkBox8;
            checkboxes[8] = checkBox9;
            checkboxes[9] = checkBox10;
            checkboxes[10] = checkBox11;
            checkboxes[11] = checkBox12;
            checkboxes[12] = checkBox13;
            checkboxes[13] = checkBox14;
            checkboxes[14] = checkBox16;
            checkboxes[15] = checkBox17;
            checkboxes[16] = checkBox18;

            // edge color
            jointColors[0] = Color.FromArgb(255, 233, 163, 201);
            jointColors[1] = Color.FromArgb(255, 233, 163, 201);
            jointColors[2] = Color.FromArgb(255, 233, 163, 201);
            jointColors[3] = Color.FromArgb(255, 197, 27, 125);
            jointColors[4] = Color.FromArgb(255, 197, 27, 125);
            jointColors[5] = Color.FromArgb(255, 197, 27, 125);
            jointColors[6] = Color.FromArgb(255, 145, 191, 219);
            jointColors[7] = Color.FromArgb(255, 145, 191, 219);
            jointColors[8] = Color.FromArgb(255, 145, 191, 219);
            jointColors[9] = Color.FromArgb(255, 69, 117, 180);
            jointColors[10] = Color.FromArgb(255, 69, 117, 180);
            jointColors[11] = Color.FromArgb(255, 69, 117, 180);
            jointColors[12] = Color.FromArgb(255, 118, 42, 131);
            jointColors[13] = Color.FromArgb(255, 158, 100, 69);
            jointColors[14] = Color.FromArgb(255, 158, 69, 100);
            jointColors[15] = Color.FromArgb(255, 69, 158, 100);
            jointColors[16] = Color.FromArgb(255, 255, 152, 0);

            boneColors[0] = Color.FromArgb(255, 233, 163, 201);
            boneColors[1] = Color.FromArgb(255, 233, 163, 201);
            boneColors[2] = Color.FromArgb(255, 233, 163, 201);
            boneColors[3] = Color.FromArgb(255, 197, 27, 125);
            boneColors[4] = Color.FromArgb(255, 197, 27, 125);
            boneColors[5] = Color.FromArgb(255, 197, 27, 125);
            boneColors[6] = Color.FromArgb(255, 145, 191, 219);
            boneColors[7] = Color.FromArgb(255, 145, 191, 219);
            boneColors[8] = Color.FromArgb(255, 145, 191, 219);
            boneColors[9] = Color.FromArgb(255, 69, 117, 180);
            boneColors[10] = Color.FromArgb(255, 69, 117, 180);
            boneColors[11] = Color.FromArgb(255, 69, 117, 180);
            boneColors[12] = Color.FromArgb(255, 118, 42, 131);
            boneColors[13] = Color.FromArgb(255, 100, 30, 58);
            boneColors[14] = Color.FromArgb(255, 30, 100, 58);
            boneColors[15] = Color.FromArgb(255, 7, 197, 131);
            boneColors[16] = Color.FromArgb(255, 125, 39, 128);

            //kps[0] = new Joint(110, 100, 1);
            //kps[1] = new Joint(120, 100, 2);
            //kps[2] = new Joint(130, 100, 8);
            //kps[3] = new Joint(140, 100, 9);
            //kps[4] = new Joint(150, 100, 3);
            //kps[5] = new Joint(160, 100, 4);
            //kps[6] = new Joint(170, 100, 7);
            //kps[7] = new Joint(180, 100, 8);
            //kps[8] = new Joint(190, 100, 12);
            //kps[9] = new Joint(200, 100, 12);
            //kps[10] = new Joint(300, 100, 9);
            //kps[11] = new Joint(400, 100, 10);
            //kps[12] = new Joint(500, 100, 13);
            //kps[13] = new Joint(600, 100, -1);
            //kps[14] = new Joint(650, 100, 14);
            //kps[15] = new Joint(680, 100, 15);
            //kps[16] = new Joint(700, 100, 16);

            //kps[0] = new Joint(110, 100, 6, "Nose");  // Nose
            //kps[1] = new Joint(120, 100, 0, "Leye");  // L eye
            //kps[2] = new Joint(130, 100, 0, "Reye");  // R eye
            //kps[3] = new Joint(140, 100, 1, "Lear");  // L ear
            //kps[4] = new Joint(150, 100, 2, "Rear");  // R ear
            //kps[5] = new Joint(160, 100, 6, "Lshoulder");  // L shoulder
            //kps[6] = new Joint(170, 100, 12, "Rshoulder");  // R shoulder
            //kps[7] = new Joint(180, 100, 5, "Lelbow");  // L elbow
            //kps[8] = new Joint(190, 100, 6, "Relbow"); // R elbow
            //kps[9] = new Joint(200, 100, 7, "Lwrist"); // L wrist
            //kps[10] = new Joint(300, 100, 8, "Rwrist"); // R wrist
            //kps[11] = new Joint(400, 100, 5, "Lhip"); // L hip
            //kps[12] = new Joint(500, 100, 11, "Rhip"); // R hip
            //kps[13] = new Joint(600, 100, 11, "Lknee"); // L knee
            //kps[14] = new Joint(650, 100, 12, "Rknee"); // R knee
            //kps[15] = new Joint(680, 100, 13, "Lankle"); // L ankle
            //kps[16] = new Joint(700, 100, 14, "Rankle"); // R ankle


            kps[0] = new Joint(434, 300, "Nose", "Rshoulder");  // Nose
            kps[1] = new Joint(484, 173, "Leye", "Nose");  // L eye
            kps[2] = new Joint(366, 204, "Reye", "Nose");  // R eye
            kps[3] = new Joint(578, 219, "Lear", "Leye");  // L ear
            kps[4] = new Joint(269, 219, "Rear", "Reye");  // R ear
            kps[5] = new Joint(627, 400, "Lshoulder", "Rshoulder");  // L shoulder
            kps[6] = new Joint(302, 442, "Rshoulder", "Rhip");  // R shoulder
            kps[7] = new Joint(748, 574, "Lelbow", "Lshoulder");  // L elbow
            kps[8] = new Joint(162, 608, "Relbow", "Rshoulder"); // R elbow
            kps[9] = new Joint(790, 888, "Lwrist", "Lelbow"); // L wrist
            kps[10] = new Joint(150, 869, "Rwrist", "Relbow"); // R wrist
            kps[11] = new Joint(635, 930, "Lhip", "Lshoulder"); // L hip
            kps[12] = new Joint(344, 949, "Rhip", "Lhip"); // R hip
            kps[13] = new Joint(654, 1183, "Lknee", "Lhip"); // L knee
            kps[14] = new Joint(378, 1221, "Rknee", "Rhip"); // R knee
            kps[15] = new Joint(669, 1482, "Lankle", "Lknee"); // L ankle
            kps[16] = new Joint(381, 1425, "Rankle", "Rknee"); // R ankle

            SetNames();

            foreach (var kpt in kps)
            {
                kpt.ParentIndex = kps.ToList().IndexOf(kps.First(a => a.Name == kpt.ParentName));
            }

            var map =
                kps.Select(a => new
                {
                    a.Name,
                    ParentName = a.ParentIndex >= 0 ? kps[a.ParentIndex].Name : "End"
                })
                .ToList();

            for (int i = 0; i < map.Count; i++)
            {
                var m = map[i];

                //checkboxes[i].Text = $"{m.Name} ({m.ParentName})";
                checkboxes[i].Text = $"{m.Name}";
            }

            int annotCount = 0;
            datasets = new Datasets();
            foreach (var f in datasets.FileList)
            {
                string relative = f.Substring(datasets.DataDir.Length);
                var tokens = relative.Split('/', '\\').Where(t => string.IsNullOrWhiteSpace(t) == false).ToList();

                var cur = treeView1.Nodes;
                TreeNode leaf = null;
                foreach (var t in tokens)
                {
                    bool found = false;
                    foreach (TreeNode n in cur)
                    {
                        if (n.Text == t)
                        {
                            cur = n.Nodes;
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        leaf = cur.Add(t);
                        cur = leaf.Nodes;
                    }
                }

                if (leaf != null)
                {
                    string annotPath = System.IO.Path.ChangeExtension(f, ".txt");
                    if (System.IO.File.Exists(annotPath))
                    {
                        leaf.ForeColor = Color.FromArgb(255, 69, 117, 255);
                        annotCount++;
                    }
                }
            }

            Console.WriteLine("# of Annotation = " + annotCount);
        }

        private void SetNames()
        {
            kps[0].Name = "Nose";
            kps[1].Name = "Leye";
            kps[2].Name = "Reye";
            kps[3].Name = "Lear";
            kps[4].Name = "Rear";
            kps[5].Name = "Lshoulder";
            kps[6].Name = "Rshoulder";
            kps[7].Name = "Lelbow";
            kps[8].Name = "Relbow";
            kps[9].Name = "Lwrist";
            kps[10].Name = "Rwrist";
            kps[11].Name = "Lhip";
            kps[12].Name = "Rhip";
            kps[13].Name = "Lknee";
            kps[14].Name = "Rknee";
            kps[15].Name = "Lankle";
            kps[16].Name = "Rankle";


            kps[0].ParentName = "Rshoulder";
            kps[1].ParentName = "Nose";
            kps[2].ParentName = "Nose";
            kps[3].ParentName = "Leye";
            kps[4].ParentName = "Reye";
            kps[5].ParentName = "Rshoulder";
            kps[6].ParentName = "Rhip";
            kps[7].ParentName = "Lshoulder";
            kps[8].ParentName = "Rshoulder";
            kps[9].ParentName = "Lelbow";
            kps[10].ParentName = "Relbow";
            kps[11].ParentName = "Lshoulder";
            kps[12].ParentName = "Lhip";
            kps[13].ParentName = "Lhip";
            kps[14].ParentName = "Rhip";
            kps[15].ParentName = "Lknee";
            kps[16].ParentName = "Rknee";
        }

        private void checkBoxJoint_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkboxes.Length; i++)
            {
                if (sender == checkboxes[i])
                {
                    kps[i].Use = checkboxes[i].Checked;
                    saveAnnotation(ImagePath);
                    break;
                }
            }
            pictureBox1.Invalidate();
        }

        private void checkBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var cb in checkboxes)
            {
                cb.Checked = checkBox15.Checked;
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

            if (e.Node.Nodes.Count > 0)
            {
                return;
            }

            // 終端ノードなら画像読み込み
            List<string> tokens = new List<string>();
            tokens.Add(e.Node.Text);
            var n = e.Node;
            while (n.Parent != null)
            {
                n = n.Parent;
                tokens.Add(n.Text);
            }

            tokens.Add(datasets.DataDir);
            tokens.Reverse();
            string path = System.IO.Path.Combine(tokens.ToArray());
            loadImage(path);
            loadAnnotation(path);
            pictureBox1.Invalidate();
        }
        void loadImage(string path)
        {
            if (Img != null)
            {
                Img.Dispose();
                Img = null;
            }
            ImagePath = path;
            using (var bmp = Bitmap.FromFile(path))
            {
                Img = bmp.Clone() as Bitmap;
            }
        }

        void saveAnnotation(string imagePath)
        {
            // text to save
            string text = imagePath + "\n";
            foreach (var kp in kps)
            {
                //text += "0," + kp.Position.X + "," + kp.Position.Y + "," + kp.Use + "\n";
                text += kp.Position.X.ToString(EnUs) + "," + kp.Position.Y.ToString(EnUs) + "\n";
            }

            // save file
            string target = System.IO.Path.ChangeExtension(imagePath, "txt");
            if (target == "")
            {
                return;
            }
            System.IO.File.WriteAllText(target, text);
        }

        void loadAnnotation(string imagePath)
        {
            string target = System.IO.Path.ChangeExtension(imagePath, "txt");
            if (System.IO.File.Exists(target) == false)
            {
                return;
            }
            var lines = System.IO.File.ReadAllLines(target);
            //string imgPath = lines[0];
            //System.Diagnostics.Debug.Assert(imgPath == imagePath);

            //for (int i = 0; i < kps.Length; i++)
            //{
            //    var tokens = lines[i + 1].Split(',');
            //    string person = tokens[0];
            //    float x = float.Parse(tokens[1]);
            //    float y = float.Parse(tokens[2]);
            //    bool use = bool.Parse(tokens[3]);
            //    kps[i] = new Joint(x, y, kps[i].ParentIndex, "unknown", use);
            //}
            for (int i = 0; i < kps.Length; i++)
            {
                var tokens = lines[i + 1].Split(',');
                float x = float.Parse(tokens[0], EnUs);
                float y = float.Parse(tokens[1], EnUs);
                kps[i] = new Joint(x, y, kps[i].ParentIndex, "unknown", true);
            }
            SetNames();
        }

        float lastRatio_ = 1.0f;

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (Img != null)
            {
                float w = e.Graphics.VisibleClipBounds.Width;
                float h = e.Graphics.VisibleClipBounds.Height;

                float rx = w / Img.Width;
                float ry = h / Img.Height;

                float ratio = Math.Min(rx, ry);
                lastRatio_ = ratio;
                float iw = Img.Width * ratio;
                float ih = Img.Height * ratio;

                e.Graphics.DrawImage(Img, 0, 0, iw, ih);

                for (int i = 0; i < kps.Length; i++)
                {
                    var kp = kps[i];
                    float x = kp.Position.X;
                    float y = kp.Position.Y;
                    x *= ratio;
                    y *= ratio;
                    float r = ratio * 10;

                    if (kp.Use)
                    {
                        e.Graphics.DrawEllipse(new Pen(jointColors[i]), x - r, y - r, 2 * r, 2 * r);
                    }

                    if (kp.ParentIndex >= 0)
                    {
                        var parent = kps[kp.ParentIndex];
                        if (kp.Use && parent.Use)
                        {
                            float px = parent.Position.X;
                            float py = parent.Position.Y;
                            px *= ratio;
                            py *= ratio;
                            e.Graphics.DrawLine(new Pen(boneColors[i], 4 * ratio), x, y, px, py);
                        }
                    }

                    if (kp.Use)
                    {
                        if (kp == HoveringJoint)
                        {
                            e.Graphics.DrawEllipse(new Pen(Color.Red, 2), x - 2 * r, y - 2 * r, 4 * r, 4 * r);
                        }
                        if (kp == SelectingJoint)
                        {
                            e.Graphics.DrawEllipse(new Pen(Color.Red, 2), x - 2 * r, y - 2 * r, 4 * r, 4 * r);
                        }
                    }
                }
            }
        }

        Joint HoveringJoint = null;
        Joint SelectingJoint = null;

        Joint findJointFromPosition(PointF screenPt)
        {
            foreach (var kp in kps)
            {
                if (kp.Use)
                {
                    float x = kp.Position.X;
                    float y = kp.Position.Y;
                    x *= lastRatio_;
                    y *= lastRatio_;
                    float r = lastRatio_ * 10;
                    float dx = screenPt.X - x;
                    float dy = screenPt.Y - y;
                    float distSq = dx * dx + dy * dy;
                    if (distSq <= r * r * 1.5f * 1.5f)
                    {
                        return kp;
                    }
                }
            }
            return null;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (SelectingJoint == null)
            {
                HoveringJoint = findJointFromPosition(e.Location);
            }
            else
            {
                HoveringJoint = null;
                int mx = e.X;
                int my = e.Y;
                SelectingJoint.Position = new PointF(mx / lastRatio_, my / lastRatio_);
            }

            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            SelectingJoint = findJointFromPosition(e.Location);
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            SelectingJoint = null;
            saveAnnotation(ImagePath);
            pictureBox1.Invalidate();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        void swap(string jointNameTemplate)
        {
            var kps1 = kps.First(a => a.Name == "R" + jointNameTemplate);
            var kps2 = kps.First(a => a.Name == "L" + jointNameTemplate);

            var t = kps1.Position;
            kps1.Position = kps2.Position;
            kps2.Position = t;
        }

        private void flipButton_Click(object sender, EventArgs e)
        {
            swap("eye");
            swap("ear");
            swap("shoulder");
            swap("knee");
            swap("elbow");
            swap("wrist");
            swap("ankle");
            swap("hip");
            saveAnnotation(ImagePath);
            pictureBox1.Invalidate();
        }
    }
}
