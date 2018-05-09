using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuronalesNetz_V0._5
{
    public partial class DisplayArr : Form
    {
        public DisplayArr()
        {
            InitializeComponent();
        }

        public void DisplayArr_Load(object sender, EventArgs e)
        {
            InitializeComponent();
        }

        public void DisplayInput(byte[] arr)
        {
            for(int i0 = 0; i0 < 500; ++i0)
            {
                for (int i1 = 0; i1 < 500; ++i1)
                {
                    ((Bitmap)InputPic.Image).SetPixel(i0, i1, Color.FromArgb(arr[1500 * i0 + i1 * 3], arr[1500 * i0 + i1 * 3 + 1], arr[1500 * i0 + i1 * 3 + 2]));
                }
            }


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
