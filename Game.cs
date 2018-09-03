using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TorenVanHanoi
{
    public partial class Game : Form
    {
        Button[] Buttons = new Button[8];
        bool[] holding = { false, false, false, false, false, false, false, false };

        Stack<int>[] towers = new Stack<int>[3];
        Panel[] sticks = new Panel[3];

        public Stack<int> Locate(int num)
        {
            for (int i = 0; i < towers.Length; i++)
            {
                if (towers[i].Contains(num))
                {
                    return towers[i];
                }
            }
            throw new Exception();
        }

        public void Clip()
        {
            for (int t = 0; t < towers.Length; t++)
            {
                int[] unboxed = new int[towers[t].Count];
                (towers[t]).CopyTo(unboxed, 0);

                int Y = 560 - (towers[t].Count * 30);
                int X = sticks[t].Location.X + sticks[t].Size.Width / 2;
                for (int i = 0; i < towers[t].Count; i++)
                {
                    var target = Buttons[unboxed[i]];
                    target.Location = new Point(X - target.Size.Width/2,Y + i * 30);
                }
            }
        }

        public bool Colide(Button a, Panel b)
        {
            int a_r = a.Location.X + a.Size.Width;
            int a_l = a.Location.X;

            int b_r = b.Location.X + b.Size.Width;
            int b_l = b.Location.X;

            if (a_r > b_l && a_l < b_r)
            {
                return true;
            }
            return false;
        }

        public Game()
        {
            InitializeComponent();

            for (int i = 0; i < 3; i++) { towers[i] = new Stack<int>(); }

            sticks[0] = panel1;
            sticks[1] = panel2;
            sticks[2] = panel3;

            Buttons[0] = disk1; towers[0].Push(7);
            Buttons[1] = disk2; towers[0].Push(6);
            Buttons[2] = disk3; towers[0].Push(5);
            Buttons[3] = disk4; towers[0].Push(4);
            Buttons[4] = disk5; towers[0].Push(3);
            Buttons[5] = disk6; towers[0].Push(2);
            Buttons[6] = disk7; towers[0].Push(1);
            Buttons[7] = disk8; towers[0].Push(0);

            foreach (Button b in Buttons) b.MouseDown += Grabbed;
            foreach (Button b in Buttons) b.MouseUp += Released;

            Clip();
        }

        public void Released(object o, EventArgs args)
        {
            Button but = o as Button;
            int size = Convert.ToInt32(but.Text) - 1;

            for (int i = 0; i < 3; i++)
            {
                if (Colide(but,sticks[i]))
                {
                    var from = Locate(size);
                    var to = towers[i];

                    if (to.Count > 0)
                    {
                        //Dont place it if
                        if (to.Peek() < size)
                        {
                            break;
                        }
                    }
                    to.Push(from.Pop());
                }
            }
            holding[size] = false;
            Updater.Stop();
            Clip();
        }

        public void Grabbed(object o, EventArgs args)
        {
            Button Button = o as Button;
            int size = Convert.ToInt32(Button.Text) - 1;

            var f = Locate(size);
            if (size == f.Min())
            { 
                holding[size] = true;
                Updater.Start();
            }
        }

        private void Updater_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < holding.Length; i++)
            {
                if (holding[i])
                {
                    Buttons[i].Location = new Point(this.PointToClient(Cursor.Position).X-Buttons[i].Size.Width/2, this.PointToClient(Cursor.Position).Y-Buttons[i].Height/2);
                }
            }
        }
    }
}
