using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hanoi_Tower
{
    public partial class Form1 : Form
    {
        //create List moves and List_TowerDisk
        private List<string> moves = new List<string>();
        private List<Disks> _TowerDisks = new List<Disks>();
        AnimateView animate = new AnimateView();
        //Make Disk start 1, set Disksize(Hieght,Weight)
        int _DiskCount = 1;
        int diskHeight = 40;
        int baseHeight = 20;

        public Form1()
        {
            //Automatically Created
            InitializeComponent();
            //Operating Animation on Panel1 
            AnimateView.view = panel1;
            //Reset as like clean Panel1 after view Animate
            resetPanel();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        // Create disks on Panel
        private void populateDisks()
        {
            int kj = 1;
            foreach (Disks disk in _TowerDisks)
            {
                PictureBox panelBox = disk.box;
                panelBox.BackColor = colorSelector(disk);
                disk.width = 200 - (20 * kj);
                panelBox.Width = disk.width;
                panelBox.Height = diskHeight;
                panelBox.Tag = disk.DiskNo;
                panelBox.BorderStyle = BorderStyle.FixedSingle;
                Point boxLocation = new Point(getDiskX(disk), (panel1.Height - baseHeight) - (diskHeight * kj++));
                panelBox.Location = boxLocation;
                disk.box = panelBox;
                panel1.Controls.Add(disk.box);
            }
        }
        //Get X position for Disk
        private int getDiskX(Disks disk)
        {
            int X = 0;
            int peg = 0;
            switch (disk.peg)
            {
                case 'A': peg = 1; break;
                case 'B': peg = 2; break;
                case 'C': peg = 3; break;
            }
            X = ((panel1.Width / 4) * peg) - (int)(disk.width / 2);
            return X;
        }
        //Reset the Panel
        private void resetPanel()
        {   //Create pegs for Tower
            setupTower();
            panel1.Controls.Clear();
            _TowerDisks = Enumerable.Range(1, _DiskCount).Select(i => new Disks() { DiskNo = i, peg = 'A', box = new PictureBox() }).OrderByDescending(i => i.DiskNo).ToList();
            //Place Disks on panel
            populateDisks();
            //Set initial text value for least possible moves
            lblCounter.Text = string.Format("Best Possible moves{0}", GetMoveCount(_DiskCount));
        }
        //Get Disk Y position
        private int getDiskY(Disks disk)
        {
            int Y = 0;
            int stackNo = _TowerDisks.Count(x => x.peg == disk.peg);
            Y = panel1.Height - baseHeight - (diskHeight * stackNo);
            return Y;
        }
        //Change Disk Color with Rainbow order
        private Color colorSelector(Disks disk)
        {
            switch (disk.DiskNo)
            {
                case 1: return Color.Red;
                case 2: return Color.Orange;
                case 3: return Color.Yellow;
                case 4: return Color.Green;
                case 5: return Color.Blue;
                default: return Color.White;
            }
        }
        //Button click event to slove tower
        private void btnSolve_Click(object sender, EventArgs e)
        {
            resetPanel();
            btnSolve.Enabled = false;
            moves.Clear();
            int NumberOfDisks = _DiskCount;
            solveTower(NumberOfDisks);
            listMoves.DataSource = null;
            listMoves.DataSource = moves;
            btnSolve.Enabled = true;
        }
        //Create tower values, process tower and display moves required
        private void solveTower(int numberOfDisks)
        {
            char aPeg = 'A'; //start tower in output
            char cPeg = 'C'; //end tower in output
            char bPeg = 'B'; //temporary tower in output
            //Solve towers using recursive method
            solveTowers(numberOfDisks, aPeg, cPeg, bPeg);
        }
        //Solve towers using recursive method (aPeg->cPeg)
        private void solveTowers(int n, char aPeg, char cPeg, char bPeg)
        {
            if (n > 0)
            {
                solveTowers(n - 1, aPeg, bPeg, cPeg);
                Disks currentDisk = _TowerDisks.Find(x => x.DiskNo == n);
                currentDisk.peg = cPeg;
                //Animate
                animate.moveUp(currentDisk.box, 50);
                if (aPeg < cPeg)//move Right
                    animate.moveRight(currentDisk.box, getDiskX(currentDisk));
                else//move Left
                    animate.moveLeft(currentDisk.box, getDiskX(currentDisk));
                animate.moveDown(currentDisk.box, getDiskY(currentDisk));
                //Format line
                string line = string.Format("Move disk {0} from {1} to{2}", n, aPeg, cPeg);
                Console.WriteLine(line);
                moves.Add(line);
                solveTowers(n - 1, bPeg, cPeg, aPeg);
            }
        }
        //get the least amount of moves required to solve the tower
        public static int GetMoveCount(int numberOfDisks)
        {   //Mersenne number(2**n)-1 | time complexity O(2**n)
            double numberOfDisks_Double = numberOfDisks;
            return (int)Math.Pow(2.0, numberOfDisks_Double) - 1; //return number of move
        }
        private void DiskCount_ValueChanged(object sender, EventArgs e)
        {
            _DiskCount = (int)DiskCount.Value;
            resetPanel();
        }
        //Paint base to panel1
        private void setupTower()
        {
            panel1.Paint += delegate
            {
                setBase();
            };
        }
        //Draw base and pegs
        private void setBase()
        {
            SolidBrush sb = new SolidBrush(Color.MediumPurple);
            Graphics g = panel1.CreateGraphics();
            int topSpacing = 120;
            int width = 40;
            //Draw bottom bar
            g.FillRectangle(sb, 0, panel1.Height - baseHeight, panel1.Width, baseHeight);
            //Draw Peg1
            drawPeg(panel1, g, sb, 1, width, topSpacing);
            //Draw Peg2
            drawPeg(panel1, g, sb, 2, width, topSpacing);
            //Draw Peg3
            drawPeg(panel1, g, sb, 3, width, topSpacing);
        }
        //Ddraw a peg
        private void drawPeg(Panel canvas, Graphics g, SolidBrush sb, int pegNo, int pegWidth, int topSpacing)
        //canvas is Panel to draw pegs on, g is Panel.CreateGraphics, sb is SolidBrush
        //PegNo is Peg Number 1-3, pegWidth is Desired peg width, topSpacing is Spacing from the top
        {
            g.FillRectangle(sb, ((int)(canvas.Width / 4) * pegNo) - (pegWidth / 2), topSpacing, pegWidth, canvas.Height - topSpacing);
        }
        //Value changed for informational label
        private void DiskCount_ValueChanged_1(object sender, EventArgs e)
        {
            _DiskCount = (int)DiskCount.Value;
            resetPanel();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Thank you! Good bye!");
            Application.Exit();
        }
    }
}
