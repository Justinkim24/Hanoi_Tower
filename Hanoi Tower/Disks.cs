using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hanoi_Tower
{
    class Disks
    {
        // A property is like a combination of a variable and a methond
        public int DiskNo { get; set; }
        public PictureBox box { get; set; }
        public int width { get; set; }
        public char peg { get; set; }
    }
}
