using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication1
{
    public partial class Menu : Form
    {
        private PlugBoard PlugBoardForm;
        /*em=Enigma Machine*/
        private Reflector emReflector;
        private Rotor[] emRotors;
        private int emRotorsAmount = 3;

        private string[] permutaionStringsArray = { "EKMFLGDQVZNTOWYHXUSPAIBRCJ",
                                                    "AJDKSIRUXBLHWTMCQGZNPYFVOE",
                                                    "BDFHJLCPRTXVZNYEIWGAKMUSQO",
                                                    "ESOVPZJAYQUIRHXLNFTGKDCMWB",
                                                    "VZBRGITYUPSDNHLXAWMJQOFECK"};
        private char[] turnoverNotchArray = {'Q',
                                             'E',
                                             'V',
                                             'J',
                                             'Z'};

        private string[] reflectorPermutaionArray = { "YRUHQSLDPXNGOKMIEBFZCWVJAT" };


        /* --------------------------------------Design Form-------------------------------*/
        /* For Form draggable pannel*/
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        /* end of draggable pannel inits*/

        /* Form Rounded Corners Inits*/
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
            (
                int nLeftRect,     // x-coordinate of upper-left corner
                int nTopRect,      // y-coordinate of upper-left corner
                int nRightRect,    // x-coordinate of lower-right corner
                int nBottomRect,   // y-coordinate of lower-right corner
                int nWidthEllipse, // height of ellipse
                int nHeightEllipse // width of ellipse
            );
        /* End Form Rounded Corners Inits*/
        /* ----------------------------------End Design Form-------------------------------*/

        /*------Form Constructor------*/
        public Menu()
        {
            InitializeComponent();
            /* Form Rounded Corners Inits*/
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            /* End Form Rounded Corners Inits*/
            this.InputTextBox.CharacterCasing = CharacterCasing.Upper;
            this.InputTextBox.PasswordChar = '*';
            this.PlugBoardForm = new PlugBoard();
            this.emReflector = new Reflector();
            this.emRotors = new Rotor[this.emRotorsAmount];
            for (int i = 0; i < this.emRotorsAmount; i++)
            {
                this.emRotors[i] = new Rotor();
            }
            resetAllComboBoxes();
        }
        public Menu(string str)
        {
            InitializeComponent();

            this.PlugBoardForm = new PlugBoard();
            this.emReflector = new Reflector();
            this.emRotors = new Rotor[this.emRotorsAmount];
            for (int i = 0; i < this.emRotorsAmount; i++)
            {
                this.emRotors[i] = new Rotor();
            }

            for (int i=0;i<1000;i++)
            {
                runEnigma("UMDPQ CUAQN LVVSP IARKC TTRJQ KCFPT OKRGO ZXALD RLPUH AUZSO SZFSU GWFNF DZCUG VEXUU LQYXO TCYRP SYGGZ HQMAG PZDKC KGOJM MYYDD H");
            }
            
        }
        /*------End of Form Constructor------*/

        /*---------------------Enigma Functions---------------------*/
        /*--------Setters-------*/
        private void setReflectorDetails()
        {
            this.emReflector.setName((char)ReflectorCombo.Text[0]);
            this.emReflector.setPermutation(reflectorPermutaionArray[0]);
        }
        private void setAllRotors()
        {
            for(int i=0;i<this.emRotorsAmount;i++)
            {
                setRotorByIndex(i);
            }
        }
        private void setRotorByIndex(int rotorIndex)
        {
            if(rotorIndex==0)
            {
                setFastRotorFromComboBoxes(rotorIndex);
                return;
            }
            else if(rotorIndex == 1)
            {
                setMiddleRotorFromComboBoxes(rotorIndex);
                return;
            }
            else if (rotorIndex == 2)
            {
                setSlowRotorFromComboBoxes(rotorIndex);
                return;
            }
        }
        private void setFastRotorFromComboBoxes(int rotorIndex)
        {
            this.emRotors[rotorIndex].setRotorName((char)FastRotorCombo.Text[0]);
            this.emRotors[rotorIndex].setRotorPermutation(permutaionStringsArray[FastRotorCombo.SelectedIndex]);
            this.emRotors[rotorIndex].setRotorTurnoverNotch(turnoverNotchArray[FastRotorCombo.SelectedIndex]-65);
            this.emRotors[rotorIndex].setRotorSetting((char)FastRotorSettingCombo.Text[0]);
            this.emRotors[rotorIndex].setRotorOffset((int)FastRotorOffsetCombo.Text[0] - 65);
            this.emRotors[rotorIndex].setRotorArrays();

        }
        private void setFastRotorRandom(int rotorIndex=0)
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 6);
            int randomNumber2 = random.Next(65, 91);
            int randomNumber3 = random.Next(0, 26);
            this.emRotors[rotorIndex].setRotorName('1');
            this.emRotors[rotorIndex].setRotorPermutation(permutaionStringsArray[randomNumber]);
            this.emRotors[rotorIndex].setRotorTurnoverNotch(turnoverNotchArray[randomNumber] - 65);
            this.emRotors[rotorIndex].setRotorSetting((char)randomNumber2);
            this.emRotors[rotorIndex].setRotorOffset(randomNumber3);
            this.emRotors[rotorIndex].setRotorArrays();

        }
        private void setMiddleRotorFromComboBoxes(int rotorIndex)
        {
            this.emRotors[rotorIndex].setRotorName((char)MiddleRotorCombo.Text[0]);
            this.emRotors[rotorIndex].setRotorPermutation(permutaionStringsArray[MiddleRotorCombo.SelectedIndex]);
            this.emRotors[rotorIndex].setRotorTurnoverNotch(turnoverNotchArray[MiddleRotorCombo.SelectedIndex] - 65);
            this.emRotors[rotorIndex].setRotorSetting((char)MiddleRotorSettingCombo.Text[0]);
            this.emRotors[rotorIndex].setRotorOffset((int)MiddleRotorOffsetCombo.Text[0] - 65);
            this.emRotors[rotorIndex].setRotorArrays();
        }
        private void setMiddleRotorRandom(int rotorIndex=1)
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 6);
            int randomNumber2 = random.Next(65, 91);
            int randomNumber3 = random.Next(0, 26);
            this.emRotors[rotorIndex].setRotorName('2');
            this.emRotors[rotorIndex].setRotorPermutation(permutaionStringsArray[randomNumber]);
            this.emRotors[rotorIndex].setRotorTurnoverNotch(turnoverNotchArray[randomNumber] - 65);
            this.emRotors[rotorIndex].setRotorSetting((char)randomNumber2);
            this.emRotors[rotorIndex].setRotorOffset(randomNumber3);
            this.emRotors[rotorIndex].setRotorArrays();
        }
        private void setSlowRotorFromComboBoxes(int rotorIndex)
        {
            this.emRotors[rotorIndex].setRotorName((char)SlowRotorCombo.Text[0]);
            this.emRotors[rotorIndex].setRotorPermutation(permutaionStringsArray[SlowRotorCombo.SelectedIndex]);
            this.emRotors[rotorIndex].setRotorTurnoverNotch(turnoverNotchArray[SlowRotorCombo.SelectedIndex] - 65);
            this.emRotors[rotorIndex].setRotorSetting((char)SlowRotorSettingCombo.Text[0]);
            this.emRotors[rotorIndex].setRotorOffset((int)SlowRotorOffsetCombo.Text[0] - 65);
            this.emRotors[rotorIndex].setRotorArrays();
        }
        private void setSlowRotorRandom(int rotorIndex=2)
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 6);
            int randomNumber2 = random.Next(65, 91);
            int randomNumber3 = random.Next(0, 26);
            this.emRotors[rotorIndex].setRotorName('3');
            this.emRotors[rotorIndex].setRotorPermutation(permutaionStringsArray[randomNumber]);
            this.emRotors[rotorIndex].setRotorTurnoverNotch(turnoverNotchArray[randomNumber] - 65);
            this.emRotors[rotorIndex].setRotorSetting((char)randomNumber2);
            this.emRotors[rotorIndex].setRotorOffset(randomNumber3);
            this.emRotors[rotorIndex].setRotorArrays();
        }
        /*----End of Setters----*/
        public void runEnigma(string loopString=null)
        {
            char[] inputString;
            inputString = InputTextBox.Text.ToCharArray();          
            char _encryptedChar;
            OutputTextBox.Text = "";
            for (int i=0;i< inputString.Length;i++)
            {
                if (inputString[i] == ' ')
                {
                    OutputTextBox.Text = OutputTextBox.Text + ' ';
                    continue;
                }
                else
                {
                    _encryptedChar = this.PlugBoardForm.getPermutation().ToCharArray()[inputString[i] - 65];
                    _encryptedChar = this.emRotors[0].encryptChar(_encryptedChar, true);
                    _encryptedChar = this.emRotors[1].encryptChar(_encryptedChar, this.emRotors[0].getIsShift());
                    _encryptedChar = this.emRotors[2].encryptChar(_encryptedChar, this.emRotors[1].getIsShift());
                    _encryptedChar = this.emReflector.getReflectorPermutation().ToCharArray()[(int)_encryptedChar - 65];
                    _encryptedChar = this.emRotors[2].encryptCharRevers(_encryptedChar);
                    _encryptedChar = this.emRotors[1].encryptCharRevers(_encryptedChar);
                    _encryptedChar = this.emRotors[0].encryptCharRevers(_encryptedChar);
                    _encryptedChar = this.PlugBoardForm.getPermutation().ToCharArray()[_encryptedChar - 65];

                    OutputTextBox.Text = OutputTextBox.Text + _encryptedChar;
                    continue;
                }
            }
            FastRotorOffsetCombo.SelectedIndex = this.emRotors[0].getRotorOffset();
            MiddleRotorOffsetCombo.SelectedIndex = this.emRotors[1].getRotorOffset();
            SlowRotorOffsetCombo.SelectedIndex = this.emRotors[2].getRotorOffset();
            //OutputTextBox.Text = OutputTextBox.Text;
        }
        /*------------------End of Enigma Functions------------------*/

        /*------Reset Functions for all combo boxes------*/
        private void resetReflectorCombo()
        {
            ReflectorCombo.Items.Insert(0, 'B');
            ReflectorCombo.SelectedIndex = 0;
        }
        private void resetRotorCombo(ComboBox ComboBoxToReset)
        {
            ComboBoxToReset.Items.Insert(0, "I");
            ComboBoxToReset.Items.Insert(1, "II");
            ComboBoxToReset.Items.Insert(2, "III");
            ComboBoxToReset.Items.Insert(3, "IV");
            ComboBoxToReset.Items.Insert(4, "V");
            ComboBoxToReset.SelectedIndex = 0;
        }
        private void resetRotorAlphaBetaCombo(ComboBox ComboBoxToReset)
        {
            for(int i=0;i<26;i++)
            {
                ComboBoxToReset.Items.Insert(i, (char)(65+i));
            }
            ComboBoxToReset.SelectedIndex = 0;
        }
        private void resetAllComboBoxes()
        {
            //Reflector
            resetReflectorCombo();
            //Slow Rotor
            resetRotorCombo(SlowRotorCombo);
            resetRotorAlphaBetaCombo(SlowRotorSettingCombo);
            resetRotorAlphaBetaCombo(SlowRotorOffsetCombo);
            //Middle Rotor
            resetRotorCombo(MiddleRotorCombo);
            resetRotorAlphaBetaCombo(MiddleRotorSettingCombo);
            resetRotorAlphaBetaCombo(MiddleRotorOffsetCombo);
            //Fast Rotor
            resetRotorCombo(FastRotorCombo);
            resetRotorAlphaBetaCombo(FastRotorSettingCombo);
            resetRotorAlphaBetaCombo(FastRotorOffsetCombo);
        }
        /*------End of Reset Functions for all combo boxes------*/

        /*------Text boxes change functions------*/
        private void InputTextBox_TextChanged(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"^[a-zA-Z ]*$");
            if (!regex.Match(InputTextBox.Text).Success && InputTextBox.Text!=null)
            {
                MessageBox.Show("This textbox accepts only english alphabetical characters");
                InputTextBox.Text = InputTextBox.Text.Remove(InputTextBox.Text.Length - 1);
                InputTextBox.SelectionStart = InputTextBox.Text.Length;
            }
        }
        private void OutputTextBox_TextChanged(object sender, EventArgs e)
        {

        }
        /*------End of Text boxes change functions------*/

        /*------Button click event functions------*/
        private void RunButton_Click(object sender, EventArgs e)
        {
            /*Set Enigma components Values*/
            setReflectorDetails();
            setAllRotors();
            /*End of Set Enigma components Values*/
            runEnigma();
            //OutputTextBox.Text = "PlugBoard:"+this.PlugBoardForm.getPermutation()+
            //                     "\n Reflector: " + this.emReflector.getReflectorPermutation()+
            //                     "\n Fast Rotor: " + this.emRotors[0].getRotorPermutation()+
            //                     ", "+ this.emRotors[0].getRotorSetting()+
            //                     ", "+ this.emRotors[0].getRotorOffset()+
            //                     ", " + this.emRotors[0].getRotorTurnoverNotch();
        }
        private void ShowHideButton_Click(object sender, EventArgs e)
        {
            if(InputTextBox.PasswordChar== default(char))
            {
                InputTextBox.PasswordChar = '*';
            }
            else
            {
                InputTextBox.PasswordChar = default(char);
            }
        }
        private void PlugboardSettingsButton_Click(object sender, EventArgs e)
        {
            PlugBoardForm.Show();
        }
        private void CloseButton_Click(object sender, EventArgs e)
        {
            PlugBoardForm.Close();
            this.Close();
        }
        private void MinimizeButton_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        /*------End of Button click event functions------*/

        /* Form Panel dragable functions*/
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }
        /* End of Form Panel dragable functions*/
    }
}
