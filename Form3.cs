using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Таблица_умножения_Forms
{
    public partial class Form3 : Form
    {
        int heightY;
        int widthX;
        public Form3()
        {
            InitializeComponent();

            /*Rectangle screenSize = Screen.PrimaryScreen.Bounds;
            heightY = screenSize.Height;
            widthX = screenSize.Width;
            
           *//* if (screenSize.Height < 900 || screenSize.Width < 1600)*/

            pictureBox1.MouseMove += (s, e) => { pictureBox1.Size = new Size(366, 504); pictureBox1.Location = new Point(83, 27); };
            pictureBox1.MouseLeave += (s, e) => { pictureBox1.Size = new Size(346, 484); pictureBox1.Location = new Point(93, 37); };

            pictureBox2.MouseMove += (s, e) => { pictureBox2.Size = new Size(366, 504); pictureBox2.Location = new Point(546, 27); };
            pictureBox2.MouseLeave += (s, e) => { pictureBox2.Size = new Size(346, 484); pictureBox2.Location = new Point(556, 37); };

            pictureBox3.MouseMove += (s, e) => { pictureBox3.Size = new Size(366, 504); pictureBox3.Location = new Point(1000, 27); };
            pictureBox3.MouseLeave += (s, e) => { pictureBox3.Size = new Size(346, 484); pictureBox3.Location = new Point(1010, 37); };

            pictureBox4.MouseMove += (s, e) => { pictureBox4.Size = new Size(366, 504); pictureBox4.Location = new Point(1463, 27); };
            pictureBox4.MouseLeave += (s, e) => { pictureBox4.Size = new Size(346, 484); pictureBox4.Location = new Point(1473, 37); };

            pictureBox5.MouseMove += (s, e) => { pictureBox5.Size = new Size(366, 504); pictureBox5.Location = new Point(83, 546); };
            pictureBox5.MouseLeave += (s, e) => { pictureBox5.Size = new Size(346, 484); pictureBox5.Location = new Point(93, 556); };

            pictureBox6.MouseMove += (s, e) => { pictureBox6.Size = new Size(366, 504); pictureBox6.Location = new Point(546, 546); };
            pictureBox6.MouseLeave += (s, e) => { pictureBox6.Size = new Size(346, 484); pictureBox6.Location = new Point(556, 556); };

            pictureBox7.MouseMove += (s, e) => { pictureBox7.Size = new Size(366, 504); pictureBox7.Location = new Point(1000, 546); };
            pictureBox7.MouseLeave += (s, e) => { pictureBox7.Size = new Size(346, 484); pictureBox7.Location = new Point(1010, 556); };

            pictureBox8.MouseMove += (s, e) => { pictureBox8.Size = new Size(366, 504); pictureBox8.Location = new Point(1464, 546); };
            pictureBox8.MouseLeave += (s, e) => { pictureBox8.Size = new Size(346, 484); pictureBox8.Location = new Point(1474, 556); };
        }

        #region устраниение мерцания форм
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED       
                return handleParam;
            }
        }
        #endregion

        private void panel2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
