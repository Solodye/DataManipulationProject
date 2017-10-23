using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestManipulateMainPgPic
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
        }

        //内存中的图片转化为二进制数组载入到内存
        public static byte[] LoadImageFile(string path)
        {
            using (FileStream fs = File.OpenRead(path))
            {
                int filelength = 0;
                filelength = (int)fs.Length; //获得文件长度 
                Byte[] image = new Byte[filelength]; //建立一个字节数组 
                fs.Read(image, 0, filelength); //按字节流，一次性读取
                return image;
            }

        }

        //数据库tb_MainPgPic(首页图片表)的设计是仅仅有一列来存放图片，所以更新图片要给出Id号
        public static bool UplodePicture(int Id, byte[] imgb)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.GetConnectionStr()))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    //封装一个更新操作的语句
                    cmd.CommandText = "update tb_MainPgPic set picture = @picture where Id = " + Id;
                    cmd.Parameters.AddWithValue("@picture", imgb);
                    int n = cmd.ExecuteNonQuery();
                    if (n == 1)
                    {
                        conn.Close();
                        return true;
                    }

                }
            }
            //using中如果有错误，自动trycatch跑到这里来
            return false;
        }

        //添加图片的命令，一开始表中没有图片，事先添加好
        public static bool AddPicture(byte[] img)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.GetConnectionStr()))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "insert into tb_MainPgPic( picture) values( @picture)";
                    cmd.Parameters.AddWithValue("@picture", img);
                    int n = cmd.ExecuteNonQuery();
                    if (n == 1)
                    {
                        conn.Close();
                        return true;
                    }
                }
            }
            //using中如果有错误，自动trycatch跑到这里来
            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string imageSrc = @"C:\Users\Administrator\Desktop\2.jpg";
            byte[] imgb = LoadImageFile(imageSrc);
            UplodePicture(1, imgb);
            Image imageFromDB = GetPictureFromDB(1);
            this.pictureBox1.Image = GetPictureFromDB(1);
            downLoadImage(imageFromDB);
        }
        private void ChagePictureSize(int height, int width, byte[] imgb)
        {
            if (imgb != null)
            {
                MemoryStream ms = new MemoryStream(imgb);
                Image img = Image.FromStream(ms);


            }

        }

        //图片从数据库中下载完毕后就是Image类型的，直接保存即可
        private void downLoadImage(Image imageFromDB)
        {
            imageFromDB.Save(@"C:\Users\Administrator\Desktop\45.jpg");
        }

        //从数据库中指定Id获取图片对象
        private static Image GetPictureFromDB(int id)
        {
            byte[] pic = null;
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.GetConnectionStr()))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "select picture from tb_MainPgPic where id = " + id;
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            //这里必须接强转，否则会弹出“只有assiment之类奇怪的东西”
                            pic = (byte[])rd["picture"];
                        }
                    }
                }
            }
            if (pic == null)
            {
                return null;
            }
            //二进制数组-> 内存流 -> Image对象
            MemoryStream ms = new MemoryStream(pic);
            Image img = Image.FromStream(ms);
            return img;
        }
    }
}
